﻿using System.Threading.Tasks;
using Expensely.Api.Constants;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Users;
using Expensely.Application.Users.Commands.CreateTokenForUser;
using Expensely.Application.Users.Commands.CreateUser;
using Expensely.Domain.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
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
        /// Performs the login and returns a new JWT if successful.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns>The JWT if the login was successful, or an error response otherwise.</returns>
        [HttpPost(ApiRoutes.Authentication.Login)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) =>
            await Result.Create(loginRequest, Errors.UnProcessableRequest)
                .Map(request => new CreateTokenForUserCommand(request.Email, request.Password))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);

        /// <summary>
        /// Performs the registration and returns a new JWT if successful.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <returns>The JWT if the registration was successful, or an error response otherwise.</returns>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Authentication.Register)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest) =>
            await Result.Create(registerRequest, Errors.UnProcessableRequest)
                .Map(request => new CreateUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.ConfirmationPassword))
                .Bind(command => Sender.Send(command))
                .Match(Ok, BadRequest);
    }
}