using System;
using System.IO;
using System.Reflection;
using Expensely.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Expensely.App
{
    /// <summary>
    /// Represents the program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry-point for the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Creating host.");

                IHost host = CreateHostBuilder(args).Build();

                Log.Information("Configuring logger.");

                using (IServiceScope scope = host.Services.CreateScope())
                {
                    ILoggerConfigurator loggerConfigurator = scope.ServiceProvider.GetRequiredService<ILoggerConfigurator>();

                    loggerConfigurator.Configure();
                }

                Log.Information("Application starting.");

                host.Run();

                Log.Information("Application started.");
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Application terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration(WithApplicationConfiguration)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static void WithApplicationConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", true, true);

            if (hostBuilderContext.HostingEnvironment.IsDevelopment())
            {
                configurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            }

            configurationBuilder.AddEnvironmentVariables();
        }
    }
}
