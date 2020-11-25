using System;
using System.Threading.Tasks;
using Expensely.Api.Constants;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Transactions.Queries.GetTransactionSummary;
using Expensely.Domain.Primitives.Maybe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    /// <summary>
    /// Represents the transactions resource controller.
    /// </summary>
    public sealed class TransactionController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public TransactionController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Gets the transaction summary for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="primaryCurrency">The primary currency.</param>
        /// <returns>200 - OK if the transaction summary is found, otherwise 404 - Not Found.</returns>
        [HttpGet(ApiRoutes.Transactions.GetTransactionSummary)]
        [ProducesResponseType(typeof(TransactionSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionSummary(Guid userId, int primaryCurrency) =>
            await Maybe<GetTransactionSummaryQuery>
                .From(new GetTransactionSummaryQuery(userId, primaryCurrency))
                .Bind(query => Sender.Send(query))
                .Match(Ok, NotFound);
    }
}
