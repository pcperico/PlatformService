using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httClient,IConfiguration configuration)
        {
            _httClient = httClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platfrom)
        {
            var httContent = new StringContent(
                JsonSerializer.Serialize(platfrom),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httClient.PostAsync(
                 $"{_configuration["CommandService"]}",
                httContent
            );
            if(response.IsSuccessStatusCode)
            
                Console.WriteLine("--> sync Post to CommandService was OK!");            
            else
                Console.WriteLine("--> sync Post to CommandService was NOT OK!");
                            
        }
    }
}
