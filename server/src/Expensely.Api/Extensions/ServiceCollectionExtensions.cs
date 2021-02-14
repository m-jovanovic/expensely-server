﻿using System;
using System.Reflection;
using Expensely.Api.Abstractions;
using Expensely.Api.Infrastructure;
using Expensely.Domain.Abstractions.Extensions;
using Expensely.Domain.Factories;
using Expensely.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Expensely.Api.Extensions
{
    /// <summary>
    /// Contains extensions methods for the service collection class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Install all of the services from the specified assembly.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly to install services from.</param>
        public static void InstallServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            IInstaller[] installers = InstallerFactory.GetInstallersFromAssembly(assembly);

            installers.ForEach(x => x.InstallServices(services));
        }

        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<ITransactionDetailsValidator, TransactionDetailsValidator>();

            services.AddTransient<ITransactionFactory, TransactionFactory>();

            return services;
        }

        /// <summary>
        /// Configures the Swagger services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Expensely API",
                    Version = "v1"
                });

                swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
