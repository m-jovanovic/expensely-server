using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.TimeZones;
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
    /// Represents the authentication controller.
    /// </summary>
    public sealed class TimeZonesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZonesController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public TimeZonesController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Gets the readonly collection of all supported time zones.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection of all supported time zones.</returns>
        [HasPermission(Permission.TimeZoneRead)]
        [HttpGet(ApiRoutes.TimeZones.GetTimeZones)]
        [ProducesResponseType(typeof(IEnumerable<GetTimeZonesQuery>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeZones(CancellationToken cancellationToken) =>
            await Sender.Send(new GetTimeZonesQuery(), cancellationToken).Map(Ok);
    }
}
