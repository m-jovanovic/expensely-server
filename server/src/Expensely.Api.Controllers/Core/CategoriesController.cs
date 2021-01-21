using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers.Constants;
using Expensely.Api.Controllers.Infrastructure;
using Expensely.Application.Queries.Categories;
using Expensely.Contracts.Categories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers.Core
{
    /// <summary>
    /// Represents the categories resource controller.
    /// </summary>
    public sealed class CategoriesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public CategoriesController(ISender sender)
            : base(sender)
        {
        }

        /// <summary>
        /// Gets the readonly collection of all supported categories.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The readonly collection of all supported categories.</returns>
        [HttpGet(ApiRoutes.Categories.GetCategories)]
        [ProducesResponseType(typeof(IReadOnlyCollection<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken) =>
            Ok(await Sender.Send(new GetCategoriesQuery(), cancellationToken));
    }
}
