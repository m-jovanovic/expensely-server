using Expensely.App.Abstractions;
using Expensely.Application.Commands.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.App.ServiceInstallers.Validation
{
    /// <summary>
    /// Represents the validation services installer.
    /// </summary>
    public sealed class ValidationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services) =>
            services.AddValidatorsFromAssembly(CommandHandlersAssembly.Assembly);
    }
}
