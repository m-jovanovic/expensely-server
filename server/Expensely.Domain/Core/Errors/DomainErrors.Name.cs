using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core.Errors
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
            public static Error NullOrEmpty => new Error("Name.NullOrEmpty", "The name is required.");

            /// <summary>
            /// Gets the name is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new Error("Name.LongerThanAllowed", "The name is longer than allowed.");
        }
    }
}
