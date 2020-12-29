using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the budget errors.
        /// </summary>
        internal static class Budget
        {
            /// <summary>
            /// Gets the budget identifier is required error.
            /// </summary>
            internal static Error IdentifierIsRequired => new("Budget.IdentifierIsRequired", "The budget identifier is required.");

            /// <summary>
            /// Gets the budget name is required error.
            /// </summary>
            internal static Error NameIsRequired => new("Budget.NameIsRequired", "The budget name is required.");

            /// <summary>
            /// Gets the budget amount less than or equal to zero error.
            /// </summary>
            internal static Error AmountLessThanOrEqualToZero => new(
                "Budget.AmountLessThanOrEqualToZero",
                "The budget amount can not be less than or equal to zero.");

            /// <summary>
            /// Gets the budget start date is required error.
            /// </summary>
            internal static Error StartDateIsRequired => new("Budget.StartDateIsRequired", "The date the budget occurred on is required.");

            /// <summary>
            /// Gets the budget end date is required error.
            /// </summary>
            internal static Error EndDateIsRequired => new("Budget.EndDateIsRequired", "The date the budget occurred on is required.");

            /// <summary>
            /// Gets the budget end date precedes start date error.
            /// </summary>
            internal static Error EndDatePrecedesStartDate => new(
                "Budget.EndDatePrecedesStartDate",
                "The budget end date must be after the start date.");
        }
    }
}
