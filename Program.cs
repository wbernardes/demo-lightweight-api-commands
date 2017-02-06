using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoLightweightApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
               .UseKestrel()
               .UseIISIntegration()
               .UseConfiguration(config)
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureLogging(logger =>
               {
                   logger.AddConsole(config.GetSection("Logging"));
               })
               .ConfigureServices(services =>
               {
                   services.AddRouting();
                   services.AddMediatR();
               })
               .Configure(app =>
               {
                   app.ConfigureRouting("api/{action}");
                   app.Run(async context =>
                   {
                       await context.Response.WriteAsync("Api is online!");
                   });
               })
               .Build();

            host.Run();
        }
    }
}
