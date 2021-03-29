using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the budget errors.
        /// </summary>
        public static class Budget
        {
            /// <summary>
            /// Gets the budget not found error.
            /// </summary>
            public static Error NotFound => new("Budget.NotFound", "The budget with the specified identifier was not found.");

            /// <summary>
            /// Gets the budget identifier is required error.
            /// </summary>
            public static Error IdentifierIsRequired => new("Budget.IdentifierIsRequired", "The budget identifier is required.");

            /// <summary>
            /// Gets the budget name is required error.
            /// </summary>
            public static Error NameIsRequired => new("Budget.NameIsRequired", "The budget name is required.");

            /// <summary>
            /// Gets the budget amount less than or equal to zero error.
            /// </summary>
            public static Error AmountLessThanOrEqualToZero => new(
                "Budget.AmountLessThanOrEqualToZero",
                "The budget amount can not be less than or equal to zero.");

            /// <summary>
            /// Gets the budget start date is required error.
            /// </summary>
            public static Error StartDateIsRequired => new("Budget.StartDateIsRequired", "The date the budget occurred on is required.");

            /// <summary>
            /// Gets the budget end date is required error.
            /// </summary>
            public static Error EndDateIsRequired => new("Budget.EndDateIsRequired", "The date the budget occurred on is required.");

            /// <summary>
            /// Gets the budget end date precedes start date error.
            /// </summary>
            public static Error EndDatePrecedesStartDate => new(
                "Budget.EndDatePrecedesStartDate",
                "The budget end date must be after the start date.");
        }
    }
}
