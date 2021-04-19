using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Descriptions;
using Expensely.Domain.UnitTests.TestData.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transactions
{
    public class CreateTransactionInvalidAmountForTransactionTypeData : TheoryData<User, ITransactionDetails>
    {
        public CreateTransactionInvalidAmountForTransactionTypeData()
        {
            User user = UserTestData.ValidUser;

            Add(user, new TransactionDetails
            {
                Description = DescriptionTestData.EmptyDescription,
                Category = Category.None,
                Money = new Money(1, CurrencyTestData.DefaultCurrency),
                OccurredOn = DateTime.UtcNow,
                TransactionType = TransactionType.Expense
            });

            Add(user, new TransactionDetails
            {
                Description = DescriptionTestData.EmptyDescription,
                Category = Category.None,
                Money = new Money(-1, CurrencyTestData.DefaultCurrency),
                OccurredOn = DateTime.UtcNow,
                TransactionType = TransactionType.Income
            });
        }
    }
}
