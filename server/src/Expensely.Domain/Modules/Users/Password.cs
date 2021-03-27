using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the password value object.
    /// </summary>
    public sealed class Password : ValueObject
    {
        private const int MinPasswordLength = 6;
        private static readonly Func<char, bool> IsLower = c => c >= 'a' && c <= 'z';
        private static readonly Func<char, bool> IsUpper = c => c >= 'A' && c <= 'Z';
        private static readonly Func<char, bool> IsNumber = c => c >= '0' && c <= '9';
        private static readonly Func<char, bool> IsNonAlphaNumeric = c => !(IsLower(c) || IsUpper(c) || IsNumber(c));

        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        /// <param name="value">The password value.</param>
        private Password(string value) => Value = value;

        /// <summary>
        /// Gets the password value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Password password) => password?.Value;

        /// <summary>
        /// Creates a new <see cref="Password"/> instance based on the specified value.
        /// </summary>
        /// <param name="password">The password value.</param>
        /// <returns>The result of the password creation process containing the password or an error.</returns>
        public static Result<Password> Create(string password) =>
            Result.Create(password, DomainErrors.Password.NullOrEmpty)
                .Ensure(x => !string.IsNullOrWhiteSpace(x), DomainErrors.Password.NullOrEmpty)
                .Ensure(x => x.Length >= MinPasswordLength, DomainErrors.Password.NotLongEnough)
                .Ensure(x => x.Any(IsLower), DomainErrors.Password.MissingLowercaseLetter)
                .Ensure(x => x.Any(IsUpper), DomainErrors.Password.MissingUppercaseLetter)
                .Ensure(x => x.Any(IsNumber), DomainErrors.Password.MissingNumber)
                .Ensure(x => x.Any(IsNonAlphaNumeric), DomainErrors.Password.MissingNonAlphaNumeric)
                .Map(x => new Password(x));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
