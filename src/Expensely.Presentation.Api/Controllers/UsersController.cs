using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Users;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Queries.Users;
using Expensely.Authorization.Abstractions;
using Expensely.Authorization.Attributes;
using Expensely.Common.Primitives.Extensions;
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
        /// Gets the transaction for the specified identifier.
        /// </summary>
        /// <param name="userId">The transaction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection of user's currencies.</returns>
        [HasPermission(Permission.UserRead)]
        [HttpGet(ApiRoutes.Users.GetUserCurrencies)]
        [ProducesResponseType(typeof(IEnumerable<UserCurrencyResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserCurrencies(Ulid userId, CancellationToken cancellationToken) =>
            await Sender.Send(new GetUserCurrenciesQuery(userId), cancellationToken).Map(Ok);

        /// <summary>
        /// Changes the user's name based on the specified request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The change name request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpPut(ApiRoutes.Users.ChangeUserName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeUserName(
            Ulid userId, [FromBody] ChangeUserNameRequest request, CancellationToken cancellationToken) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new ChangeUserNameCommand(userId, value.FirstName, value.LastName))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Adds the specified currency to the user's currencies.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpPut(ApiRoutes.Users.AddUserCurrency)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddUserCurrency(Ulid userId, int currency, CancellationToken cancellationToken) =>
            await Result.Success(new AddUserCurrencyCommand(userId, currency))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Removes the specified currency from the user's currencies.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpDelete(ApiRoutes.Users.RemoveUserCurrency)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemoveUserCurrency(Ulid userId, int currency, CancellationToken cancellationToken) =>
            await Result.Success(new RemoveUserCurrencyCommand(userId, currency))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(NoContent, BadRequest);

        /// <summary>
        /// Changes the user's primary currency to the specified currency.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpPut(ApiRoutes.Users.ChangeUserPrimaryCurrency)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeUserPrimaryCurrency(Ulid userId, int currency, CancellationToken cancellationToken) =>
            await Result.Success(new ChangeUserPrimaryCurrencyCommand(userId, currency))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Changes the user's time zone to the specified time zone.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpPut(ApiRoutes.Users.ChangeUserTimeZone)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeUserTimeZone(Ulid userId, string timeZoneId, CancellationToken cancellationToken) =>
            await Result.Success(new ChangeUserTimeZoneCommand(userId, timeZoneId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Changes the user's password based on the specified request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The change user password request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpPut(ApiRoutes.Users.ChangeUserPassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangeUserPassword(
            Ulid userId, [FromBody] ChangeUserPasswordRequest request, CancellationToken cancellationToken) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new ChangeUserPasswordCommand(userId, value.CurrentPassword, value.NewPassword, value.ConfirmationPassword))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Sets up the user's primary currency and time zone identifier based on the specified request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">The setup user request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HasPermission(Permission.UserModify)]
        [HttpPut(ApiRoutes.Users.SetupUser)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SetupUser(
            Ulid userId, [FromBody] SetupUserRequest request, CancellationToken cancellationToken) =>
            await Result.Create(request, ApiErrors.UnProcessableRequest)
                .Map(value => new SetupUserCommand(userId, value.Currency, value.TimeZoneId))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);
    }
}
