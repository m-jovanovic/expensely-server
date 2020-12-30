using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Queries.Categories.GetCategories;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Contracts.Categories;
using Expensely.Domain.Core;

namespace Expensely.Application.Queries.Handlers.Categories.GetCategories
{
    /// <summary>
    /// Represents the <see cref="GetCategoriesQuery"/> handler.
    /// </summary>
    public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryResponse>>
    {
        /// <inheritdoc />
        public Task<IReadOnlyCollection<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<CategoryResponse> categories = Category
                .List
                .Select(x => new CategoryResponse
                {
                    Value = x.Value,
                    Name = x.Name
                }).ToList();

            return Task.FromResult(categories);
        }
    }
}
