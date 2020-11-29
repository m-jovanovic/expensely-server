using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Queries.Transactions.Queries.GetCurrentMonthTransactionSummary;
using Expensely.Common.Clock;
using Expensely.Contracts.Transactions;
using Expensely.Domain.Primitives.Maybe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the transactions resource controller.
    /// </summary>
    public sealed class TransactionController : ApiController
    {
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dateTime">The date and time.</param>
        public TransactionController(ISender sender, IDateTime dateTime)
            : base(sender) =>
            _dateTime = dateTime;

        /// <summary>
        /// Gets the current month transaction summary for the specified parameters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="primaryCurrency">The primary currency.</param>
        /// <returns>200 - OK if the transaction summary is found, otherwise 404 - Not Found.</returns>
        [HttpGet(ApiRoutes.Transactions.GetCurrentMonthTransactionSummary)]
        [ProducesResponseType(typeof(TransactionSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentMonthTransactionSummary(Guid userId, int primaryCurrency) =>
            await Maybe<GetCurrentMonthTransactionSummaryQuery>
                .From(new GetCurrentMonthTransactionSummaryQuery(userId, primaryCurrency, _dateTime.UtcNow))
                .Bind(query => Sender.Send(query))
                .Match(Ok, NotFound);
    }
}
