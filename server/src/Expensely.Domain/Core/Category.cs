using Expensely.Domain.Abstractions.Primitives;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the category enumeration.
    /// </summary>
    public sealed class Category : Enumeration<Category>
    {
        /// <summary>
        /// The none category instance.
        /// </summary>
        public static readonly Category None = new(default, string.Empty);

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
        /// <param name="value">The category value.</param>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Category(int value)
            : base(value, ContainsValue(value) ? FromValue(value).Value.Name : None.Name)
        {
        }
    }
}
