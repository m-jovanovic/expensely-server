using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Shared.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when there is an attempt to perform an operation with different currencies.
    /// </summary>
    public sealed class CurrenciesNotEqualDomainException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesNotEqualDomainException"/> class.
        /// </summary>
        /// <param name="first">The first currency.</param>
        /// <param name="second">The second currency.</param>
        public CurrenciesNotEqualDomainException(Currency first, Currency second)
            : base(new Error("Currency.NotEqual", $"The currencies {first.Name} and {second.Name} are not equal."))
        {
        }
    }
}
