using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.Data;
using PlatformService.Data.Repositories.Impl;
using PlatformService.Data.Repositories.Interfaces;
using PlatformService.SyncDataServices.Http;
using System;

namespace PlatformService
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                Console.WriteLine("---> Using InMem DB");
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
            }
            if (_env.IsProduction())
            {
                Console.WriteLine("----> Using SQL Server DB");
                services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PlatformsConn")));
            }
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> Command Service Endpoint {Configuration["CommandService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app,env.IsProduction());
        }
    }
}
