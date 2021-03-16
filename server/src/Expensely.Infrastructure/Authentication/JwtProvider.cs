﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Authentication;
using Expensely.Common.Abstractions.Clock;
using Expensely.Common.Abstractions.ServiceLifetimes;
using Expensely.Domain.Modules.Authentication;
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
        private readonly JwtSettings _settings;
        private readonly ISystemTime _systemTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtProvider"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT settings options.</param>
        /// <param name="systemTime">The current date and time.</param>
        public JwtProvider(IOptions<JwtSettings> jwtOptions, ISystemTime systemTime)
        {
            _settings = jwtOptions.Value;
            _systemTime = systemTime;
        }

        /// <inheritdoc />
        public AccessTokens CreateAccessTokens(User user)
        {
            string token = CreateToken(user);

            RefreshToken refreshToken = CreateRefreshToken();

            return new AccessTokens(token, refreshToken);
        }

        private static IEnumerable<Claim> CreateClaims(User user)
        {
            yield return new Claim(JwtRegisteredClaimNames.Sub, user.Id);
            yield return new Claim(JwtRegisteredClaimNames.Email, user.Email);
            yield return new Claim(CustomJwtClaimTypes.Name, user.GetFullName());
            yield return new Claim(
                CustomJwtClaimTypes.PrimaryCurrency,
                user.PrimaryCurrency is null ? string.Empty : user.PrimaryCurrency.Value.ToString(CultureInfo.InvariantCulture));
        }

        private string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime tokenExpirationTime = _systemTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _settings.Issuer,
                _settings.Audience,
                CreateClaims(user),
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
                _systemTime.UtcNow.AddMinutes(_settings.RefreshTokenExpirationInMinutes));
        }
    }
}
