using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseConfiguration(GetConfiguration(args))
                        .UseStartup<Startup>();
                });
        }

        private static IConfigurationRoot GetConfiguration(string[] args)
        {
            var confBuilder = new ConfigurationBuilder();

            confBuilder.AddJsonFile("sharedappsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env != null)
            {
                confBuilder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

                if (env == Environments.Development)
                {
                    var appAssembly = Assembly.Load(new AssemblyName(Assembly.GetEntryAssembly().GetName().Name));

                    if (appAssembly != null)
                    {
                        confBuilder.AddUserSecrets(appAssembly, optional: true);
                    }
                }
            }

            confBuilder.AddEnvironmentVariables()
                .AddCommandLine(args);

            return confBuilder.Build();
        }
    }
}
