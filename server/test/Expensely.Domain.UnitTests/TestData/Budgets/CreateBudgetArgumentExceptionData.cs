using System;
using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Names;
using Expensely.Domain.UnitTests.TestData.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Budgets
{
    public class CreateBudgetArgumentExceptionData : TheoryData<User, IBudgetDetails, string>
    {
        public CreateBudgetArgumentExceptionData()
        {
            User user = UserTestData.ValidUser;

            Add(
                user,
                new BudgetDetails
                {
                    Name = null
                },
                "name");

            Add(
                user,
                new BudgetDetails
                {
                    Name = NameTestData.ValidName,
                    Money = Money.Zero(CurrencyTestData.DefaultCurrency),
                    Categories = Array.Empty<Category>(),
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                },
                "money");

            Add(
                user,
                new BudgetDetails
                {
                    Name = NameTestData.ValidName,
                    Money = Money.Zero(CurrencyTestData.DefaultCurrency),
                    Categories = Array.Empty<Category>(),
                    StartDate = default
                },
                "startDate");

            Add(
                user,
                new BudgetDetails
                {
                    Name = NameTestData.ValidName,
                    Money = Money.Zero(CurrencyTestData.DefaultCurrency),
                    Categories = Array.Empty<Category>(),
                    StartDate = DateTime.UtcNow,
                    EndDate = default
                },
                "endDate");
        }
    }
}
