using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder<Startup>(args)
                .UseConfiguration(GetConfiguration(args));
        }

        private static IConfigurationRoot GetConfiguration(string[] args)
        {
            var confBuilder = new ConfigurationBuilder();

            confBuilder.AddJsonFile("sharedappsettings.json", optional: true, reloadOnChange: true);
            confBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env != null)
            {
                confBuilder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

                if (env == EnvironmentName.Development)
                {
                    var appAssembly = Assembly.Load(new AssemblyName(Assembly.GetEntryAssembly().GetName().Name));

                    if (appAssembly != null)
                    {
                        confBuilder.AddUserSecrets(appAssembly, optional: true);
                    }
                }
            }

            confBuilder.AddEnvironmentVariables();
            confBuilder.AddCommandLine(args);

            return confBuilder.Build();
        }
    }
}
