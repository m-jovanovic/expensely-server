using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Users;
using Expensely.Persistence.Indexes.Users;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.Repositories
{
    /// <summary>
    /// Represents the user repository.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        public UserRepository(IAsyncDocumentSession session) => _session = session;

        /// <inheritdoc />
        public async Task<Maybe<User>> GetByIdAsync(Ulid userId, CancellationToken cancellationToken = default) =>
            await _session.LoadAsync<User>(userId.ToString(), cancellationToken);

        /// <inheritdoc />
        public async Task<Maybe<User>> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
            await _session
                .Query<User, Users_ByEmail>()
                .SingleOrDefaultAsync(x => x.Email.Value == email.Value, cancellationToken);

        /// <inheritdoc />
        public async Task<Maybe<User>> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default) =>
            await _session
                .Query<User, Users_ByRefreshToken>()
                .SingleOrDefaultAsync(x => x.RefreshToken.Token == refreshToken, cancellationToken);

        /// <inheritdoc />
        public async Task<bool> AnyWithEmailAsync(Email email, CancellationToken cancellationToken = default) =>
            await _session
                .Query<User, Users_ByEmail>()
                .AnyAsync(x => x.Email.Value == email.Value, cancellationToken);

        /// <inheritdoc />
        public async Task AddAsync(User user, CancellationToken cancellationToken = default) =>
            await _session.StoreAsync(user, cancellationToken);
    }
}
