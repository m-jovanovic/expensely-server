using Expensely.Application.Abstractions.Common;
using Expensely.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

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
        public static void AddInfrastructure(this IServiceCollection services) => services.AddTransient<IDateTime, MachineDateTime>();
    }
}
