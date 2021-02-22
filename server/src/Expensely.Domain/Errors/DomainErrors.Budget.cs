using Expensely.Domain.Primitives;
using Expensely.Shared.Primitives.Errors;

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
            /// Gets the budget not found error.
            /// </summary>
            public static Error NotFound => new("Budget.NotFound", "The budget with the specified identifier was not found.");
        }
    }
}
