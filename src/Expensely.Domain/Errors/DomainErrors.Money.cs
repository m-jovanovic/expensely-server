using System;
using Expensely.Common.Primitives.Errors;
using Expensely.Domain.Modules.Common;

namespace Expensely.Domain.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the money errors.
        /// </summary>
        public static class Money
        {
            /// <summary>
            /// Gets the budget end date precedes the start date error.
            /// </summary>
            public static Func<Currency, Currency, Error> CurrenciesDoNotMatch => (currency1, currency2) =>
                new Error("Money.CurrenciesDoNotMatch", $"The currency {currency1.Code} does not match {currency2.Code}.");
        }
    }
}
