using System.Collections.Generic;

namespace Expensely.Domain.Primitives
{
    /// <summary>
    /// Represents a concrete domain error.
    /// </summary>
    public sealed class Error : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the empty error instance.
        /// </summary>
        internal static Error None => new Error(string.Empty, string.Empty);

        /// <summary>
        /// Converts the error to its respective string representation.
        /// </summary>
        /// <param name="error">The error.</param>
        public static implicit operator string(Error error) => error?.Code ?? string.Empty;

        /// <inheritdoc />
        public override string ToString() => $"{Code}, {Message}";

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
        }
    }
}
