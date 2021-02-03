using System;
using System.IO;
using System.Reflection;
using Expensely.Api.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Serilog;

namespace Expensely.Api
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
                IHost host = CreateHostBuilder(args).Build();

                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IConfiguration configuration = scope.ServiceProvider.GetService<IConfiguration>();

                    IDocumentStore documentStore = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

                    LoggerConfigurator.Configure(configuration, documentStore);
                }

                Log.Information("Application starting.");

                host.Run();
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

        /// <summary>
        /// Creates the host builder for the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration(WithApplicationConfiguration)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        /// <summary>
        /// Configures the application configuration.
        /// </summary>
        /// <param name="hostBuilderContext">The host builder context.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
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
