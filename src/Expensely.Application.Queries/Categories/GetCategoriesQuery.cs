using System.Collections.Generic;
using Expensely.Application.Contracts.Categories;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Categories
{
    /// <summary>
    /// Represents the query for getting the collection of all supported categories.
    /// </summary>
    public sealed class GetCategoriesQuery : IQuery<IEnumerable<CategoryResponse>>
    {
    }
}
