using System;
using System.Threading.Tasks;
using Expensely.Api.Constants;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Users.Commands.AddUserCurrency;
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
        /// Adds the specified currency to the users currencies.
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
    }
}
