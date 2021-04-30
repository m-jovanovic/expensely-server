using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Expensely.App.ServiceInstallers.Documentation
{
    /// <summary>
    /// Represents the authorization operation filter.
    /// </summary>
    public sealed class AuthorizationOperationFilter : IOperationFilter
    {
        private static readonly HttpStatusCode[] ResponseStatusCodes =
        {
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Forbidden
        };

        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            object[] customAttributes = context.MethodInfo.GetCustomAttributes(true);

            object[] declaringTypeCustomAttributes = context.MethodInfo.DeclaringType!.GetCustomAttributes(true);

            bool isAuthorized = declaringTypeCustomAttributes.OfType<AuthorizeAttribute>().Any() ||
                                      customAttributes.OfType<AuthorizeAttribute>().Any();

            bool isAnonymousAllowed = declaringTypeCustomAttributes.OfType<AllowAnonymousAttribute>().Any() ||
                                      customAttributes.OfType<AllowAnonymousAttribute>().Any();

            if (!isAuthorized || isAnonymousAllowed)
            {
                return;
            }

            foreach (HttpStatusCode statusCode in ResponseStatusCodes)
            {
                operation.Responses.TryAdd(
                    ((int)statusCode).ToString(CultureInfo.InvariantCulture),
                    new OpenApiResponse { Description = statusCode.ToString() });
            }

            var jwtBearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [jwtBearerScheme] = Array.Empty<string>()
                }
            };
        }
    }
}
