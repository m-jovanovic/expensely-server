using Expensely.Common.Primitives.Errors;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the name errors.
        /// </summary>
        public static class Name
        {
            /// <summary>
            /// Gets the name is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new("Name.NullOrEmpty", "The name is required.");

            /// <summary>
            /// Gets the name is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new("Name.LongerThanAllowed", "The name is longer than allowed.");
        }
    }
}
