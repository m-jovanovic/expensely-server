using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Infrastructure.Authentication.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the JWT provider.
    /// </summary>
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtProvider"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT options.</param>
        /// <param name="dateTime">The current date and time.</param>
        public JwtProvider(IOptions<JwtSettings> jwtOptions, IDateTime dateTime)
        {
            _jwtSettings = jwtOptions.Value;
            _dateTime = dateTime;
        }

        /// <inheritdoc />
        public string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime tokenExpirationTime = _dateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                CreateClaims(user),
                null,
                tokenExpirationTime,
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        /// <inheritdoc />
        public RefreshToken CreateRefreshToken()
        {
            var refreshTokenBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(refreshTokenBytes);

            return new RefreshToken(
                Convert.ToBase64String(refreshTokenBytes),
                _dateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationInMinutes));
        }

        /// <summary>
        /// Creates the collection of claims for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The collection of claims for the specified user.</returns>
        private static IEnumerable<Claim> CreateClaims(User user)
        {
            yield return new Claim(JwtClaimTypes.UserId, user.Id);
            yield return new Claim(JwtClaimTypes.Email, user.Email);
            yield return new Claim(JwtClaimTypes.Name, user.GetFullName());

            Maybe<Currency> maybePrimaryCurrency = user.GetPrimaryCurrency();

            yield return new Claim(
               JwtClaimTypes.PrimaryCurrency,
               maybePrimaryCurrency.HasValue ? maybePrimaryCurrency.Value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }
    }
}
