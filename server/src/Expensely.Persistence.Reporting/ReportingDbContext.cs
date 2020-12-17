using System.Reflection;
using Expensely.Application.Abstractions.Data;
using Expensely.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Persistence.Reporting
{
    /// <summary>
    /// Represents the reporting database context.
    /// </summary>
    public sealed class ReportingDbContext : BaseDbContext, IReportingDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
