using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the JWT provider.
    /// </summary>
    public sealed class JwtProvider : IJwtProvider, ITransient
    {
        private readonly JwtOptions _options;
        private readonly IClaimsProvider _claimsProvider;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtProvider"/> class.
        /// </summary>
        /// <param name="options">The JWT options.</param>
        /// <param name="claimsProvider">The claims provider.</param>
        /// <param name="systemTime">The current date and time.</param>
        public JwtProvider(IOptions<JwtOptions> options, IClaimsProvider claimsProvider, ISystemTime systemTime)
        {
            _options = options.Value;
            _systemTime = systemTime;
            _claimsProvider = claimsProvider;
        }

        /// <inheritdoc />
        public AccessTokens GetAccessTokens(User user)
        {
            string token = CreateToken(user);

            RefreshToken refreshToken = CreateRefreshToken();

            return new AccessTokens(token, refreshToken);
        }

        private string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime tokenExpirationTime = _systemTime.UtcNow.AddMinutes(_options.AccessTokenExpirationInMinutes);

            IEnumerable<Claim> claims = _claimsProvider.GetClaimsForUser(user);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                tokenExpirationTime,
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        private RefreshToken CreateRefreshToken()
        {
            var refreshTokenBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(refreshTokenBytes);

            return new RefreshToken(
                Convert.ToBase64String(refreshTokenBytes),
                _systemTime.UtcNow.AddMinutes(_options.RefreshTokenExpirationInMinutes));
        }
    }
}
