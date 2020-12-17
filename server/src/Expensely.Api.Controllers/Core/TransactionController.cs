using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Queries.Transactions.GetCurrentMonthTransactionSummary;
using Expensely.Application.Queries.Transactions.GetTransactions;
using Expensely.Common.Abstractions.Clock;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the transactions resource controller.
    /// </summary>
    public sealed class TransactionController : ApiController
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dateTime">The date and time.</param>
        public TransactionController(ISender sender, IDateTime dateTime)
            : base(sender) =>
            _dateTime = dateTime;

        /// <summary>
        /// Gets the transactions for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if any transactions are found, otherwise 404 - Not Found.</returns>
        [HttpGet(ApiRoutes.Transactions.GetTransactions)]
        [ProducesResponseType(typeof(TransactionListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactions(Guid userId, int limit, string cursor, CancellationToken cancellationToken) =>
            await Maybe<GetTransactionsQuery>
                .From(new GetTransactionsQuery(userId, limit, cursor, _dateTime.UtcNow))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Gets the current month transaction summary for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="primaryCurrency">The primary currency.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction summary is found, otherwise 404 - Not Found.</returns>
        [HttpGet(ApiRoutes.Transactions.GetCurrentMonthTransactionSummary)]
        [ProducesResponseType(typeof(TransactionSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentMonthTransactionSummary(
            Guid userId, int primaryCurrency, CancellationToken cancellationToken) =>
            await Maybe<GetCurrentMonthTransactionSummaryQuery>
                .From(new GetCurrentMonthTransactionSummaryQuery(userId, primaryCurrency, _dateTime.UtcNow))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);
    }
}
