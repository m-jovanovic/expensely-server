using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        /// <summary>
        /// Creates the host builder for the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder
                        .UseSerilog()
                        .UseStartup<Startup>());
    }
}
