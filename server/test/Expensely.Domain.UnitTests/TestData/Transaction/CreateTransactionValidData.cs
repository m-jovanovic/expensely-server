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
    public class CreateTransactionValidData : TheoryData<CreateTransactionRequest>
    {
        public CreateTransactionValidData()
        {
            Domain.Modules.Users.User user = UserTestData.ValidUser;

            Domain.Modules.Common.Currency currency = CurrencyTestData.DefaultCurrency;

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
