using System.Collections.Generic;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the description value object.
    /// </summary>
    public sealed class Description : ValueObject
    {
        /// <summary>
        /// The description maximum length.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        /// <param name="value">The description value.</param>
        private Description(string value)
            : this() =>
            Value = value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Description()
        {
        }

        /// <summary>
        /// Gets the description value.
        /// </summary>
        public string Value { get; private set; }

        public static implicit operator string(Description description) => description?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Description"/> instance based on the specified value.
        /// </summary>
        /// <param name="description">The description value.</param>
        /// <returns>The result of the description creation process containing the description or an error.</returns>
        public static Result<Description> Create(string description) =>
            Result.Success(description ?? string.Empty)
                .Ensure(x => x.Length <= MaxLength, DomainErrors.Description.LongerThanAllowed)
                .Map(x => new Description(x));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
