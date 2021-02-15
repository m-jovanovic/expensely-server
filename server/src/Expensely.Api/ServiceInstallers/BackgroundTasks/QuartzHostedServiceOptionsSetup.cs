using Microsoft.Extensions.Options;
using Quartz;

namespace Expensely.Api.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the <see cref="QuartzHostedServiceOptions"/> setup.
    /// </summary>
    public sealed class QuartzHostedServiceOptionsSetup : IConfigureOptions<QuartzHostedServiceOptions>
    {
        /// <inheritdoc />
        public void Configure(QuartzHostedServiceOptions options) => options.WaitForJobsToComplete = true;
    }
}
