using Expensely.Domain.Abstractions.Exceptions;
using Expensely.Domain.Abstractions.Primitives;
using Expensely.Domain.Core;

namespace Expensely.Domain.Exceptions
{
    /// <summary>
    /// Represents the exception that occurs when there is an attempt to perform
    /// an operation that is not supported for different currencies.
    /// </summary>
    public sealed class MoneyCurrenciesNotEqualDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyCurrenciesNotEqualDomainException"/> class.
        /// </summary>
        /// <param name="left">The first currency.</param>
        /// <param name="right">The second currency.</param>
        public MoneyCurrenciesNotEqualDomainException(Currency left, Currency right)
            : base(new Error("Money.CurrenciesNotEqual", $"The currencies {left.Name} and {right.Name} must be the same."))
        {
        }
    }
}
