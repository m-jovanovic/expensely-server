using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;
using FluentValidation;
using MediatR;
using ValidationException = Expensely.Application.Abstractions.Exceptions.ValidationException;

namespace Expensely.Application.Abstractions.Behaviors
{
    /// <summary>
    /// Represents the validation behavior middleware.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, ICommand<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest,TResponse}"/> class.
        /// </summary>
        /// <param name="validators">The validator for the current request type.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
