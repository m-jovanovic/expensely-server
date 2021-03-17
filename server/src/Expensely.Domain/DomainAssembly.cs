using System.Reflection;

namespace Expensely.Domain
{
    /// <summary>
    /// Represents the domain assembly.
    /// </summary>
    public static class DomainAssembly
    {
        /// <summary>
        /// Gets the domain assembly.
        /// </summary>
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
