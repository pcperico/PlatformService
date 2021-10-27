using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data{
    public static class PrepDb{
        public static void PrepPopulation(IApplicationBuilder app,bool isProd){
            using (var serviceScoope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScoope.ServiceProvider.GetService<AppDbContext>(),isProd);
            }
        }

        private static void SeedData(AppDbContext context,bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("---> Attempting to apply migrations....");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error applying migration {ex.Message}");
                }
                
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--->Seeding data....");
                context.Platforms.AddRange(
                    new Platform(){Name = "Dot Net",Publisher = "Microsoft",Cost = "Free"},
                    new Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();

            }
        }
    }
}