﻿using Expensely.BackgroundTasks;
using Expensely.WebApp.Abstractions;
using Expensely.WebApp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Expensely.WebApp.ServiceInstallers.BackgroundTasks
{
    /// <summary>
    /// Represents the background tasks services installer.
    /// </summary>
    public sealed class BackgroundTasksServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services)
        {
            services.ConfigureOptions<QuartzHostedServiceOptionsSetup>();

            services.ConfigureOptions<MessageProcessingJobSettingsSetup>();

            services.ConfigureOptions<MessageProcessingJobSetup>();

            services.AddQuartz(configure => configure.UseMicrosoftDependencyInjectionScopedJobFactory());

            services.AddQuartzHostedService();

            services.AddTransientAsMatchingInterface(BackgroundTasksAssembly.Assembly);

            services.AddScopedAsMatchingInterface(BackgroundTasksAssembly.Assembly);
        }
    }
}
