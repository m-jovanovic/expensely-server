using System.Text;
using Expensely.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.App.ServiceInstallers.Authentication
{
    /// <summary>
    /// Represents the <see cref="JwtBearerOptions"/> setup.
    /// </summary>
    public sealed class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
    {
        private const string ConfigurationSectionName = "Authentication";
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtBearerOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="options">The JWT options.</param>
        public JwtBearerOptionsSetup(IConfiguration configuration, IOptions<JwtOptions> options)
        {
            _configuration = configuration;
            _options = options.Value;
        }

        /// <inheritdoc />
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);

            options.TokenValidationParameters.ValidIssuer = _options.Issuer;

            options.TokenValidationParameters.ValidAudience = _options.Audience;

            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey));
        }
    }
}
