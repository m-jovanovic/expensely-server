using System;
using Expensely.Application.Contracts.Categories;
using Expensely.Domain.Modules.Transactions;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction response.
    /// </summary>
    public sealed class TransactionResponse
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public CategoryResponse Category { get; init; }

        /// <summary>
        /// Gets the formatted amount.
        /// </summary>
        public string FormattedAmount { get; init; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public int Currency { get; init; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; init; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public int TransactionType { get; init; }

        /// <summary>
        /// Creates a new <see cref="TransactionResponse"/> from the specified <see cref="Transaction"/> instance.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The new <see cref="TransactionResponse"/> instance.</returns>
        public static TransactionResponse FromTransaction(Transaction transaction) =>
            new()
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Category = new CategoryResponse
                {
                    Id = transaction.Category.Value,
                    Name = transaction.Category.Name,
                    IsExpense = transaction.Category.IsExpense,
                    IsDefault = transaction.Category.IsDefault
                },
                FormattedAmount = transaction.Money.Format(),
                Amount = transaction.Money.Amount,
                Currency = transaction.Money.Currency.Value,
                OccurredOn = transaction.OccurredOn,
                TransactionType = transaction.TransactionType.Value
            };
    }
}
