using System.Linq;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents.Indexes;

namespace Expensely.Persistence.Indexes.Transactions
{
    /// <summary>
    /// Represents the index on the transactions collection by the user identifier, occurred on, created on.
    /// </summary>
    public sealed class Transactions_ByUserIdAndOccurredOnAndCreatedOn : AbstractIndexCreationTask<Transaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions_ByUserIdAndOccurredOnAndCreatedOn"/> class.
        /// </summary>
        public Transactions_ByUserIdAndOccurredOnAndCreatedOn() =>
            Map = transactions =>
                from transaction in transactions
                select new
                {
                    transaction.UserId,
                    transaction.OccurredOn,
                    transaction.CreatedOnUtc
                };
    }
}
