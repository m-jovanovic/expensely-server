using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Authentication;
using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Primitives.Result;
using Expensely.Presentation.Api.Constants;
using Expensely.Presentation.Api.Errors;
using Expensely.Presentation.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Presentation.Api.Controllers
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
        /// <returns>The JWT if the login was successful, otherwise an error response.</returns>
        [HttpPost(ApiRoutes.Authentication.Login)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken) =>
            await Result.Create(loginRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new LoginCommand(request.Email, request.Password))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Registers a user based on the specified request.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The empty response if the operation was successful, otherwise an error response.</returns>
        [HttpPost(ApiRoutes.Authentication.Register)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken) =>
            await Result.Create(registerRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new RegisterCommand(
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
        /// <returns>The JWT if the token was refreshed successful, otherwise an error response.</returns>
        [HttpPost(ApiRoutes.Authentication.RefreshToken)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken) =>
            await Result.Create(refreshTokenRequest, ApiErrors.UnProcessableRequest)
                .Map(request => new RefreshTokenCommand(request.RefreshToken))
                .Bind(command => Sender.Send(command, cancellationToken))
                .Match(Ok, BadRequest);
    }
}
