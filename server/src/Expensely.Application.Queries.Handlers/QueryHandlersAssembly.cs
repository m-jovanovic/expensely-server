using System.Reflection;

namespace Expensely.Application.Queries.Handlers
{
    /// <summary>
    /// Represents the query handlers assembly.
    /// </summary>
    public static class QueryHandlersAssembly
    {
        /// <summary>
        /// Gets the query handlers assembly.
        /// </summary>
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
