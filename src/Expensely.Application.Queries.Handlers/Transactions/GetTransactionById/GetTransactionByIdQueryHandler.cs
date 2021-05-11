using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Queries.Transactions;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Application.Queries.Handlers.Transactions.GetTransactionById
{
    /// <summary>
    /// Represents the <see cref="GetTransactionByIdQuery"/> handler.
    /// </summary>
    public sealed class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, Maybe<TransactionResponse>>
    {
        private readonly IUserInformationProvider _userInformationProvider;
        private readonly IGetTransactionByIdDataRequest _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransactionByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userInformationProvider">The user information provider.</param>
        /// <param name="request">The get transaction by identifier data request.</param>
        public GetTransactionByIdQueryHandler(IUserInformationProvider userInformationProvider, IGetTransactionByIdDataRequest request)
        {
            _request = request;
            _userInformationProvider = userInformationProvider;
        }

        /// <inheritdoc />
        public async Task<Maybe<TransactionResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            TransactionModel transactionModel = await _request.GetAsync(
                new GetTransactionByIdRequest(request.TransactionId.ToString()),
                cancellationToken);

            if (transactionModel is null || transactionModel.UserId != _userInformationProvider.UserId)
            {
                return Maybe<TransactionResponse>.None;
            }

            var transactionResponse = new TransactionResponse(
                transactionModel.Id,
                transactionModel.Description,
                transactionModel.Category,
                transactionModel.Amount,
                transactionModel.Currency,
                transactionModel.OccurredOn,
                transactionModel.TransactionType);

            return transactionResponse;
        }
    }
}
