using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Modules.Common;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionDetailsById
{
    /// <summary>
    /// Represents the <see cref="GetTransactionDetailsByIdQuery"/> handler.
    /// </summary>
    public sealed class GetTransactionDetailsByIdQueryHandler :
        IQueryHandler<GetTransactionDetailsByIdQuery, Maybe<TransactionDetailsResponse>>
    {
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IGetTransactionDetailsByIdDataRequest _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionDetailsByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="request">The get transaction details by identifier data request.</param>
        public GetTransactionDetailsByIdQueryHandler(
            IUserInformationProvider userInformationProvider,
            IGetTransactionDetailsByIdDataRequest request)
        {
            _userInformationProvider = userInformationProvider;
            _request = request;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionDetailsResponse>> Handle(
            GetTransactionDetailsByIdQuery request,
            CancellationToken cancellationToken)
        {
            TransactionDetailsModel transactionDetailsModel = await _request.GetAsync(
                new GetTransactionDetailsByIdRequest(request.TransactionId.ToString()),
                cancellationToken);

            if (transactionDetailsModel is null || transactionDetailsModel.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionDetailsResponse>.None;
            }

            Currency currency = Currency.FromValue(transactionDetailsModel.Currency).Value;

            var money = new Money(transactionDetailsModel.Amount, currency);

            var transactionDetailsResponse = new TransactionDetailsResponse(
                transactionDetailsModel.Id,
                transactionDetailsModel.Description,
                transactionDetailsModel.Category,
                money.Format(),
                transactionDetailsModel.OccurredOn);

            return transactionDetailsResponse;
        }
    }
}
