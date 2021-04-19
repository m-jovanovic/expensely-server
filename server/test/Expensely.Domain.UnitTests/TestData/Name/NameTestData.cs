namespace Expensely.Domain.UnitTests.TestData.Name
{
    public class NameTestData
    {
        public static readonly Domain.Modules.Budgets.Name ValidName = Domain.Modules.Budgets.Name.Create(nameof(Name)).Value;

        public static readonly string LongerThanAllowedName = new('*', Domain.Modules.Budgets.Name.MaxLength + 1);
    }
}
