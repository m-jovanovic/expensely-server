using System.Text;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Common.Abstractions.Clock;
using Expensely.Domain.Services;
using Expensely.Infrastructure.Authentication;
using Expensely.Infrastructure.Authentication.Settings;
using Expensely.Infrastructure.Common;
using Expensely.Infrastructure.Cryptography;
using Expensely.Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure
{
    /// <summary>
    /// Contains the extensions method for registering dependencies in the DI framework.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]))
                });

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));

            services.AddTransient<IDateTime, MachineDateTime>();

            services.AddTransient<IPasswordService, PasswordService>();

            services.AddScoped<IUserInformationProvider, UserInformationProvider>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<IEventPublisher, EventPublisher>();

            return services;
        }
    }
}
