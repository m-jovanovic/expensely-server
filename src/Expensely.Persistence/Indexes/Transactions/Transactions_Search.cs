using System.Linq;
using Expensely.Domain.Modules.Transactions;
using Raven.Client.Documents.Indexes;

namespace Expensely.Persistence.Indexes.Transactions
{
    /// <summary>
    /// Represents the index on the transactions collection by:
    /// - user identifier,
    /// - transaction type,
    /// - currency,
    /// - category,
    /// - occurred on,
    /// - created on.
    /// </summary>
    public sealed class Transactions_Search : AbstractIndexCreationTask<Transaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions_Search"/> class.
        /// </summary>
        public Transactions_Search() =>
            Map = transactions =>
                from transaction in transactions
                select new
                {
                    transaction.UserId,
                    TransactionType_Value = transaction.TransactionType.Value,
                    Money_Currency_Value = transaction.Money.Currency.Value,
                    Category_Value = transaction.Category.Value,
                    transaction.OccurredOn,
                    transaction.CreatedOnUtc
                };
    }
}
