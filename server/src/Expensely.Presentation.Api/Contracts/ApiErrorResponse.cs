using System.Collections.Generic;
using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Presentation.Api.Contracts
{
    /// <summary>
    /// Represents API an error response.
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiErrorResponse"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public ApiErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IReadOnlyCollection<Error> Errors { get; }
    }
}
