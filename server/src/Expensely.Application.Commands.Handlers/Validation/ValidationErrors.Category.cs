using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    internal static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the category errors.
        /// </summary>
        internal static class Category
        {
            /// <summary>
            /// Gets the category not found error.
            /// </summary>
            internal static Error NotFound => new("Category.NotFound", "The category with the specified value was not found.");
        }
    }
}
