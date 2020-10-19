using System.Collections.Generic;
using Expensely.Domain.Primitives;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the description value object.
    /// </summary>
    public sealed class Description : ValueObject
    {
        /// <summary>
        /// The description maximum length.
        /// </summary>
        public const int MaxLength = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        /// <param name="value">The description value.</param>
        private Description(string value) => Value = value;

        /// <summary>
        /// Gets the description value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Description description) => description?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Description"/> instance based on the specified value.
        /// </summary>
        /// <param name="firstName">The description value.</param>
        /// <returns>The result of the description creation process containing the description or an error.</returns>
        public static Result<Description> Create(string firstName) =>
            Result.Create(firstName, DomainErrors.Description.NullOrEmpty)
                .Ensure(x => !string.IsNullOrWhiteSpace(x), DomainErrors.Description.NullOrEmpty)
                .Ensure(x => x.Length <= MaxLength, DomainErrors.Description.LongerThanAllowed)
                .Map(x => new Description(x));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
