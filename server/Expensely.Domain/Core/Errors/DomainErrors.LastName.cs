using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Core.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the last name errors.
        /// </summary>
        public static class LastName
        {
            /// <summary>
            /// Gets the last name is null or empty error.
            /// </summary>
            public static Error NullOrEmpty => new Error("LastName.NullOrEmpty", "The last name is required.");

            /// <summary>
            /// Gets the last name is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new Error("LastName.LongerThanAllowed", "The last name is longer than allowed.");
        }
    }
}
