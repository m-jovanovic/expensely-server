using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Expensely.WebApp.ServiceInstallers.Authentication
{
    /// <summary>
    /// Represents the <see cref="AuthenticationOptions"/> setup.
    /// </summary>
    public sealed class AuthenticationOptionsSetup : IConfigureOptions<AuthenticationOptions>
    {
        /// <inheritdoc />
        public void Configure(AuthenticationOptions options) => options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}
