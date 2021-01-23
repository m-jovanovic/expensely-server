using System.Threading;
using System.Threading.Tasks;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Domain.Repositories;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the user repository.
    /// </summary>
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public UserRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<User>> GetByIdAsync(string userId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<User>(userId, cancellationToken);

        /// <inheritdoc />
        // TODO: Fix index usage.
        public async Task<Maybe<User>> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
            await _session
                .Query<User>()
                .SingleOrDefaultAsync(x => x.Email.Value == email.Value, cancellationToken);

        /// <inheritdoc />
        // TODO: Fix index usage.
        public async Task<bool> AnyWithEmailAsync(Email email, CancellationToken cancellationToken = default) =>
            await _session
                .Query<User>()
                .AnyAsync(x => x.Email.Value == email.Value, cancellationToken);

        /// <inheritdoc />
        public async Task AddAsync(User user, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(user, cancellationToken);
    }
}
