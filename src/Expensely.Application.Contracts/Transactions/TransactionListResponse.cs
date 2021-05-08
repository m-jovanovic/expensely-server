using System;
using System.Collections.Generic;
using Expensely.Domain.Modules.Common;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction list response.
    /// </summary>
    public sealed class TransactionListResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionListResponse"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="cursor">The cursor.</param>
        public TransactionListResponse(IEnumerable<TransactionListItem> items, string cursor = "")
        {
            Cursor = cursor;
            Items = items;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IEnumerable<TransactionListItem> Items { get; }

        /// <summary>
        /// Gets the cursor.
        /// </summary>
        public string Cursor { get; }

        /// <summary>
        /// Represents the <see cref="TransactionListResponse"/> item.
        /// </summary>
        public sealed record TransactionListItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TransactionListItem"/> class.
            /// </summary>
            /// <param name="id">The identifier.</param>
            /// <param name="description">The description.</param>
            /// <param name="category">The category.</param>
            /// <param name="money">The money.</param>
            /// <param name="occurredOn">The occurred on date.</param>
            public TransactionListItem(string id, string description, string category, Money money, DateTime occurredOn)
            {
                Id = id;
                Description = description;
                Category = category;
                FormattedAmount = money.Format();
                OccurredOn = occurredOn;
            }

            /// <summary>
            /// Gets the identifier.
            /// </summary>
            public string Id { get; }

            /// <summary>
            /// Gets the description.
            /// </summary>
            public string Description { get; }

            /// <summary>
            /// Gets the category.
            /// </summary>
            public string Category { get; }

            /// <summary>
            /// Gets the formatted amount.
            /// </summary>
            public string FormattedAmount { get; }

            /// <summary>
            /// Gets the occurred on date.
            /// </summary>
            public DateTime OccurredOn { get; }
        }
    }
}
