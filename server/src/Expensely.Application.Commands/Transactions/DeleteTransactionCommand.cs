using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Shared.Primitives.Result;

namespace Expensely.Application.Commands.Transactions
{
    /// <summary>
    /// Represents the command for deleting a transaction.
    /// </summary>
    public sealed class DeleteTransactionCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTransactionCommand"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        public DeleteTransactionCommand(Guid transactionId) => TransactionId = transactionId.ToString();

        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        public string TransactionId { get; }
    }
}
