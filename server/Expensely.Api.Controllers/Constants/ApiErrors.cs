using Expensely.Domain.Primitives;

namespace Expensely.Api.Controllers.Constants
{
    /// <summary>
    /// Contains the API errors.
    /// </summary>
    internal static class ApiErrors
    {
        /// <summary>
        /// Gets the un-processable request error.
        /// </summary>
        internal static Error UnProcessableRequest => new Error("API.UnProcessableRequest", "The server could not process the request.");

        /// <summary>
        /// Gets the server error error.
        /// </summary>
        internal static Error ServerError => new Error("API.ServerError", "The server encountered an unrecoverable error.");
    }
}
