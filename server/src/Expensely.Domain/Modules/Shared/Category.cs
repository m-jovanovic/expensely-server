using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Shared
{
    /// <summary>
    /// Represents the category enumeration.
    /// </summary>
    public sealed class Category : Enumeration<Category>
    {
        /// <summary>
        /// The none category.
        /// </summary>
        public static readonly Category None = new(1, "None");

        /// <summary>
        /// The shopping category.
        /// </summary>
        public static readonly Category Shopping = new(2, "Shopping");

        /// <summary>
        /// The groceries category.
        /// </summary>
        public static readonly Category Groceries = new(3, "Groceries");

        /// <summary>
        /// The food category.
        /// </summary>
        public static readonly Category Food = new(4, "Food");

        /// <summary>
        /// The drinks category.
        /// </summary>
        public static readonly Category Drinks = new(5, "Drinks");

        /// <summary>
        /// The clothing category.
        /// </summary>
        public static readonly Category Clothing = new(6, "Clothing");

        /// <summary>
        /// The travel category.
        /// </summary>
        public static readonly Category Travel = new(7, "Travel");

        /// <summary>
        /// The bills category.
        /// </summary>
        public static readonly Category Bills = new(8, "Bills");

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="value">The currency value.</param>
        /// <param name="name">The currency name.</param>
        private Category(int value, string name)
            : base(value, name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Category()
        {
        }
    }
}
