using System;
using System.Threading.Tasks;
using Expensely.Api.Constants;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Users.Commands.AddUserCurrency;
using Expensely.Application.Users.Commands.ChangeUserPrimaryCurrency;
using Expensely.Application.Users.Commands.RemoveUserCurrency;
using Expensely.Domain.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
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
        /// <param name="id">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <returns>200 - OK if the currency was added to the users currencies successfully, otherwise 400 - Bad Request.</returns>
        [HttpPost(ApiRoutes.Users.AddUserCurrency)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserCurrency(Guid id, int currency) =>
            await Result.Success(new AddUserCurrencyCommand(id, currency))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Removes the specified currency from the user's currencies.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <returns>204 - No Content if the currency was removed from the users currencies successfully, otherwise 400 - Bad Request.</returns>
        [HttpDelete(ApiRoutes.Users.RemoveUserCurrency)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUserCurrency(Guid id, int currency) =>
            await Result.Success(new RemoveUserCurrencyCommand(id, currency))
                .Bind(command => Sender.Send(command))
                .Match(NoContent, BadRequest);

        /// <summary>
        /// Changes the user's primary currency to the specified currency.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        /// <returns>200 - OK if the user's primary currency was changed successfully, otherwise 400 - Bad Request.</returns>
        [HttpPut(ApiRoutes.Users.ChangeUserPrimaryCurrency)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUserPrimaryCurrency(Guid id, int currency) =>
            await Result.Success(new ChangeUserPrimaryCurrencyCommand(id, currency))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);
    }
}
