using System.Reflection;

namespace Expensely.Application.Commands.Handlers
{
    /// <summary>
    /// Represents the command handlers assembly.
    /// </summary>
    public static class CommandHandlersAssembly
    {
        /// <summary>
        /// Gets the command handlers assembly.
        /// </summary>
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
