using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Categories;
using Expensely.Application.Queries.Categories;
using Expensely.Application.Queries.Processors.Categories;
using Expensely.Domain.Modules.Common;

namespace Expensely.Persistence.QueryProcessors.Categories
{
    /// <summary>
    /// Represents the <see cref="GetCategoriesQuery"/> processor.
    /// </summary>
    public sealed class GetCategoriesQueryProcessor : IGetCategoriesQueryProcessor
    {
        /// <inheritdoc />
        public Task<IReadOnlyCollection<CategoryResponse>> Process(GetCategoriesQuery query, CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<CategoryResponse> categories = Category
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
