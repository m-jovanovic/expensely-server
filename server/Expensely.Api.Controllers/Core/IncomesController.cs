using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Contracts;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Commands.Incomes.CreateIncome;
using Expensely.Application.Commands.Incomes.DeleteIncome;
using Expensely.Application.Commands.Incomes.UpdateIncome;
using Expensely.Contracts.Incomes;
using Expensely.Domain.Primitives.Result;
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
        /// <param name="request">The create income request.</param>
        /// <returns>200 - OK if the income was created successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Incomes.CreateIncome)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest request) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(x => new CreateIncomeCommand(
                    x.UserId,
                    x.Name,
                    x.Amount,
                    x.Currency,
                    x.OccurredOn,
                    x.Description))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Updates the income based on the specified request.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <param name="request">The update income request.</param>
        /// <returns>200 - OK if the income was updated successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Incomes.UpdateIncome)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateIncome(Guid incomeId, [FromBody] UpdateIncomeRequest request) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(x => new UpdateIncomeCommand(
                    incomeId,
                    x.Name,
                    x.Amount,
                    x.Currency,
                    x.OccurredOn,
                    x.Description))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);
    }
}
