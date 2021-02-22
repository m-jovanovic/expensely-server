using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Users;
using Expensely.Application.Contracts.Users;
using Expensely.Presentation.Api.Constants;
using Expensely.Presentation.Api.Errors;
using Expensely.Presentation.Api.Infrastructure;
using Expensely.Shared.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Presentation.Api.Controllers
{
    /// <summary>
    /// Represents the users resource controller.
    /// </summary>
    public sealed class UsersController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public UsersController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Adds the specified currency to the user's currencies.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the currency was added to the users currencies successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Users.AddUserCurrency)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddUserCurrency(Guid userId, int currency, CancellationToken cancellationToken) =>
            await Result.Success(new AddUserCurrencyCommand(userId, currency))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Removes the specified currency from the user's currencies.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>204 - No Content if the currency was removed from the users currencies successfully, otherwise 400 - Bad Request.</returns>
        [HttpDelete(ApiRoutes.Users.RemoveUserCurrency)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemoveUserCurrency(Guid userId, int currency, CancellationToken cancellationToken) =>
            await Result.Success(new RemoveUserCurrencyCommand(userId, currency))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, BadRequest);

        /// <summary>
        /// Changes the user's primary currency to the specified currency.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the user's primary currency was changed successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Users.ChangeUserPrimaryCurrency)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeUserPrimaryCurrency(Guid userId, int currency, CancellationToken cancellationToken) =>
            await Result.Success(new ChangeUserPrimaryCurrencyCommand(userId, currency))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Changes the user's password based on the specified request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The change password request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>200 - OK if the user's password was changed successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Users.ChangeUserPassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeUserPassword(
            Guid userId, [FromBody] ChangePasswordRequest request, CancellationToken cancellationToken) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new ChangeUserPasswordCommand(userId, value.CurrentPassword, value.NewPassword, value.ConfirmationPassword))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);
    }
}
