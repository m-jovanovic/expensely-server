﻿using Expensely.Domain.Primitives;

namespace Expensely.Application.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class Errors
    {
        /// <summary>
        /// Contains the currency errors.
        /// </summary>
        internal static class Currency
        {
            /// <summary>
            /// Gets the currency not found error.
            /// </summary>
            internal static Error NotFound => new Error("Currency.NotFound", "The currency with the specified value was not found.");
        }
    }
}
