using Expensely.BackgroundTasks.Jobs;
using Expensely.BackgroundTasks.Settings;
using Microsoft.Extensions.Options;
using Quartz;

namespace Expensely.WebApp.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the <see cref="MessageProcessingJob"/> setup.
    /// </summary>
    public sealed class MessageProcessingJobSetup : IPostConfigureOptions<QuartzOptions>
    {
        private readonly MessageProcessingJobSettings _messageProcessingJobSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessingJobSetup"/> class.
        /// </summary>
        /// <param name="messageProcessingJobSettingsOptions">The message processing job settings.</param>
        public MessageProcessingJobSetup(IOptions<MessageProcessingJobSettings> messageProcessingJobSettingsOptions) =>
            _messageProcessingJobSettings = messageProcessingJobSettingsOptions.Value;

        /// <inheritdoc />
        public void PostConfigure(string name, QuartzOptions options)
        {
            var jobKey = new JobKey(nameof(MessageProcessingJob));

            options.AddJob<MessageProcessingJob>(jobBuilder => jobBuilder.WithIdentity(jobKey));

            options.AddTrigger(triggerBuilder => triggerBuilder
                .ForJob(jobKey)
                .WithSimpleSchedule(scheduleBuilder =>
                    scheduleBuilder.WithIntervalInSeconds(_messageProcessingJobSettings.IntervalInSeconds).RepeatForever()));
        }
    }
}
