using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Queries.Utility;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Expensely.Api.Behaviors
{
    /// <summary>
    /// Represents the logging behavior middleware.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, ICommand<TResponse>
        where TResponse : Result
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="dateTime">The date and time.</param>
        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            IUserInformationProvider userInformationProvider,
            IDateTime dateTime)
        {
            _logger = logger;
            _dateTime = dateTime;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string requestName = request.GetType().Name;

            _logger.LogInformation(GetRequestStartedLogMessage(), GetLogArguments(requestName).ToArray());

            TResponse response = await next();

            _logger.LogInformation(GetRequestCompletedLogMessage(), GetLogArguments(requestName).ToArray());

            return response;
        }

        private string GetRequestStartedLogMessage() =>
            _userInformationProvider.IsAuthenticated
                ? "Request started {@UserId} {@Request} {@UtcNow}."
                : "Request started {@Request} {@UtcNow}.";

        private string GetRequestCompletedLogMessage() =>
            _userInformationProvider.IsAuthenticated
                ? "Request completed {@UserId} {@Request} {@UtcNow}."
                : "Request completed {@Request} {@UtcNow}.";

        private IEnumerable<object> GetLogArguments(string requestName)
        {
            if (_userInformationProvider.IsAuthenticated)
            {
                yield return _userInformationProvider.UserId;
            }

            yield return requestName;

            yield return _dateTime.UtcNow.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture);
        }
    }
}
