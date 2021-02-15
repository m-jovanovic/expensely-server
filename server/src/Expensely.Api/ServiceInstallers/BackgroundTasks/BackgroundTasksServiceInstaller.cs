using Expensely.Api.Abstractions;
using Expensely.Messaging.Factories;
using Expensely.Messaging.Jobs;
using Expensely.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Expensely.Api.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the background tasks services installer.
    /// </summary>
    public sealed class BackgroundTasksServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<QuartzHostedServiceOptionsSetup>();

            services.ConfigureOptions<MessageProcessingJobSettingsSetup>();

            services.AddQuartz(configure => configure.UseMicrosoftDependencyInjectionScopedJobFactory());

            services.AddQuartzHostedService();

            services.AddTransient<MessageProcessingJob>();

            services.AddTransient<IEventHandlerFactory, EventHandlerFactory>();

            services.AddScoped<IMessageDispatcher, MessageDispatcher>();
        }
    }
}
