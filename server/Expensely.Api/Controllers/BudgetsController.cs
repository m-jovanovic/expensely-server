using System;
using System.Threading.Tasks;
using Expensely.Api.Constants;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Budgets.Commands.CreateBudget;
using Expensely.Application.Budgets.Commands.DeleteBudget;
using Expensely.Application.Budgets.Commands.UpdateBudget;
using Expensely.Application.Contracts.Budgets;
using Expensely.Domain.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    /// <summary>
    /// Represents the budgets resource controller.
    /// </summary>
    public sealed class BudgetsController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetsController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public BudgetsController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Creates the budget based on the specified request.
        /// </summary>
        /// <param name="request">The create budget request.</param>
        /// <returns>200 - OK if the budget was created successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Budgets.CreateBudget)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetRequest request) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new CreateBudgetCommand(
                    value.UserId,
                    value.Name,
                    value.Amount,
                    value.Currency,
                    value.StartDate,
                    value.EndDate))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Creates the budget based on the specified request.
        /// </summary>
        /// <param name="id">The budget identifier.</param>
        /// <param name="request">The create budget request.</param>
        /// <returns>200 - OK if the budget was updated successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Budgets.UpdateBudget)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBudget(Guid id, [FromBody] UpdateBudgetRequest request) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Ensure(value => value.BudgetId == id, ApiErrors.UnProcessableRequest)
                .Map(value => new UpdateBudgetCommand(
                    value.BudgetId,
                    value.Name,
                    value.Amount,
                    value.Currency,
                    value.StartDate,
                    value.EndDate))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Deletes the budget with the specified identifier.
        /// </summary>
        /// <param name="id">The budget identifier.</param>
        /// <returns>204 - No Content if the budget was deleted successfully, otherwise 404 - Not Found.</returns>
        [HttpDelete(ApiRoutes.Budgets.DeleteBudget)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudget(Guid id) =>
            await Result.Success(new DeleteBudgetCommand(id))
                .Bind(command => Sender.Send(command))
                .Match(NoContent, _ => NotFound());
    }
}
