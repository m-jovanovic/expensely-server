using Raven.Client.Documents.Linq;

namespace Expensely.Persistence.Extensions
{
    /// <summary>
    /// Contains the extension methods for the <see cref="IRavenQueryable{T}"/> class.
    /// </summary>
    internal static class RavenQueryableExtensions
    {
        /// <summary>
        /// Customizes the <see cref="IRavenQueryable{T}"/> to not track entities.
        /// </summary>
        /// <typeparam name="T">The query result type.</typeparam>
        /// <param name="query">The query instance.</param>
        /// <returns>The customized query.</returns>
        internal static IRavenQueryable<T> WithNoTracking<T>(this IRavenQueryable<T> query) => query.Customize(x => x.NoTracking());
    }
}
