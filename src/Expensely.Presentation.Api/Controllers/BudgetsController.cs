using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Budgets;
using Expensely.Application.Contracts.Budgets;
using Expensely.Application.Queries.Budgets;
using Expensely.Authorization.Abstractions;
using Expensely.Authorization.Attributes;
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
        /// Gets the active budgets for the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection of active budgets for the specified user identifier.</returns>
        [HasPermission(Permission.BudgetRead)]
        [HttpGet(ApiRoutes.Budgets.GetActiveBudgets)]
        [ProducesResponseType(typeof(IEnumerable<BudgetListItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveBudgets(Ulid userId, CancellationToken cancellationToken) =>
            await Sender.Send(new GetActiveBudgetsQuery(userId), cancellationToken).Map(Ok);

        /// <summary>
        /// Gets the budget for the specified identifier.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The budget with the specified identifier, if it exists.</returns>
        [HasPermission(Permission.BudgetRead)]
        [HttpGet(ApiRoutes.Budgets.GetBudgetById)]
        [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBudgetById(Ulid budgetId, CancellationToken cancellationToken) =>
            await Maybe<GetBudgetByIdQuery>
                .From(new GetBudgetByIdQuery(budgetId))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Gets the budget details for the specified identifier.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The details of the budget with the specified identifier, if it exists.</returns>
        [HasPermission(Permission.BudgetRead)]
        [HttpGet(ApiRoutes.Budgets.GetBudgetDetailsById)]
        [ProducesResponseType(typeof(BudgetDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBudgetDetailsById(Ulid budgetId, CancellationToken cancellationToken) =>
            await Maybe<GetBudgetDetailsByIdQuery>
                .From(new GetBudgetDetailsByIdQuery(budgetId))
                .Bind(query => Sender.Send(query, cancellationToken))
                .Match(Ok, NotFound);

        /// <summary>
        /// Creates the budget based on the specified request.
        /// </summary>
        /// <param name="createBudgetRequest">The create budget request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.BudgetModify)]
        [HttpPost(ApiRoutes.Budgets.CreateBudget)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateBudget(
            [FromBody] CreateBudgetRequest createBudgetRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(createBudgetRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new CreateBudgetCommand(
                    request.UserId,
                    request.Name,
                    request.Amount,
                    request.Currency,
                    request.Categories,
                    request.StartDate,
                    request.EndDate))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Creates the budget based on the specified request.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="updateBudgetRequest">The update budget request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.BudgetModify)]
        [HttpPut(ApiRoutes.Budgets.UpdateBudget)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateBudget(
            Ulid budgetId,
            [FromBody] UpdateBudgetRequest updateBudgetRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(updateBudgetRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new UpdateBudgetCommand(
                    budgetId,
                    request.Name,
                    request.Amount,
                    request.Currency,
                    request.Categories,
                    request.StartDate,
                    request.EndDate))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Deletes the budget with the specified identifier.
        /// </summary>
        /// <param name="budgetId">The budget identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.BudgetModify)]
        [HttpDelete(ApiRoutes.Budgets.DeleteBudget)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudget(Ulid budgetId, CancellationToken cancellationToken) =>
            await Result.Success(new DeleteBudgetCommand(budgetId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, _ => NotFound());
    }
}
