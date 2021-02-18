using Expensely.Application.Contracts.Users;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Users
{
    /// <summary>
    /// Represents the command for refreshing a user's token.
    /// </summary>
    public sealed class RefreshUserTokenCommand : ICommand<Result<TokenResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshUserTokenCommand"/> class.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        public RefreshUserTokenCommand(string refreshToken) => RefreshToken = refreshToken;

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string RefreshToken { get; }
    }
}
