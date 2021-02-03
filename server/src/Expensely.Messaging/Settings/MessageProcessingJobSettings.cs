namespace Expensely.Messaging.Settings
{
    /// <summary>
    /// Represents the message processing job configuration settings.
    /// </summary>
    public sealed class MessageProcessingJobSettings
    {
        /// <summary>
        /// The settings key.
        /// </summary>
        public const string SettingsKey = "Messaging:MessageProcessingJob";

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
