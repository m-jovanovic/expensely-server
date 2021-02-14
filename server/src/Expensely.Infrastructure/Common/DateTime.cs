using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Abstractions.ServiceLifetimes;

namespace Expensely.Infrastructure.Common
{
    /// <summary>
    /// Represents the current machine date and time.
    /// </summary>
    public sealed class DateTime : IDateTime, ITransient
    {
        /// <inheritdoc />
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}
