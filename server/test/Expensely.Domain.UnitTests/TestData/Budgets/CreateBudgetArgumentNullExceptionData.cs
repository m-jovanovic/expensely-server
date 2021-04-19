using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Names;
using Expensely.Domain.UnitTests.TestData.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Budgets
{
    public class CreateBudgetArgumentNullExceptionData : TheoryData<User, IBudgetDetails, string>
    {
        public CreateBudgetArgumentNullExceptionData()
        {
            Add(null, new BudgetDetails(), "user");

            User user = UserTestData.ValidUser;

            Add(
                user,
                new BudgetDetails
                {
                    Name = NameTestData.ValidName,
                    Money = null
                },
                "money");

            Add(
                user,
                new BudgetDetails
                {
                    Name = NameTestData.ValidName,
                    Money = Money.Zero(CurrencyTestData.DefaultCurrency),
                    Categories = null
                },
                "categories");
        }
    }
}
