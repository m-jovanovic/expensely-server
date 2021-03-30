using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.UnitTests.TestData.Currency;
using Expensely.Domain.UnitTests.TestData.Description;
using Expensely.Domain.UnitTests.TestData.User;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transaction
{
    public class CreateTransactionArgumentExceptionData : TheoryData<Domain.Modules.Users.User, ITransactionDetails, string>
    {
        public CreateTransactionArgumentExceptionData() =>
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
