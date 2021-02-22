using System;

namespace Expensely.Common.Primitives.Errors
{
    /// <summary>
    /// Represents a concrete domain error.
    /// </summary>
    public sealed class Error : IEquatable<Error>
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
        /// Gets the empty error instance.
        /// </summary>
        public static Error None => new(string.Empty, string.Empty);

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        public static implicit operator string(Error error) => error?.Code ?? string.Empty;

        public static bool operator ==(Error a, Error b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Error a, Error b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(Error other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code && Message == other.Message;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is not Error error)
            {
                return false;
            }

            return Equals(error);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Code, Message);
    }
}
