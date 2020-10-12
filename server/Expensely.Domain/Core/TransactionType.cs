using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the enumeration of the supported transaction types.
    /// </summary>
    public sealed class TransactionType : Enumeration<TransactionType>
    {
        /// <summary>
        /// The income transaction type.
        /// </summary>
        public static readonly TransactionType Income = new TransactionType(1, "Income");

        /// <summary>
        /// The expense transaction type.
        /// </summary>
        public static readonly TransactionType Expense = new TransactionType(2, "Expense");

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionType"/> class.
        /// </summary>
        /// <param name="value">The transaction type value.</param>
        /// <param name="name">The transaction type name.</param>
        private TransactionType(int value, string name)
            : base(value, name)
        {
        }
    }
}
