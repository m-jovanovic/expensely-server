using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.UnitTests.TestData.Currency;
using Expensely.Domain.UnitTests.TestData.Description;
using Expensely.Domain.UnitTests.TestData.User;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transaction
{
    public class CreateTransactionInvalidCategoryForTransactionTypeData : TheoryData<Domain.Modules.Users.User, ITransactionDetails>
    {
        public CreateTransactionInvalidCategoryForTransactionTypeData()
        {
            Domain.Modules.Users.User user = UserTestData.ValidUser;

            Add(user, new TransactionDetails
            {
                Description = DescriptionTestData.EmptyDescription,
                Category = Category.Cash,
                Money = new Money(-1, CurrencyTestData.DefaultCurrency),
                OccurredOn = DateTime.UtcNow,
                TransactionType = TransactionType.Expense
            });

            Add(user, new TransactionDetails
            {
                Description = DescriptionTestData.EmptyDescription,
                Category = Category.Shopping,
                Money = new Money(1, CurrencyTestData.DefaultCurrency),
                OccurredOn = DateTime.UtcNow,
                TransactionType = TransactionType.Income
            });
        }
    }
}
