using Expensely.Common.Primitives.Errors;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.UnitTests.TestData.Currency;
using Expensely.Domain.UnitTests.TestData.User;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transaction
{
    public class TransactionTypeWithInvalidAmountData : TheoryData<Domain.Modules.Users.User, Domain.Modules.Common.Currency, TransactionType, decimal, Error>
    {
        public TransactionTypeWithInvalidAmountData()
        {
            Domain.Modules.Users.User user = UserTestData.ValidUser;

            Domain.Modules.Common.Currency currency = CurrencyTestData.DefaultCurrency;

            user.AddCurrency(currency);

            Add(user, currency, TransactionType.Expense, 0, DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);

            Add(user, currency, TransactionType.Expense, 1, DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);

            Add(user, currency, TransactionType.Income, 0, DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);

            Add(user, currency, TransactionType.Income, -1, DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
