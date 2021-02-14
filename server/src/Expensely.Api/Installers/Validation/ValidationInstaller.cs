using Expensely.Api.Abstractions;
using Expensely.Application.Commands.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.Installers.Validation
{
    /// <summary>
    /// Represents the validation installer.
    /// </summary>
    internal sealed class ValidationInstaller : IInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services) =>
            services.AddValidatorsFromAssembly(CommandHandlersAssembly.Assembly);
    }
}
