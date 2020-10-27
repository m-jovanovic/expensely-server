﻿using System;
using System.Threading.Tasks;
using Expensely.Api.Constants;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Abstractions.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Commands.UpdateExpense;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
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
        /// <returns>200 - OK if any expenses are found, otherwise 404 - Not Found.</returns>
        [HttpGet(ApiRoutes.Expenses.Get)]
        [ProducesResponseType(typeof(ExpenseListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid userId, int limit, string cursor) =>
            await Maybe<GetExpensesQuery>
                .From(new GetExpensesQuery(userId, limit, cursor, _dateTime.UtcNow))
                .Bind(query => Sender.Send(query))
                .Match(Ok, NotFound);

        /// <summary>
        /// Creates the expense based on the specified request.
        /// </summary>
        /// <param name="request">The create expense request.</param>
        /// <returns>201 - Created if the expense was created successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Expenses.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request) =>
            await Result.Create(request, Errors.UnProcessableRequest)
                .Map(value => new CreateExpenseCommand(
                    value.UserId,
                    value.Name,
                    value.Amount,
                    value.Currency,
                    value.OccurredOn,
                    value.Description))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Creates the expense based on the specified request.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <param name="request">The create expense request.</param>
        /// <returns>200 - OK if the expense was updated successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Expenses.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseRequest request) =>
            await Result.Create(request, Errors.UnProcessableRequest)
                .Ensure(value => value.ExpenseId == id, Errors.UnProcessableRequest)
                .Map(value => new UpdateExpenseCommand(
                    value.ExpenseId,
                    value.Name,
                    value.Amount,
                    value.Currency,
                    value.OccurredOn,
                    value.Description))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Deletes the expense with the specified identifier.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <returns>204 - No Content if the expense was deleted successfully, otherwise 404 - Not Found.</returns>
        [HttpDelete(ApiRoutes.Expenses.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExpense(Guid id) =>
            await Result.Success(new DeleteExpenseCommand(id))
                .Bind(command => Sender.Send(command))
                .Match(NoContent, _ => NotFound());
    }
}