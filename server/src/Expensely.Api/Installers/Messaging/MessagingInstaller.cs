﻿using System;
using Expensely.Api.Abstractions;
using Expensely.Api.Behaviors;
using Expensely.Application.Commands.Handlers;
using Expensely.Application.Events.Handlers;
using Expensely.Application.Queries.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.Installers.Messaging
{
    /// <summary>
    /// Represents the messaging installer.
    /// </summary>
    internal sealed class MessagingInstaller : IInstaller
    {
        private const string EventHandlerPostfix = "EventHandler";

        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.AddMediatR(CommandHandlersAssembly.Assembly, QueryHandlersAssembly.Assembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            AddEventHandlers(services);
        }

        private static void AddEventHandlers(IServiceCollection services) =>
            services.Scan(scan =>
                scan.FromAssemblies(EventHandlersAssembly.Assembly)
                    .AddClasses(filter =>
                        filter.Where(type => type.Name.EndsWith(EventHandlerPostfix, StringComparison.Ordinal)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
    }
}
