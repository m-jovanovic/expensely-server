using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Messaging.Abstractions.Entities;

namespace Expensely.Messaging.Specifications
{
    /// <summary>
    /// Represents the specification for determining the unprocessed messages.
    /// </summary>
    public sealed class UnprocessedMessageSpecification : Specification<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessedMessageSpecification"/> class.
        /// </summary>
        /// <param name="take">The number of messages to take.</param>
        public UnprocessedMessageSpecification(int take)
        {
            ApplyOrderBy(x => x.CreatedOnUtc);

            Take = take;
        }

        /// <inheritdoc />
        public override Expression<Func<Message, bool>> ToExpression() => message => !message.Processed;
    }
}
