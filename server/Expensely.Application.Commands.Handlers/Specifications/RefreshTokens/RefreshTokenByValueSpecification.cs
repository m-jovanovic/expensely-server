using System;
using System.Linq.Expressions;
using Expensely.Application.Abstractions.Specifications;
using Expensely.Domain.Core;

namespace Expensely.Application.Commands.Handlers.Specifications.RefreshTokens
{
    /// <summary>
    /// Represents the specification for determining the refresh token by value.
    /// </summary>
    public sealed class RefreshTokenByValueSpecification : Specification<RefreshToken>
    {
        private readonly string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenByValueSpecification"/> class.
        /// </summary>
        /// <param name="token">The token value.</param>
        public RefreshTokenByValueSpecification(string token) => _token = token;

        /// <inheritdoc />
        public override Expression<Func<RefreshToken, bool>> ToExpression() => x => x.Token == _token;
    }
}
