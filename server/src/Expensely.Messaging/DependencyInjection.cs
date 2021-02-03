using System.Globalization;
using Expensely.Messaging.Factories;
using Expensely.Messaging.Jobs;
using Expensely.Messaging.Services;
using Expensely.Messaging.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Expensely.Messaging
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        private const string MessageProcessingJobIntervalInSecondsSetting = "Messaging:MessageProcessingJob:IntervalInSeconds";

        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessageProcessingJobSettings>(configuration.GetSection(MessageProcessingJobSettings.SettingsKey));

            services.AddQuartz(quartzConfigurator =>
            {
                quartzConfigurator.UseMicrosoftDependencyInjectionScopedJobFactory();

                var jobKey = new JobKey(nameof(MessageProcessingJob));

                quartzConfigurator.AddJob<MessageProcessingJob>(jobKey);

                quartzConfigurator.AddTrigger(triggerConfigurator =>
                    triggerConfigurator.ForJob(jobKey).WithSimpleSchedule(scheduleBuilder =>
                    {
                        int seconds = int.Parse(configuration[MessageProcessingJobIntervalInSecondsSetting], CultureInfo.InvariantCulture);

                        scheduleBuilder.WithIntervalInSeconds(seconds).RepeatForever();
                    }));
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            services.AddTransient<IEventHandlerFactory, EventHandlerFactory>();

            services.AddScoped<IMessageDispatcher, MessageDispatcher>();

            return services;
        }
    }
}
