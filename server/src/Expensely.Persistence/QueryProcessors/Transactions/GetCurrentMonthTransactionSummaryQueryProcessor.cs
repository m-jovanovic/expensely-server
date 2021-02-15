﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Queries.Processors.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Abstractions.Maybe;
using Expensely.Domain.Core;
using Expensely.Persistence.Indexes.Transactions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Expensely.Persistence.QueryProcessors.Transactions
{
    /// <summary>
    /// Represents the <see cref="GetCurrentMonthTransactionSummaryQuery"/> processor.
    /// </summary>
    public sealed class GetCurrentMonthTransactionSummaryQueryProcessor : IGetCurrentMonthTransactionSummaryQueryProcessor
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IUserInformationProvider _userInformationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCurrentMonthTransactionSummaryQueryProcessor"/> class.
        /// </summary>
        /// <param name="session">The document session.</param>
        /// <param name="userInformationProvider">The user information provider.</param>
        public GetCurrentMonthTransactionSummaryQueryProcessor(
            IAsyncDocumentSession session,
            IUserInformationProvider userInformationProvider)
        {
            _session = session;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionSummaryResponse>> Process(
            GetCurrentMonthTransactionSummaryQuery query,
            CancellationToken cancellationToken = default)
        {
            if (query.UserId != _userInformationProvider.UserId ||
                _userInformationProvider.PrimaryCurrency.HasNoValue ||
                query.PrimaryCurrency != _userInformationProvider.PrimaryCurrency.Value.Value)
            {
                return Maybe<TransactionSummaryResponse>.None;
            }

            Transactions_Monthly.Result[] monthlyTransactions = await _session
                .Query<Transactions_Monthly.Result, Transactions_Monthly>()
                .Where(x =>
                    x.UserId == query.UserId &&
                    x.Year == query.StartOfMonth.Year &&
                    x.Month == query.StartOfMonth.Month &&
                    x.Currency == query.PrimaryCurrency)
                .ToArrayAsync(cancellationToken);

            Currency currency = Currency.FromValue(query.PrimaryCurrency).Value;

            string FormatAmount(TransactionType transactionType)
            {
                Transactions_Monthly.Result monthlyTransaction = monthlyTransactions
                    .FirstOrDefault(x => x.TransactionType == transactionType);

                return currency.Format(monthlyTransaction?.Amount ?? decimal.Zero);
            }

            var transactionSummaryResponse = new TransactionSummaryResponse
            {
                FormattedExpense = FormatAmount(TransactionType.Expense),
                FormattedIncome = FormatAmount(TransactionType.Income)
            };

            return transactionSummaryResponse;
        }
    }
}
