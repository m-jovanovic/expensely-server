using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Exceptions;
using Expensely.Domain.Abstractions.Exceptions;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Presentation.Api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Expensely.Presentation.Api.Middleware
{
    /// <summary>
    /// Represents the global exception handler middleware.
    /// </summary>
    internal sealed class GlobalExceptionHandlerMiddleware
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate pointing to the next middleware in the chain.</param>
        /// <param name="logger">The logger.</param>
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware with the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <returns>The task that can be awaited by the next middleware.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The HTTP response that is modified based on the exception.</returns>
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndErrors(exception);

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)httpStatusCode;

            string response = JsonSerializer.Serialize(new ApiErrorResponse(errors), JsonSerializerOptions);

            await httpContext.Response.WriteAsync(response);
        }

        /// <summary>
        /// Gets the HTTP status code and collection of errors for the specified exception.
        /// </summary>
        /// <param name="exception">The exception that has occurred.</param>
        /// <returns>The HTTP status code and collection of errors for the specified exception.</returns>
        private static (HttpStatusCode StatusCode, IReadOnlyCollection<Error> Errors) GetHttpStatusCodeAndErrors(Exception exception) =>
            exception switch
            {
                ValidationException validationException => (HttpStatusCode.UnprocessableEntity, validationException.Errors),
                DomainException domainException => (HttpStatusCode.UnprocessableEntity, new[] { domainException.Error }),
                _ => (HttpStatusCode.InternalServerError, new[] { ApiErrors.ServerError })
            };
    }
}
