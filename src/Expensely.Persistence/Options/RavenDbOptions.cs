namespace Expensely.Persistence.Options
{
    /// <summary>
    /// Represents the RavenDB settings.
    /// </summary>
    public sealed class RavenDbOptions
    {
        /// <summary>
        /// Gets the certificate.
        /// </summary>
        public string Certificate { get; init; }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string Database { get; init; }

        /// <summary>
        /// Gets the cluster nodes URLs.
        /// </summary>
        public string[] Urls { get; init; }
    }
}
