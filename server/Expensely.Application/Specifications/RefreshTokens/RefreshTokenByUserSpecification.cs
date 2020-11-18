using System;
using System.Linq.Expressions;
using Expensely.Domain.Core;

namespace Expensely.Application.Specifications.RefreshTokens
{
    /// <summary>
    /// Represents the specification for determining the refresh token by user.
    /// </summary>
    public sealed class RefreshTokenByUserSpecification : Specification<RefreshToken>
    {
        private readonly Guid _userId;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenByUserSpecification"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public RefreshTokenByUserSpecification(User user) => _userId = user.Id;

        /// <inheritdoc />
        public override Expression<Func<RefreshToken, bool>> ToExpression() => x => x.UserId == _userId;
    }
}
