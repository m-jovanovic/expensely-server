namespace Expensely.Application.Contracts.Categories
{
    /// <summary>
    /// Represents the category response.
    /// </summary>
    public sealed class CategoryResponse
    {
        /// <summary>
        /// Gets the category identifier.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets the category name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets a value indicating whether or not the category is the default category.
        /// </summary>
        public bool IsDefault { get; init; }

        /// <summary>
        /// Gets a value indicating whether or not the category is an expense.
        /// </summary>
        public bool IsExpense { get; init; }
    }
}
