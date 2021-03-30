namespace Expensely.Domain.UnitTests.TestData.Description
{
    public class DescriptionTestData
    {
        public static readonly Domain.Modules.Transactions.Description EmptyDescription = Domain.Modules.Transactions.Description.Create(string.Empty).Value;

        public static readonly string LongerThanAllowedDescription = new('*', Domain.Modules.Transactions.Description.MaxLength + 1);
    }
}
