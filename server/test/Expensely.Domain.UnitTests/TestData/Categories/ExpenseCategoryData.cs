using System.Linq;
using Expensely.Domain.Modules.Common;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Categories
{
    public sealed class ExpenseCategoryData : TheoryData<Category>
    {
        public ExpenseCategoryData()
        {
            foreach (Category category in Category.List.Where(x => x.IsExpense && !x.IsDefault))
            {
                Add(category);
            }
        }
    }
}
