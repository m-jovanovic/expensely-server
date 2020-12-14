using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Utility;
using Expensely.Common.Clock;
using Expensely.Common.Messaging;
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
        where TResponse : class
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dateTime">The date and time.</param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IDateTime dateTime)
        {
            _logger = logger;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string requestName = request.GetType().Name;

            _logger.LogInformation(
                "Request started {@Request} {@UtcNow}.",
                requestName,
                _dateTime.UtcNow.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture));

            TResponse response = await next();

            _logger.LogInformation(
                "Request completed {@Request} {@UtcNow}.",
                requestName,
                _dateTime.UtcNow.ToString(DateTimeFormats.DateTimeWithMilliseconds, CultureInfo.InvariantCulture));

            return response;
        }
    }
}
