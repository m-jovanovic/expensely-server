using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Categories;
using Expensely.Application.Queries.Categories;
using Expensely.Application.Queries.Processors.Categories;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.Handlers.Categories
{
    /// <summary>
    /// Represents the <see cref="GetCategoriesQuery"/> handler.
    /// </summary>
    public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryResponse>>
    {
        private readonly IGetCategoriesQueryProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCategoriesQueryHandler"/> class.
        /// </summary>
        /// <param name="processor">The get categories query processor.</param>
        public GetCategoriesQueryHandler(IGetCategoriesQueryProcessor processor) => _processor = processor;

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken) =>
            await _processor.Process(request, cancellationToken);
    }
}
