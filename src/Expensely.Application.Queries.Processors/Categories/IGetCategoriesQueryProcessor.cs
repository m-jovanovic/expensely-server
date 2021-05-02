using System.Collections.Generic;
using Expensely.Application.Contracts.Categories;
using Expensely.Application.Queries.Categories;
using Expensely.Application.Queries.Processors.Abstractions;

namespace Expensely.Application.Queries.Processors.Categories
{
    /// <summary>
    /// Represents the <see cref="GetCategoriesQuery"/> processor interface.
    /// </summary>
    public interface IGetCategoriesQueryProcessor : IQueryProcessor<GetCategoriesQuery, IEnumerable<CategoryResponse>>
    {
    }
}
