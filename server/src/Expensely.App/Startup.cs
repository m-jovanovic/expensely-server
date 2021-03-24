using System.Reflection;
using Expensely.App.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Expensely.App
{
    /// <summary>
    /// Represents the application startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the application services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services) =>
            services.InstallServicesFromAssembly(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLogContextEnrichment();

            app.UseSerilogRequestLogging();

            app.UseGlobalExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(configure => configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwagger();

            app.UseSwaggerUI();
        }
    }
}
