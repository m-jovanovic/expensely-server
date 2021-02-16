using Expensely.BackgroundTasks.Factories;
using Expensely.BackgroundTasks.Jobs;
using Expensely.BackgroundTasks.Services;
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

            services.AddQuartz(configure => configure.UseMicrosoftDependencyInjectionScopedJobFactory());

            services.AddQuartzHostedService();

            services.AddTransient<MessageProcessingJob>();

            services.AddTransient<IEventHandlerFactory, EventHandlerFactory>();

            services.AddScoped<IMessageDispatcher, MessageDispatcher>();
        }
    }
}
