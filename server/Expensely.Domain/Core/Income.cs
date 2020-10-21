using System;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Core
{
    /// <summary>
    /// Represents the income monetary transaction.
    /// </summary>
    public class Income : Transaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="name">The name of the income.</param>
        /// <param name="money">The monetary amount of the income.</param>
        /// <param name="occurredOn">The date the income occurred on.</param>
        /// <param name="description">The description of the income.</param>
        public Income(Guid userId, Name name, Money money, DateTime occurredOn, Description description)
            : base(userId, name, money, occurredOn, description) =>
            Ensure.NotLessThanOrEqualToZero(money.Amount, "The monetary amount must be greater than zero", nameof(money));

        /// <summary>
        /// Initializes a new instance of the <see cref="Income"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Income()
        {
        }
    }
}
