using Expensely.Common.Primitives.Errors;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the budget errors.
        /// </summary>
        public static class Budget
        {
            /// <summary>
            /// Gets the budget category already exists error.
            /// </summary>
            public static Error CategoryAlreadyExists => new(
                "Budget.CategoryAlreadyExists",
                "The specified category already exists in the budget's categories.");

            /// <summary>
            /// Gets the budget category does not exist error.
            /// </summary>
            public static Error CategoryDoesNotExist => new(
                "Budget.CategoryDoesNotExist",
                "The specified category does not exist in the budget's categories.");
        }
    }
}
