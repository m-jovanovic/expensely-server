using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Contracts;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Commands.Expenses.CreateExpense;
using Expensely.Application.Commands.Expenses.DeleteExpense;
using Expensely.Application.Commands.Expenses.UpdateExpense;
using Expensely.Application.Queries.Expenses.GetExpenses;
using Expensely.Common.Clock;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the expenses resource controller.
    /// </summary>
    public sealed class ExpensesController : ApiController
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpensesController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dateTime">The current date and time.</param>
        public ExpensesController(ISender sender, IDateTime dateTime)
            : base(sender) =>
            _dateTime = dateTime;

        /// <summary>
        /// Gets the expenses for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if any expenses are found, otherwise 404 - Not Found.</returns>
        [HttpGet(ApiRoutes.Expenses.GetExpenses)]
        [ProducesResponseType(typeof(ExpenseListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenses(Guid userId, int limit, string cursor, CancellationToken cancellationToken) =>
            await Maybe<GetExpensesQuery>
                .From(new GetExpensesQuery(userId, limit, cursor, _dateTime.UtcNow))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Creates the expense based on the specified request.
        /// </summary>
        /// <param name="request">The create expense request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the expense was created successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Expenses.CreateExpense)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request, CancellationToken cancellationToken) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new CreateExpenseCommand(
                    value.UserId,
                    value.Name,
                    value.Amount,
                    value.Currency,
                    value.OccurredOn,
                    value.Description))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Updates the expense based on the specified request.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <param name="request">The update expense request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the expense was updated successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Expenses.UpdateExpense)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExpense(
            Guid expenseId, [FromBody] UpdateExpenseRequest request, CancellationToken cancellationToken) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new UpdateExpenseCommand(
                    expenseId,
                    value.Name,
                    value.Amount,
                    value.Currency,
                    value.OccurredOn,
                    value.Description))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Deletes the expense with the specified identifier.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>204 - No Content if the expense was deleted successfully, otherwise 404 - Not Found.</returns>
        [HttpDelete(ApiRoutes.Expenses.DeleteExpense)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExpense(Guid expenseId, CancellationToken cancellationToken) =>
            await Result.Success(new DeleteExpenseCommand(expenseId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, _ => NotFound());
    }
}
