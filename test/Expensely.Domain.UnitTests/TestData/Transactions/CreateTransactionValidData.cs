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
    public class CreateTransactionValidData : TheoryData<CreateTransactionRequest>
    {
        public CreateTransactionValidData()
        {
            User user = UserTestData.ValidUser;

            Currency currency = CurrencyTestData.DefaultCurrency;

            user.AddCurrency(currency);

            var createExpenseRequest = new CreateTransactionRequest(
                user,
                DescriptionTestData.EmptyDescription,
                Category.None.Value,
                -1.0m,
                currency.Value,
                DateTime.UtcNow.Date,
                TransactionType.Expense.Value);

            Add(createExpenseRequest);

            var createIncomeRequest = new CreateTransactionRequest(
                user,
                DescriptionTestData.EmptyDescription,
                Category.None.Value,
                1.0m,
                currency.Value,
                DateTime.UtcNow.Date,
                TransactionType.Income.Value);

            Add(createIncomeRequest);
        }
    }
}
