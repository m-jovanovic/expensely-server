using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the income errors.
        /// </summary>
        internal static class Income
        {
            /// <summary>
            /// Gets the income identifier is required error.
            /// </summary>
            internal static Error IdentifierIsRequired => new Error("Income.IdentifierIsRequired", "The income identifier is required.");

            /// <summary>
            /// Gets the income name is required error.
            /// </summary>
            internal static Error NameIsRequired => new Error("Income.NameIsRequired", "The income name is required.");

            /// <summary>
            /// Gets the income amount less than or equal to zero error.
            /// </summary>
            internal static Error AmountLessThanOrEqualToZero => new Error(
                "Income.AmountLessThanOrEqualToZero",
                "The income amount can not be less than or equal to zero.");

            /// <summary>
            /// Gets the income occurred on date is required error.
            /// </summary>
            internal static Error OccurredOnDateIsRequired => new Error(
                "Income.OccurredOnDateIsRequired",
                "The date the income occurred on is required.");
        }
    }
}
