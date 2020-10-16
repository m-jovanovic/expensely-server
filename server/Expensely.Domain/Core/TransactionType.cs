using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the enumeration of the supported transaction types.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// The expense transaction type.
        /// </summary>
        Expense = 1,

        /// <summary>
        /// The income transaction type.
        /// </summary>
        Income = 2
    }
}
