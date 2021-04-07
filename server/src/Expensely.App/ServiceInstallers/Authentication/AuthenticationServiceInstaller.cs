using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Expensely.App.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Authentication
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

            services.ConfigureOptions<JwtOptionsSetup>();

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.ConfigureOptions<AuthenticationOptionsSetup>();

            services.AddAuthentication().AddJwtBearer();
        }
    }
}
