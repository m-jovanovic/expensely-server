using System;
using System.Globalization;
using Expensely.Common.Abstractions.Constants;

namespace Expensely.Common.Abstractions.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to a display date string representation.
        /// </summary>
        /// <param name="dateTime">The date time object.</param>
        /// <returns>The display date string representation of the <see cref="DateTime"/> object.</returns>
        public static string ToDisplayDate(this DateTime dateTime) =>
            dateTime.ToString(DateTimeFormats.DisplayDate, CultureInfo.InvariantCulture);
    }
}
