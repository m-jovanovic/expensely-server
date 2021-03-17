using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Primitives.ServiceLifetimes;

namespace Expensely.Infrastructure.Clock
{
    /// <summary>
    /// Represents the current machine date and time.
    /// </summary>
    public sealed class SystemTime : ISystemTime, ITransient
    {
        /// <inheritdoc />
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}
