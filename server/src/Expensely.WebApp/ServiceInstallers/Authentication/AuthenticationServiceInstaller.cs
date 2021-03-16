using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Expensely.WebApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.WebApp.ServiceInstallers.Authentication
{
    /// <summary>
    /// Represents the authentication services installer.
    /// </summary>
    public class AuthenticationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub] = ClaimTypes.Name;

            services.ConfigureOptions<JwtSettingsSetup>();

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.ConfigureOptions<AuthenticationOptionsSetup>();

            services.AddAuthentication().AddJwtBearer();
        }
    }
}
