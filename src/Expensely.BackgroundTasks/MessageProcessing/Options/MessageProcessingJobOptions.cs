namespace Expensely.BackgroundTasks.MessageProcessing.Options
{
    /// <summary>
    /// Represents the message processing job configuration settings.
    /// </summary>
    public sealed class MessageProcessingJobOptions
    {
        /// <summary>
        /// Gets the interval in seconds.
        /// </summary>
        public int IntervalInSeconds { get; init; }

        /// <summary>
        /// Gets the batch size.
        /// </summary>
        public int BatchSize { get; init; }

        /// <summary>
        /// Gets the retry count threshold.
        /// </summary>
        public int RetryCountThreshold { get; init; }
    }
}
