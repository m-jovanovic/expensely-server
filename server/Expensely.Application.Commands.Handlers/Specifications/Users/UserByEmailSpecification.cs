using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Domain.Core;

namespace Expensely.Application.Commands.Handlers.Specifications.Users
{
    /// <summary>
    /// Represents the specification for determining the user by email.
    /// </summary>
    public sealed class UserByEmailSpecification : Specification<User>
    {
        private readonly Email _email;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserByEmailSpecification"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        public UserByEmailSpecification(Email email) => _email = email;

        /// <inheritdoc />
        public override Expression<Func<User, bool>> ToExpression() => user => user.Email.Value == _email;
    }
}
