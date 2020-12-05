using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
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
