using System.Linq;
using Expensely.Domain.Core;
using Raven.Client.Documents.Indexes;

namespace Expensely.Persistence.Indexes.Transactions
{
    /// <summary>
    /// Represents the index on the transactions collection by the user identifier, occurred on, created on and transaction type.
    /// </summary>
    public sealed class Transactions_ByUserIdAndOccurredOnAndCreatedOnAndTransactionType : AbstractIndexCreationTask<Transaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions_ByUserIdAndOccurredOnAndCreatedOnAndTransactionType"/> class.
        /// </summary>
        public Transactions_ByUserIdAndOccurredOnAndCreatedOnAndTransactionType() =>
            Map = transactions =>
                from transaction in transactions
                select new
                {
                    transaction.UserId,
                    transaction.OccurredOn,
                    transaction.CreatedOnUtc,
                    transaction.TransactionType
                };
    }
}
