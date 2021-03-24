using Expensely.BackgroundTasks.MessageProcessing.Abstractions;
using Expensely.BackgroundTasks.MessageProcessing.Settings;
using Microsoft.Extensions.Options;
using Quartz;

namespace Expensely.App.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the <see cref="IMessageProcessingJob"/> setup.
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
            var jobKey = new JobKey(IMessageProcessingJob.Name);

            options.AddJob<IMessageProcessingJob>(jobBuilder => jobBuilder.WithIdentity(jobKey));

            options.AddTrigger(triggerBuilder => triggerBuilder
                .ForJob(jobKey)
                .WithSimpleSchedule(scheduleBuilder =>
                    scheduleBuilder.WithIntervalInSeconds(_messageProcessingJobSettings.IntervalInSeconds).RepeatForever()));
        }
    }
}
