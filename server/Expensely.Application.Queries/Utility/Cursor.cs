using System;
using System.Text;

namespace Expensely.Application.Queries.Utility
{
    /// <summary>
    /// Contains utility methods for working with cursors.
    /// </summary>
    public static class Cursor
    {
        /// <summary>
        /// Creates a base-64 encoded cursor based on the specified values.
        /// </summary>
        /// <param name="values">The values for the cursor.</param>
        /// <returns>The base-64 encoded cursor.</returns>
        public static string Create(params string[] values)
        {
            string cursorValue = string.Join(",", values);

            byte[] cursorBytes = Encoding.UTF8.GetBytes(cursorValue);

            string cursor = Convert.ToBase64String(cursorBytes);

            return cursor;
        }

        /// <summary>
        /// Parses the values from the specified cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="expectedCount">The expected number of cursor values.</param>
        /// <returns>The decoded cursor values.</returns>
        /// <exception cref="ArgumentException"> if the expected count does not match that number of values.</exception>
        public static string[] Parse(string cursor, int expectedCount)
        {
            byte[] decodedCursor = Convert.FromBase64String(cursor);

            string[] cursorValues = Encoding.UTF8.GetString(decodedCursor).Split(',');

            if (cursorValues.Length != expectedCount)
            {
                throw new InvalidOperationException("The specified cursor is invalid.");
            }

            return cursorValues;
        }
    }
}
