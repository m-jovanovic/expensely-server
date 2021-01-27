using Expensely.Messaging.Factories;
using Expensely.Messaging.Jobs;
using Expensely.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Expensely.Messaging
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddQuartz(quartzConfigurator =>
            {
                quartzConfigurator.UseMicrosoftDependencyInjectionScopedJobFactory();

                var jobKey = new JobKey(nameof(MessageProcessingJob));

                quartzConfigurator.AddJob<MessageProcessingJob>(jobKey);

                quartzConfigurator.AddTrigger(triggerConfigurator =>
                    triggerConfigurator
                        .ForJob(jobKey)
                        .WithSimpleSchedule(scheduleBuilder =>
                            scheduleBuilder
                                .WithIntervalInSeconds(15)
                                .RepeatForever()));
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            services.AddTransient<IEventHandlerFactory, EventHandlerFactory>();

            services.AddScoped<IMessageDispatcher, MessageDispatcher>();

            return services;
        }
    }
}
