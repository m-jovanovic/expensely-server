using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the currency errors.
        /// </summary>
        public static class Currency
        {
            /// <summary>
            /// Gets the currency not found error.
            /// </summary>
            public static Error NotFound => new("Currency.NotFound", "The currency with the specified value was not found.");
        }
    }
}
