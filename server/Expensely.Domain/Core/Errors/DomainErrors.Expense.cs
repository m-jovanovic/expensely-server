using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the expense errors.
        /// </summary>
        public static class Expense
        {
            /// <summary>
            /// Gets the expense not found error.
            /// </summary>
            public static Error NotFound => new Error("Expense.NotFound", "The expense with the specified identifier was not found.");
        }
    }
}
