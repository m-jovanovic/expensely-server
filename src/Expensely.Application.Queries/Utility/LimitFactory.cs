using System;

namespace Expensely.Application.Queries.Utility
{
    /// <summary>
    /// Contains utility methods for creating valid limits.
    /// </summary>
    public static class LimitFactory
    {
        /// <summary>
        /// The maximum limit value.
        /// </summary>
        public const int MaxLimit = 20;

        /// <summary>
        /// Gets the limit based on the specified value and the default maximum limit of <see cref="MaxLimit"/>.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>The smaller value out of limit and <see cref="MaxLimit"/>, incremented by one.</returns>
        public static int GetLimit(int limit) => GetLimit(limit, MaxLimit);

        /// <summary>
        /// Gets the limit based on the specified value and the maximum limit.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <param name="maxLimit">The maximum limit.</param>
        /// <returns>The smaller value out of limit and max limit, incremented by one.</returns>
        public static int GetLimit(int limit, int maxLimit) => Math.Min(Math.Abs(limit), maxLimit) + 1;
    }
}
