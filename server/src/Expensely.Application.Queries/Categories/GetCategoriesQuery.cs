﻿using System.Collections.Generic;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Contracts.Categories;

namespace Expensely.Application.Queries.Categories
{
    /// <summary>
    /// Represents the query for getting a collection of all supported categories.
    /// </summary>
    public sealed class GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryResponse>>
    {
    }
}
