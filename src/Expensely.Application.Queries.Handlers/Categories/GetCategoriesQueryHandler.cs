using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Categories;
using Expensely.Application.Queries.Categories;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Modules.Common;

namespace Expensely.Application.Queries.Handlers.Categories
{
    /// <summary>
    /// Represents the <see cref="GetCategoriesQuery"/> handler.
    /// </summary>
    public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IEnumerable<CategoryResponse>>
    {
        /// <inheritdoc />
        public Task<IEnumerable<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<CategoryResponse> categories = Category
                .List
                .Select(x => new CategoryResponse
                {
                    Id = x.Value,
                    Name = x.Name,
                    IsDefault = x.IsDefault,
                    IsExpense = x.IsExpense
                }).ToList();

            return Task.FromResult(categories);
        }
    }
}
