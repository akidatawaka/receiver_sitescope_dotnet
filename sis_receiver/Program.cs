using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sis_receiver.Entities.Config;
using Serilog;
using System.IO;
using sis_receiver.Services;
using System;

namespace sis_receiver
{
    class Program
    {
        private static string _env;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            try
            {
                IHost host = new HostBuilder()
                     .ConfigureHostConfiguration(configHost =>
                     {
                         configHost.SetBasePath(Directory.GetCurrentDirectory());
                         configHost.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                         configHost.AddCommandLine(args);
                     })
                     .ConfigureAppConfiguration((hostContext, configApp) =>
                     {
                         configApp.SetBasePath(Directory.GetCurrentDirectory());
                         configApp.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                         string json_config = "appsettings.json";
                         if (args.Length > 0)
                         {
                             json_config = args[0];
                         }

                         if (!File.Exists(json_config))
                         {
                             throw new Exception("[Error] configuration file (appsettings.json) : " + json_config + " is not found");
                         }
                         else
                         {
                             configApp.AddJsonFile(json_config, true);
                         }
                         configApp.AddCommandLine(args);
                     })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddOptions();
                        services.Configure<SisReceiverConfig>(hostContext.Configuration);
                        services.AddLogging();
                        services.AddSingleton(typeof(IHostedService), typeof(SisReceiverService));
                    })
                    .ConfigureLogging((hostContext, configLogging) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(hostContext.Configuration)
                                .CreateLogger();

                        configLogging.ClearProviders();
                        _env = hostContext.HostingEnvironment.EnvironmentName;
                    })
                    .Build();

                await host.RunAsync();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

        }
    }
}
