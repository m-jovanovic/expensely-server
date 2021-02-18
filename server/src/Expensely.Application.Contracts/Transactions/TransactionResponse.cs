﻿using System;
using System.Globalization;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Transactions;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction response.
    /// </summary>
    public sealed class TransactionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionResponse"/> class.
        /// </summary>
        /// <param name="id">The transaction identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="category">The category.</param>
        /// <param name="money">The money amount.</param>
        /// <param name="occurredOn">The occurred on date.</param>
        public TransactionResponse(string id, Description description, Category category, Money money, DateTime occurredOn)
        {
            Id = id;
            Description = description;
            Category = category.Name;
            FormattedAmount = money.Format();
            OccurredOn = occurredOn.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
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
        public string OccurredOn { get; }
    }
}