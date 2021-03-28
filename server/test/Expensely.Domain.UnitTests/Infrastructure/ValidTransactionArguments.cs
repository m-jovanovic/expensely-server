using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.Infrastructure
{
    public class ValidTransactionArguments : TheoryData<User, Description, Category, Money, DateTime, TransactionType>
    {
        public ValidTransactionArguments()
        {
            User user = UserTestData.ValidUser;

            Currency currency = CurrencyTestData.DefaultCurrency;

            user.AddCurrency(currency);

            Description description = DescriptionTestData.EmptyDescription;

            Add(user, description, Category.None, new Money(-1, currency), DateTime.UtcNow.Date, TransactionType.Expense);

            Add(user, description, Category.None, new Money(1, currency), DateTime.UtcNow.Date, TransactionType.Income);
        }
    }
}
