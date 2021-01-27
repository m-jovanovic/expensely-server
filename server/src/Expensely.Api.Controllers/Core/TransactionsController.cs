﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Contracts;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Commands.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Abstractions.Clock;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the transactions resource controller.
    /// </summary>
    public sealed class TransactionsController : ApiController
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dateTime">The date and time.</param>
        public TransactionsController(ISender sender, IDateTime dateTime)
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
            Guid userId,
            int primaryCurrency,
            CancellationToken cancellationToken) =>
            await Maybe<GetCurrentMonthTransactionSummaryQuery>
                .From(new GetCurrentMonthTransactionSummaryQuery(userId, primaryCurrency, _dateTime.UtcNow))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Creates the transaction based on the specified request.
        /// </summary>
        /// <param name="createTransactionRequest">The create transaction request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction was created successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Transactions.CreateTransaction)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] CreateTransactionRequest createTransactionRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(createTransactionRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new CreateTransactionCommand(
                    request.UserId,
                    request.Name,
                    request.Description,
                    request.Category,
                    request.Amount,
                    request.Currency,
                    request.OccurredOn,
                    request.TransactionType))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Updates the transaction based on the specified request.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="updateTransactionRequest">The update transaction request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction was updated successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Transactions.UpdateTransaction)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTransaction(
            Guid transactionId,
            [FromBody] UpdateTransactionRequest updateTransactionRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(updateTransactionRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new UpdateTransactionCommand(
                    transactionId,
                    request.Name,
                    request.Description,
                    request.Category,
                    request.Amount,
                    request.Currency,
                    request.OccurredOn))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Deletes the transaction with the specified identifier.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>204 - No Content if the transaction was deleted successfully, otherwise 404 - Not Found.</returns>
        [HttpDelete(ApiRoutes.Transactions.DeleteTransaction)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(Guid transactionId, CancellationToken cancellationToken) =>
            await Result.Success(new DeleteTransactionCommand(transactionId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, _ => NotFound());
    }
}
