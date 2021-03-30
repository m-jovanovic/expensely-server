using Expensely.Common.Primitives.Errors;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transactions
{
    public class TransactionTypeWithInvalidAmountData : TheoryData<User, Currency, TransactionType, decimal, Error>
    {
        public TransactionTypeWithInvalidAmountData()
        {
            User user = UserTestData.ValidUser;

            Currency currency = CurrencyTestData.DefaultCurrency;

            user.AddCurrency(currency);

            Add(user, currency, TransactionType.Expense, 0, DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);

            Add(user, currency, TransactionType.Expense, 1, DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);

            Add(user, currency, TransactionType.Income, 0, DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);

            Add(user, currency, TransactionType.Income, -1, DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
