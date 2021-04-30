using Expensely.Common.Primitives.Errors;

namespace Expensely.Application.Commands.Handlers.Validation
{
    /// <summary>
    /// Contains the application layer errors.
    /// </summary>
    public static partial class ValidationErrors
    {
        /// <summary>
        /// Contains the category errors.
        /// </summary>
        public static class Category
        {
            /// <summary>
            /// Gets the category not found error.
            /// </summary>
            public static Error NotFound => new("Category.NotFound", "The category with the specified value was not found.");
        }
    }
}
