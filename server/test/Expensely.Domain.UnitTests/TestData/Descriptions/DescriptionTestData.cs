using Expensely.Domain.Modules.Transactions;

namespace Expensely.Domain.UnitTests.TestData.Descriptions
{
    public class DescriptionTestData
    {
        public static readonly Description EmptyDescription = Description.Create(string.Empty).Value;

        public static readonly string LongerThanAllowedDescription = new('*', Description.MaxLength + 1);
    }
}
