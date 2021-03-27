using System.Collections.Generic;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the first name value object.
    /// </summary>
    public sealed class FirstName : ValueObject
    {
        private const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstName"/> class.
        /// </summary>
        /// <param name="value">The first name value.</param>
        private FirstName(string value)
            : this() =>
            Value = value;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstName"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private FirstName()
        {
        }

        /// <summary>
        /// Gets the first name value.
        /// </summary>
        public string Value { get; private set; }

        public static implicit operator string(FirstName firstName) => firstName?.Value;

        /// <summary>
        /// Creates a new <see cref="FirstName"/> instance based on the specified value.
        /// </summary>
        /// <param name="firstName">The first name value.</param>
        /// <returns>The result of the first name creation process containing the first name or an error.</returns>
        public static Result<FirstName> Create(string firstName) =>
            Result.Create(firstName, DomainErrors.FirstName.NullOrEmpty)
                .Ensure(x => !string.IsNullOrWhiteSpace(x), DomainErrors.FirstName.NullOrEmpty)
                .Ensure(x => x.Length <= MaxLength, DomainErrors.FirstName.LongerThanAllowed)
                .Map(x => new FirstName(x));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
