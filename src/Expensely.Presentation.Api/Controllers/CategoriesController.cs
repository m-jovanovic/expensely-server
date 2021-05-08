using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Categories;
using Expensely.Application.Queries.Categories;
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
        /// <returns>The collection of all supported categories.</returns>
        [HasPermission(Permission.CategoryRead)]
        [HttpGet(ApiRoutes.Categories.GetCategories)]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken) =>
            await Sender.Send(new GetCategoriesQuery(), cancellationToken).Map(Ok);
    }
}
