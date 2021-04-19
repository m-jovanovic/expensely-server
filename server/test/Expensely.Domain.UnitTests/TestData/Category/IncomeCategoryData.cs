using System.Linq;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Category
{
    public sealed class IncomeCategoryData : TheoryData<Domain.Modules.Common.Category>
    {
        public IncomeCategoryData()
        {
            foreach (Domain.Modules.Common.Category category in Domain.Modules.Common.Category.List.Where(x => !x.IsExpense && !x.IsDefault))
            {
                Add(category);
            }
        }
    }
}
