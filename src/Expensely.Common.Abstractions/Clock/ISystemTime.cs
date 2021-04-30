using System;

namespace Expensely.Common.Abstractions.Clock
{
    /// <summary>
    /// Represents the interface for getting the current system time.
    /// </summary>
    public interface ISystemTime
    {
        /// <summary>
        /// Gets the current date and time in UTC format.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
