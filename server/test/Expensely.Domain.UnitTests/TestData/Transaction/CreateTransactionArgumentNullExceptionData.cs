using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.UnitTests.TestData.Currency;
using Expensely.Domain.UnitTests.TestData.Description;
using Expensely.Domain.UnitTests.TestData.User;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transaction
{
    public class CreateTransactionArgumentNullExceptionData : TheoryData<Domain.Modules.Users.User, ITransactionDetails, string>
    {
        public CreateTransactionArgumentNullExceptionData()
        {
            Add(null, new TransactionDetails(), "user");

            Domain.Modules.Users.User user = UserTestData.ValidUser;

            Add(
                user,
                new TransactionDetails
                {
                    Description = null
                },
                "description");

            Add(
                user,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = null
                },
                "category");

            Add(
                user,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = Category.None,
                    Money = null
                },
                "money");

            Add(
                user,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = Category.None,
                    Money = new Money(default, CurrencyTestData.DefaultCurrency),
                    OccurredOn = DateTime.UtcNow,
                    TransactionType = null
                },
                "transactionType");
        }
    }
}
