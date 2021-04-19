using Expensely.Domain.Modules.Budgets;

namespace Expensely.Domain.UnitTests.TestData.Names
{
    public class NameTestData
    {
        public static readonly Name ValidName = Name.Create(nameof(Name)).Value;

        public static readonly string LongerThanAllowedName = new('*', Name.MaxLength + 1);
    }
}
