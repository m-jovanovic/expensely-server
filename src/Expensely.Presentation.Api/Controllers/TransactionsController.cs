using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Transactions;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Authorization.Abstractions;
using Expensely.Authorization.Attributes;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Primitives.Extensions;
using Expensely.Common.Primitives.Maybe;
using Expensely.Common.Primitives.Result;
using Expensely.Presentation.Api.Constants;
using Expensely.Presentation.Api.Errors;
using Expensely.Presentation.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Presentation.Api.Controllers
{
    /// <summary>
    /// Represents the transactions resource controller.
    /// </summary>
    public sealed class TransactionsController : ApiController
    {
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="systemTime">The system time.</param>
        public TransactionsController(ISender sender, ISystemTime systemTime)
            : base(sender) =>
            _systemTime = systemTime;

        /// <summary>
        /// Gets the transactions for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The transaction list response with 200 - OK status code.</returns>
        [HasPermission(Permission.TransactionRead)]
        [HttpGet(ApiRoutes.Transactions.GetTransactions)]
        [ProducesResponseType(typeof(TransactionListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactions(Ulid userId, int limit, string cursor, CancellationToken cancellationToken) =>
            await Sender.Send(new GetTransactionsQuery(userId, limit, cursor, _systemTime.UtcNow), cancellationToken).Map(Ok);

        /// <summary>
        /// Gets the transaction for the specified identifier.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction with the specified identifier is found, otherwise 404 - Not Found.</returns>
        [HasPermission(Permission.TransactionRead)]
        [HttpGet(ApiRoutes.Transactions.GetTransactionById)]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionById(Ulid transactionId, CancellationToken cancellationToken) =>
            await Maybe<GetTransactionByIdQuery>
                .From(new GetTransactionByIdQuery(transactionId))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Gets the transaction details for the specified identifier.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction with the specified identifier is found, otherwise 404 - Not Found.</returns>
        [HasPermission(Permission.TransactionRead)]
        [HttpGet(ApiRoutes.Transactions.GetTransactionDetailsById)]
        [ProducesResponseType(typeof(TransactionDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionDetailsById(Ulid transactionId, CancellationToken cancellationToken) =>
            await Maybe<GetTransactionDetailsByIdQuery>
                .From(new GetTransactionDetailsByIdQuery(transactionId))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Gets the current month transaction summary for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction summary is found, otherwise 404 - Not Found.</returns>
        [HasPermission(Permission.TransactionRead)]
        [HttpGet(ApiRoutes.Transactions.GetCurrentMonthTransactionSummary)]
        [ProducesResponseType(typeof(TransactionSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentMonthTransactionSummary(
            Ulid userId,
            int currency,
            CancellationToken cancellationToken) =>
            await Maybe<GetCurrentMonthTransactionSummaryQuery>
                .From(new GetCurrentMonthTransactionSummaryQuery(userId, currency, _systemTime.UtcNow))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Gets the current month expenses per category for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The expenses per category with 200 - OK status code.</returns>
        [HasPermission(Permission.TransactionRead)]
        [HttpGet(ApiRoutes.Transactions.GetCurrentMonthExpensesPerCategory)]
        [ProducesResponseType(typeof(IEnumerable<ExpensePerCategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentMonthExpensesPerCategory(
            Ulid userId,
            int currency,
            CancellationToken cancellationToken) =>
            await Sender
                .Send(new GetCurrentMonthExpensesPerCategoryQuery(userId, currency, _systemTime.UtcNow), cancellationToken)
                .Map(Ok);

        /// <summary>
        /// Creates the transaction based on the specified request.
        /// </summary>
        /// <param name="createTransactionRequest">The create transaction request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the transaction was created successfully, otherwise 400 - Bad Request.</returns>
        [HasPermission(Permission.TransactionModify)]
        [HttpPost(ApiRoutes.Transactions.CreateTransaction)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] CreateTransactionRequest createTransactionRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(createTransactionRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new CreateTransactionCommand(
                    request.UserId,
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
        [HasPermission(Permission.TransactionModify)]
        [HttpPut(ApiRoutes.Transactions.UpdateTransaction)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateTransaction(
            Ulid transactionId,
            [FromBody] UpdateTransactionRequest updateTransactionRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(updateTransactionRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new UpdateTransactionCommand(
                    transactionId,
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
        [HasPermission(Permission.TransactionModify)]
        [HttpDelete(ApiRoutes.Transactions.DeleteTransaction)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(Ulid transactionId, CancellationToken cancellationToken) =>
            await Result.Success(new DeleteTransactionCommand(transactionId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, _ => NotFound());
    }
}
