using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Errors
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
            public static Error NotFound => new("Expense.NotFound", "The expense with the specified identifier was not found.");
        }
    }
}
