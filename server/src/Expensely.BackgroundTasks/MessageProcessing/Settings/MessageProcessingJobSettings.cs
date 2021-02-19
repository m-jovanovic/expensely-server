namespace Expensely.BackgroundTasks.MessageProcessing.Settings
{
    /// <summary>
    /// Represents the message processing job configuration settings.
    /// </summary>
    public sealed class MessageProcessingJobSettings
    {
        /// <summary>
        /// Gets the interval in seconds.
        /// </summary>
        public int IntervalInSeconds { get; init; }

        /// <summary>
        /// Gets the batch size.
        /// </summary>
        public int BatchSize { get; init; }
    }
}
