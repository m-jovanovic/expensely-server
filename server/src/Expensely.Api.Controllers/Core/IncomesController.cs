using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Contracts;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Commands.Incomes;
using Expensely.Contracts.Incomes;
using Expensely.Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the incomes resource controller.
    /// </summary>
    public sealed class IncomesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomesController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public IncomesController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Creates the income based on the specified request.
        /// </summary>
        /// <param name="createIncomeRequest">The create income request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the income was created successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Incomes.CreateIncome)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIncome(
            [FromBody] CreateIncomeRequest createIncomeRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(createIncomeRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new CreateIncomeCommand(
                    request.UserId,
                    request.Name,
                    request.Category,
                    request.Amount,
                    request.Currency,
                    request.OccurredOn,
                    request.Description))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Updates the income based on the specified request.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <param name="updateIncomeRequest">The update income request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the income was updated successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Incomes.UpdateIncome)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateIncome(
            Guid incomeId,
            [FromBody] UpdateIncomeRequest updateIncomeRequest,
            CancellationToken cancellationToken) =>
            await Result.Create(updateIncomeRequest, ApiErrors.UnProcessableRequest)
                .Map(incomeRequest => new UpdateIncomeCommand(
                    incomeId,
                    incomeRequest.Name,
                    incomeRequest.Category,
                    incomeRequest.Amount,
                    incomeRequest.Currency,
                    incomeRequest.OccurredOn,
                    incomeRequest.Description))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Deletes the income with the specified identifier.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>204 - No Content if the income was deleted successfully, otherwise 404 - Not Found.</returns>
        [HttpDelete(ApiRoutes.Incomes.DeleteIncome)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIncome(Guid incomeId, CancellationToken cancellationToken) =>
            await Result.Success(new DeleteIncomeCommand(incomeId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, _ => NotFound());
    }
}
