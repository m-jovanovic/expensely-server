using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Exceptions;
using Expensely.Common.Primitives.Errors;
using Expensely.Domain.Exceptions;
using Expensely.Presentation.Api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Expensely.App.Middleware
{
    /// <summary>
    /// Represents the global exception handler middleware.
    /// </summary>
    public sealed class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) => _logger = logger;

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="context">The HTTP httpContext.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The HTTP response that is modified based on the exception.</returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(exception);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)httpStatusCode;

            string response = JsonSerializer.Serialize(new ApiErrorResponse(errors), JsonSerializerOptions);

            await context.Response.WriteAsync(response);
        }

        /// <summary>
        /// Gets the HTTP status code and collection of errors for the specified exception.
        /// </summary>
        /// <param name="exception">The exception that has occurred.</param>
        /// <returns>The HTTP status code and collection of errors for the specified exception.</returns>
        private static (HttpStatusCode StatusCode, IReadOnlyCollection<Error> Errors) GetHttpStatusCodeAndErrors(Exception exception) =>
            exception switch
            {
                ValidationException validationException => (HttpStatusCode.BadRequest, validationException.Errors),
                DomainException domainException => (HttpStatusCode.UnprocessableEntity, new[] { domainException.Error }),
                _ => (HttpStatusCode.InternalServerError, new[] { ApiErrors.ServerError })
            };
    }
}
