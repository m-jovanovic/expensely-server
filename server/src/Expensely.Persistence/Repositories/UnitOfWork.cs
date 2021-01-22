using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Repositories;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the unit of work.
    /// </summary>
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public UnitOfWork(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _session.SaveChangesAsync(cancellationToken);
    }
}
