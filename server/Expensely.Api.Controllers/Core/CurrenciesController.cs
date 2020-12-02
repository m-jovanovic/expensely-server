using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Queries.Currencies.GetCurrencies;
using Expensely.Contracts.Currencies;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
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
        /// <returns>The readonly collection of all supported currencies</returns>
        [HttpGet(ApiRoutes.Currencies.GetCurrencies)]
        [ProducesResponseType(typeof(IReadOnlyCollection<CurrencyResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrencies() => Ok(await Sender.Send(new GetCurrenciesQuery()));
    }
}
