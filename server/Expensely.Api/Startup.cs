using System;
using System.Data;
using Dapper;
using Expensely.Api.Behaviors;
using Expensely.Api.Controllers;
using Expensely.Api.Extensions;
using Expensely.Application.Commands.Handlers;
using Expensely.Application.Events.Handlers;
using Expensely.Application.Queries.Handlers;
using Expensely.Infrastructure;
using Expensely.Messaging.BackgroundServices;
using Expensely.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Expensely.Api
{
    /// <summary>
    /// Represents the application startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration) => Configuration = configuration;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the application services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);

            services
                .AddInfrastructure(Configuration)
                .AddPersistence(Configuration);

            services.AddValidatorsFromAssembly(CommandHandlersAssembly.Assembly);

            services.AddMediatR(CommandHandlersAssembly.Assembly, QueryHandlersAssembly.Assembly);

            services.AddEventHandlers();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddHostedService<MessageProcessingBackgroundService>();

            services.AddControllers()
                .AddApplicationPart(ControllersAssembly.Assembly);

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddSwagger();
        }

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

                app.ConfigureSwagger();
            }

            app.ApplyMigrations();

            app.UseCors(configure =>
                configure
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
