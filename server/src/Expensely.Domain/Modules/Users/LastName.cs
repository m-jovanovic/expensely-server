using System.Collections.Generic;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the last name value object.
    /// </summary>
    public sealed class LastName : ValueObject
    {
        private const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastName"/> class.
        /// </summary>
        /// <param name="value">The last name value.</param>
        private LastName(string value)
            : this() =>
            Value = value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastName"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private LastName()
        {
        }

        /// <summary>
        /// Gets the last name value.
        /// </summary>
        public string Value { get; private set; }

        public static implicit operator string(LastName lastName) => lastName?.Value;

        /// <summary>
        /// Creates a new <see cref="FirstName"/> instance based on the specified value.
        /// </summary>
        /// <param name="lastName">The last name value.</param>
        /// <returns>The result of the last name creation process containing the last name or an error.</returns>
        public static Result<LastName> Create(string lastName) =>
            Result.Create(lastName, DomainErrors.LastName.NullOrEmpty)
                .Ensure(x => !string.IsNullOrWhiteSpace(x), DomainErrors.LastName.NullOrEmpty)
                .Ensure(x => x.Length <= MaxLength, DomainErrors.LastName.LongerThanAllowed)
                .Map(x => new LastName(x));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
