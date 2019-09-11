using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace sis_receiver_api
{
    public class Program
    {
        private static string _env;

        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string json_config = "appsettings.json";

            if (args.Length > 0)
            {
                json_config = args[0];
            }

            if (!File.Exists(json_config))
            {
                throw new Exception("[Error] configuration file (appsettings.json) : " + json_config + " is not found");
            }

            Console.WriteLine("Using Configuration " + json_config);
            IConfiguration configuration;
            configuration = new ConfigurationBuilder()
            .AddJsonFile(json_config, true)
            .Build();

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls(configuration["ServerUrls"])
                .ConfigureServices(services => services.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile(json_config, true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseStartup<Startup>()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.ClearProviders();
                    _env = hostContext.HostingEnvironment.EnvironmentName;
                });
        }
    }
}
