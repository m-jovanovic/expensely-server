using Expensely.BackgroundTasks.MessageProcessing;
using Expensely.BackgroundTasks.MessageProcessing.Factories;
using Expensely.BackgroundTasks.MessageProcessing.Services;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Expensely.WebApp.ServiceInstallers.BackgroundTasks
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

            services.ConfigureOptions<MessageProcessingJobSetup>();

            services.AddQuartz(configure => configure.UseMicrosoftDependencyInjectionScopedJobFactory());

            services.AddQuartzHostedService();

            services.AddTransient<MessageProcessingJob>();

            services.AddTransient<IEventHandlerFactory, EventHandlerFactory>();

            services.AddScoped<IMessageDispatcher, MessageDispatcher>();
        }
    }
}
