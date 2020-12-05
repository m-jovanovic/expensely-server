using System;

namespace Expensely.Domain.Abstractions.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="DateTime"/> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the start of the week based on the date and time instance. The first day of the week is assumed to be Monday.
        /// </summary>
        /// <param name="dateTime">The date and time instance.</param>
        /// <returns>The start of the week.</returns>
        public static DateTime StartOfWeek(this DateTime dateTime) =>
            dateTime.AddDays(-(7 + (dateTime.DayOfWeek - DayOfWeek.Monday)) % 7);
    }
}
