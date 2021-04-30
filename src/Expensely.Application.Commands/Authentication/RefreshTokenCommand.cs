using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Result;

namespace Expensely.Application.Commands.Authentication
{
    /// <summary>
    /// Represents the command for refreshing a user's token.
    /// </summary>
    public sealed class RefreshTokenCommand : ICommand<Result<TokenResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommand"/> class.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        public RefreshTokenCommand(string refreshToken) => RefreshToken = refreshToken;

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string RefreshToken { get; }
    }
}
