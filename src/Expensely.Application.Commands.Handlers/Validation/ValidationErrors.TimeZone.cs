using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the time zone errors.
        /// </summary>
        public static class TimeZone
        {
            /// <summary>
            /// Gets the time zone id is required error.
            /// </summary>
            public static Error IdentifierIsRequired => new("TimeZone.IdentifierIsRequired", "The time zone identifier is required.");

            /// <summary>
            /// Gets the time zone not found error.
            /// </summary>
            public static Error NotFound => new("TimeZone.NotFound", "The time zone with the specified identifier was not found.");
        }
    }
}
