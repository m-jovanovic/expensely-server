using Expensely.Domain.Core;

namespace Expensely.Domain.Models
{
    /// <summary>
    /// Represents the transaction information.
    /// </summary>
    public class TransactionInformation
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public Name Name { get; init; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public Description Description { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public Currency Currency { get; init; }
    }
}
