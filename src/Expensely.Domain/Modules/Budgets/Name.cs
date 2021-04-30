using System.Collections.Generic;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the name value object.
    /// </summary>
    public sealed class Name : ValueObject
    {
        /// <summary>
        /// The name maximum length.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="Name"/> class.
        /// </summary>
        /// <param name="value">The name value.</param>
        private Name(string value)
            : this() =>
            Value = value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Name"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Name()
        {
        }

        /// <summary>
        /// Gets the name value.
        /// </summary>
        public string Value { get; private set; }

        public static implicit operator string(Name name) => name?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Name"/> instance based on the specified value.
        /// </summary>
        /// <param name="name">The name value.</param>
        /// <returns>The result of the name creation process containing the name or an error.</returns>
        public static Result<Name> Create(string name) =>
            Result.Create(name, DomainErrors.Name.NullOrEmpty)
                .Ensure(x => !string.IsNullOrWhiteSpace(x), DomainErrors.Name.NullOrEmpty)
                .Ensure(x => x.Length <= MaxLength, DomainErrors.Name.LongerThanAllowed)
                .Map(x => new Name(x));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
