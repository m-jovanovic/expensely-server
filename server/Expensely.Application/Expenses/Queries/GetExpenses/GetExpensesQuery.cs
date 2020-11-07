using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Utility;
using Expensely.Domain.Primitives.Maybe;

namespace Expensely.Application.Expenses.Queries.GetExpenses
{
    /// <summary>
    /// Represents the query for getting a list of expenses.
    /// </summary>
    public sealed class GetExpensesQuery : IQuery<Maybe<ExpenseListResponse>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpensesQuery"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="cursor">The cursor.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        public GetExpensesQuery(Guid userId, int limit, string cursor, DateTime utcNow)
        {
            UserId = userId;
            Limit = LimitFactory.GetLimit(limit);
            (OccurredOn, CreatedOnUtc) = ParseCursor(cursor, utcNow);
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Gets the occurred on date.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        public DateTime CreatedOnUtc { get; }

        /// <summary>
        /// Parses the specified cursor value and returns the parsed values.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        /// <returns>The parsed cursor values if the cursor is not empty, otherwise the current date and time in UTC format.</returns>
        private static (DateTime OccurredOn, DateTime CreatedOnUtc) ParseCursor(string cursor, DateTime utcNow)
        {
            if (string.IsNullOrWhiteSpace(cursor))
            {
                return (utcNow, utcNow);
            }

            string[] cursorValues = Cursor.Parse(cursor, 2);

            return (
                DateTime.TryParse(cursorValues[0], out DateTime occurredOn) ? occurredOn : utcNow.Date,
                DateTime.TryParse(cursorValues[1], out DateTime createdOnUtc) ? createdOnUtc : utcNow.Date);
        }
    }
}
