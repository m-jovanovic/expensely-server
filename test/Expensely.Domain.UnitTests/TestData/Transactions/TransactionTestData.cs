using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Descriptions;
using Expensely.Domain.UnitTests.TestData.Users;

namespace Expensely.Domain.UnitTests.TestData.Transactions
{
    public static class TransactionTestData
    {
        public static Transaction ValidExpense
        {
            get
            {
                User user = UserTestData.ValidUser;

                user.AddCurrency(CurrencyTestData.DefaultCurrency);

                var transactionDetails = new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = Category.Bills,
                    Money = new Money(-1.0m, CurrencyTestData.DefaultCurrency),
                    OccurredOn = DateTime.UtcNow.Date,
                    TransactionType = TransactionType.Expense
                };

                return Transaction.Create(user, transactionDetails);
            }
        }
    }
}
