using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Currencies;
using Expensely.Application.Queries.Currencies;
using Expensely.Authorization.Abstractions;
using Expensely.Authorization.Attributes;
using Expensely.Common.Primitives.Extensions;
using Expensely.Presentation.Api.Constants;
using Expensely.Presentation.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Presentation.Api.Controllers
{
    /// <summary>
    /// Represents the currencies resource controller.
    /// </summary>
    public sealed class CurrenciesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public CurrenciesController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Gets the readonly collection of all supported currencies.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection of all supported currencies.</returns>
        [HasPermission(Permission.CurrencyRead)]
        [HttpGet(ApiRoutes.Currencies.GetCurrencies)]
        [ProducesResponseType(typeof(IEnumerable<CurrencyResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken) =>
            await Sender.Send(new GetCurrenciesQuery(), cancellationToken).Map(Ok);
    }
}
