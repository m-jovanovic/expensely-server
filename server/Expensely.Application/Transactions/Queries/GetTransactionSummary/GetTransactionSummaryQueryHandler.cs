using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Transactions;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Transactions.Queries.GetTransactionSummary
{
    /// <summary>
    /// Represents the <see cref="GetTransactionSummaryQuery"/> handler.
    /// </summary>
    internal sealed class GetTransactionSummaryQueryHandler
        : IQueryHandler<GetTransactionSummaryQuery, Maybe<TransactionSummaryResponse>>
    {
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionSummaryQueryHandler"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="dbContext">The database context.</param>
        public GetTransactionSummaryQueryHandler(IUserInformationProvider userInformationProvider, IDbContext dbContext)
        {
            _userInformationProvider = userInformationProvider;
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionSummaryResponse>> Handle(
            GetTransactionSummaryQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionSummaryResponse>.None;
            }

            var transactionAmountsPerTypeDictionary = (
                    await _dbContext.Set<Transaction>()
                        .AsNoTracking()
                        .Where(x =>
                            x.UserId == request.UserId &&
                            x.Money.Currency.Value == request.PrimaryCurrency)
                        .GroupBy(x => EF.Property<int>(x, "TransactionType"))
                        .Select(grouping => new TransactionAmountPerType
                        {
                            TransactionType = grouping.Key,
                            Amount = grouping.Sum(x => x.Money.Amount)
                        })
                        .ToListAsync(cancellationToken))
                .ToDictionary(x => x.TransactionType, x => x.Amount);

            Currency currency = Currency.FromValue(request.PrimaryCurrency).Value;

            var response = new TransactionSummaryResponse
            {
                FormattedExpense = FormatAmountForTransactionType(transactionAmountsPerTypeDictionary, TransactionType.Expense, currency),
                FormattedIncome = FormatAmountForTransactionType(transactionAmountsPerTypeDictionary, TransactionType.Income, currency)
            };

            return response;
        }

        private static string FormatAmountForTransactionType(
            IReadOnlyDictionary<int, decimal> transactionAmountsDictionary,
            TransactionType transactionType,
            Currency currency) =>
            $"{GetAmountForTransactionType(transactionAmountsDictionary, transactionType)} {currency.Code}";

        private static decimal GetAmountForTransactionType(
            IReadOnlyDictionary<int, decimal> transactionAmountsDictionary,
            TransactionType transactionType) =>
            transactionAmountsDictionary.TryGetValue((int)transactionType, out decimal amount)
                ? amount
                : decimal.Zero;
    }
}
