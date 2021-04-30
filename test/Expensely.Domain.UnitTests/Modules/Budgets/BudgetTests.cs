using System;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Budgets;
using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Budgets.Exceptions;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Budgets;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Names;
using Expensely.Domain.UnitTests.TestData.Users;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Budgets
{
    public class BudgetTests
    {
        [Theory]
        [ClassData(typeof(CreateBudgetArgumentNullExceptionData))]
        public void Create_ShouldThrowArgumentNullException_WhenArgumentsAreInvalid(
            User user,
            IBudgetDetails budgetDetails,
            string paramName)
        {
            // Arrange
            // Act
            Action action = () => Budget.Create(user, budgetDetails);

            // Assert
            FluentActions.Invoking(action).Should().Throw<ArgumentNullException>().And.ParamName.Should().Be(paramName);
        }

        [Theory]
        [ClassData(typeof(CreateBudgetArgumentExceptionData))]
        public void Create_ShouldThrowArgumentException_WhenArgumentsAreInvalid(
            User user,
            IBudgetDetails budgetDetails,
            string paramName)
        {
            // Arrange
            // Act
            Action action = () => Budget.Create(user, budgetDetails);

            // Assert
            FluentActions.Invoking(action).Should().Throw<ArgumentException>().And.ParamName.Should().Be(paramName);
        }

        [Fact]
        public void Create_ShouldThrowBudgetEndDatePrecedesStartDateDomainException_WhenEndDatePrecedesStartDate()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            var budgetDetails = new BudgetDetails
            {
                Name = NameTestData.ValidName,
                Categories = Array.Empty<Category>(),
                Money = new Money(1.0m, CurrencyTestData.DefaultCurrency),
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(-1)
            };

            // Act
            Action action = () => Budget.Create(user, budgetDetails);

            // Assert
            FluentActions.Invoking(action).Should().Throw<BudgetEndDatePrecedesStartDateDomainException>()
                .And.Error.Should().Be(DomainErrors.Budget.EndDatePrecedesStartDate(budgetDetails.StartDate, budgetDetails.EndDate));
        }

        [Fact]
        public void Create_ShouldCreateBudget_WithProperValues()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            var budgetDetails = new BudgetDetails
            {
                Name = NameTestData.ValidName,
                Categories = new[] { Category.Bills, Category.Clothing },
                Money = new Money(1.0m, CurrencyTestData.DefaultCurrency),
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(1)
            };

            // Act
            var budget = Budget.Create(user, budgetDetails);

            // Assert
            budget.UserId.Should().Be(Ulid.Parse(user.Id));
            budget.Name.Should().Be(budgetDetails.Name);
            budget.Categories.Should().Equal(budgetDetails.Categories);
            budget.Money.Should().Be(budgetDetails.Money);
            budget.StartDate.Should().Be(budgetDetails.StartDate);
            budget.EndDate.Should().Be(budgetDetails.EndDate);
            budget.Expired.Should().BeFalse();
            budget.CreatedOnUtc.Should().Be(default);
            budget.ModifiedOnUtc.Should().BeNull();
        }
    }
}
