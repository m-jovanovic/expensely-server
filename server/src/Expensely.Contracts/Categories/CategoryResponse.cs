namespace Expensely.Contracts.Categories
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
    }
}
