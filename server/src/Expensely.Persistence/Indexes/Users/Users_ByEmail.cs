using System.Linq;
using Expensely.Domain.Core;
using Raven.Client.Documents.Indexes;

namespace Expensely.Persistence.Indexes.Users
{
    /// <summary>
    /// Represents the index on users collection by email field.
    /// </summary>
    internal sealed class Users_ByEmail : AbstractIndexCreationTask<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Users_ByEmail"/> class.
        /// </summary>
        public Users_ByEmail() =>
            Map = users =>
                from user in users
                select new
                {
                    Email = user.Email.Value
                };
    }
}
