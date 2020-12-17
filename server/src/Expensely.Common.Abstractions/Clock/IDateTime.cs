using System;

namespace Expensely.Common.Abstractions.Clock
{
    /// <summary>
    /// Represents the interface for getting the current date and time.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current date and time in UTC format.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
