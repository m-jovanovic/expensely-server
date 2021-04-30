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
    public class UserAndTransactionDetailsValidData : TheoryData<User, ITransactionDetails>
    {
        public UserAndTransactionDetailsValidData()
        {
            User user = UserTestData.ValidUser;

            Currency currency = CurrencyTestData.DefaultCurrency;

            user.AddCurrency(currency);

            var expenseTransactionDetails = new TransactionDetails
            {
                Description = DescriptionTestData.EmptyDescription,
                Category = Category.None,
                Money = new Money(-1, currency),
                OccurredOn = DateTime.UtcNow.Date,
                TransactionType = TransactionType.Expense
            };

            Add(user, expenseTransactionDetails);

            var incomeTransactionDetails = new TransactionDetails
            {
                Description = DescriptionTestData.EmptyDescription,
                Category = Category.None,
                Money = new Money(1, currency),
                OccurredOn = DateTime.UtcNow.Date,
                TransactionType = TransactionType.Income
            };

            Add(user, incomeTransactionDetails);
        }
    }
}
