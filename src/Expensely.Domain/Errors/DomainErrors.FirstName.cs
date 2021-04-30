using Expensely.Common.Primitives.Errors;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the first name errors.
        /// </summary>
        public static class FirstName
        {
            /// <summary>
            /// Gets the first name is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new("FirstName.NullOrEmpty", "The first name is required.");

            /// <summary>
            /// Gets the first name is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new("FirstName.LongerThanAllowed", "The first name is longer than allowed.");
        }
    }
}
