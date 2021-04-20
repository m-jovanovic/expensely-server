using Expensely.Domain.Errors;
using Expensely.Domain.Exceptions;

namespace Expensely.Domain.Modules.Common.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when there is an attempt to perform
    /// an operation that is not supported with two different currencies.
    /// </summary>
    public sealed class CurrenciesDoNotMatchDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesDoNotMatchDomainException"/> class.
        /// </summary>
        /// <param name="currency1">The first currency.</param>
        /// <param name="currency2">The second currency.</param>
        public CurrenciesDoNotMatchDomainException(Currency currency1, Currency currency2)
            : base(DomainErrors.Money.CurrenciesDoNotMatch(currency1, currency2))
        {
        }
    }
}
