using System.Collections.Generic;
using Expensely.Application.Queries.Abstractions;
using Expensely.Contracts.Categories;

namespace Expensely.Application.Queries.Categories.GetCategories
{
    /// <summary>
    /// Represents the <see cref="GetCategoriesQuery"/> processor interface.
    /// </summary>
    public interface IGetCategoriesQueryProcessor : IQueryProcessor<GetCategoriesQuery, IReadOnlyCollection<CategoryResponse>>
    {
    }
}
