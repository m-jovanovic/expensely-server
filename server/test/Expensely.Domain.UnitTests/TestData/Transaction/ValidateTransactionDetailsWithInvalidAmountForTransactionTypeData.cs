using Expensely.Common.Primitives.Errors;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
using Expensely.Domain.UnitTests.TestData.Currency;
using Expensely.Domain.UnitTests.TestData.User;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transaction
{
    public class ValidateTransactionDetailsWithInvalidAmountForTransactionTypeData : TheoryData<ValidateTransactionDetailsRequest, Error>
    {
        public ValidateTransactionDetailsWithInvalidAmountForTransactionTypeData()
        {
            Domain.Modules.Users.User user = UserTestData.ValidUser;

            Domain.Modules.Common.Currency currency = CurrencyTestData.DefaultCurrency;

            Category category = Category.None;

            user.AddCurrency(currency);

            Add(
                new ValidateTransactionDetailsRequest(
                    user,
                    default,
                    category.Value,
                    0.0m,
                    currency.Value,
                    default,
                    TransactionType.Expense.Value),
                DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);

            Add(
                new ValidateTransactionDetailsRequest(
                    user,
                    default,
                    category.Value,
                    1.0m,
                    currency.Value,
                    default,
                    TransactionType.Expense.Value),
                DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);

            Add(
                new ValidateTransactionDetailsRequest(
                    user,
                    default,
                    category.Value,
                    0.0m,
                    currency.Value,
                    default,
                    TransactionType.Income.Value),
                DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);

            Add(
                new ValidateTransactionDetailsRequest(
                    user,
                    default,
                    category.Value,
                    -1.0m,
                    currency.Value,
                    default,
                    TransactionType.Income.Value),
                DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
