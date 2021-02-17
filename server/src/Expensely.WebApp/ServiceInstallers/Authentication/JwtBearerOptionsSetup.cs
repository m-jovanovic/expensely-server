using System.Text;
using Expensely.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.WebApp.ServiceInstallers.Authentication
{
    /// <summary>
    /// Represents the <see cref="JwtBearerOptions"/> setup.
    /// </summary>
    public sealed class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
    {
        private const string ConfigurationSectionName = "Authentication";
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtBearerOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="jwtSettingsOptions">The JWT settings.</param>
        public JwtBearerOptionsSetup(IConfiguration configuration, IOptions<JwtSettings> jwtSettingsOptions)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettingsOptions.Value;
        }

        /// <inheritdoc />
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);

            options.TokenValidationParameters.ValidIssuer = _jwtSettings.Issuer;

            options.TokenValidationParameters.ValidAudience = _jwtSettings.Audience;

            options.TokenValidationParameters.IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
        }
    }
}
