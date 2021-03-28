using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transactions
{
    public class CreateTransactionArgumentExceptionArguments : TheoryData<User, ITransactionDetails, string>
    {
        public CreateTransactionArgumentExceptionArguments() =>
            Add(
                UserTestData.ValidUser,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = Category.None,
                    Money = new Money(default, CurrencyTestData.DefaultCurrency),
                    OccurredOn = default,
                    TransactionType = TransactionType.Expense
                },
                "occurredOn");
    }
}
