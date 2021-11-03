using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private IConnection connection;
        private IModel channel;

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ connection shut down");
        
        }

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port= int.Parse( _configuration["RabbitMQPort"])
            };
            try 
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
                Console.WriteLine("--> Connected to MesssageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to message bus: {ex.Message}");
            }
        }
        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var messageObject = JsonSerializer.Serialize(platformPublishedDto);
            if(connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ conn open, sending message...");
                SendMessage(messageObject);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ conn is closed, not sending message...");

            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"--> Message sent: {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus disposed");
            if(channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }

        }
    }
}
