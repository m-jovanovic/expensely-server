using System;
using Expensely.Common.Abstractions.Clock;

namespace Expensely.Infrastructure.Common
{
    /// <summary>
    /// Represents the current machine date and time.
    /// </summary>
    public sealed class MachineDateTime : IDateTime
    {
        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
