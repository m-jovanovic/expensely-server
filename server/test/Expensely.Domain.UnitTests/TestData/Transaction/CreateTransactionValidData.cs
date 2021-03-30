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
    public class CreateTransactionValidData : TheoryData<Domain.Modules.Users.User, ITransactionDetails>
    {
        public CreateTransactionValidData()
        {
            Domain.Modules.Users.User user = UserTestData.ValidUser;

            Domain.Modules.Common.Currency currency = CurrencyTestData.DefaultCurrency;

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
                Money = new Money(-1, currency),
                OccurredOn = DateTime.UtcNow.Date,
                TransactionType = TransactionType.Expense
            };

            Add(user, incomeTransactionDetails);
        }
    }
}
