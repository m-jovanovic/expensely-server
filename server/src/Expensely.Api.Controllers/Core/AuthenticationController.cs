using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Contracts;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Commands.Users.CreateUser;
using Expensely.Application.Commands.Users.CreateUserTokenForCredentials;
using Expensely.Application.Commands.Users.RefreshUserToken;
using Expensely.Contracts.Users;
using Expensely.Domain.Abstractions.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the authentication controller.
    /// </summary>
    [AllowAnonymous]
    public sealed class AuthenticationController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public AuthenticationController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Logs in the user based on the specified request and returns a new JWT if successful.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The JWT if the login was successful, or an error response otherwise.</returns>
        [HttpPost(ApiRoutes.Authentication.Login)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken) =>
            await Result.Create(loginRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new CreateUserTokenForCredentialsCommand(request.Email, request.Password))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Registers a user based on the specified request.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The JWT if the registration was successful, or an error response otherwise.</returns>
        [HttpPost(ApiRoutes.Authentication.Register)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken) =>
            await Result.Create(registerRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new CreateUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.ConfirmationPassword))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Refreshes the user's token based on the specified request and returns a new JWT if successful.
        /// </summary>
        /// <param name="refreshTokenRequest">The refresh token request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The JWT if the token was refreshed successful, or an error response otherwise.</returns>
        [HttpPost(ApiRoutes.Authentication.RefreshToken)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken) =>
            await Result.Create(refreshTokenRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new RefreshUserTokenCommand(request.RefreshToken))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);
    }
}
