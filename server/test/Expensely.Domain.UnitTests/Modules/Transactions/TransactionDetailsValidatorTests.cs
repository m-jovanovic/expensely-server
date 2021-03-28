using System;
using Expensely.Common.Primitives.Errors;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Transactions
{
    public class TransactionDetailsValidatorTests
    {
        public static TheoryData<User, Currency> UserThatHasNoCurrencyArguments => new()
        {
            { UserTestData.ValidUser, CurrencyTestData.DefaultCurrency }
        };

        public static TheoryData<User, Currency, TransactionType, decimal, Error> TransactionTypeWithInvalidAmountArguments
        {
            get
            {
                var theoryData = new TheoryData<User, Currency, TransactionType, decimal, Error>();

                User user = UserTestData.ValidUser;

                Currency currency = CurrencyTestData.DefaultCurrency;

                user.AddCurrency(currency);

                theoryData.Add(user, currency, TransactionType.Expense, 0, DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);
                theoryData.Add(user, currency, TransactionType.Expense, 1, DomainErrors.Transaction.ExpenseAmountGreaterThanOrEqualToZero);
                theoryData.Add(user, currency, TransactionType.Income, 0, DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
                theoryData.Add(user, currency, TransactionType.Income, -1, DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);

                return theoryData;
            }
        }

        public static TheoryData<User, Description, Category, Money, DateTime, TransactionType> ValidArguments
        {
            get
            {
                var theoryData = new TheoryData<User, Description, Category, Money, DateTime, TransactionType>();

                User user = UserTestData.ValidUser;

                Currency currency = CurrencyTestData.DefaultCurrency;

                user.AddCurrency(currency);

                Description description = DescriptionTestData.EmptyDescription;

                theoryData.Add(user, description, Category.None, new Money(-1, currency), DateTime.UtcNow, TransactionType.Expense);
                theoryData.Add(user, description, Category.None, new Money(1, currency), DateTime.UtcNow, TransactionType.Income);

                return theoryData;
            }
        }

        [Fact]
        public void Validate_ShouldReturnFailureResult_WhenDescriptionIsLongerThanAllowed()
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result result = transactionDetailsValidator.Validate(
                default,
                DescriptionTestData.LongerThanAllowedDescription,
                default,
                default,
                default,
                default,
                default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(UserThatHasNoCurrencyArguments))]
        public void Validate_ShouldReturnFailureResult_WhenUserDoesNotHaveCurrency(User user, Currency currency)
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result result = transactionDetailsValidator.Validate(
                user,
                DescriptionTestData.EmptyDescription,
                default,
                default,
                currency.Value,
                default,
                default);

            // Assert
            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Theory]
        [MemberData(nameof(TransactionTypeWithInvalidAmountArguments))]
        public void Validate_ShouldReturnFailureResult_WhenAmountIsInvalidForTransactionType(
            User user,
            Currency currency,
            TransactionType transactionType,
            decimal amount,
            Error expectedError)
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result result = transactionDetailsValidator.Validate(
                user,
                DescriptionTestData.EmptyDescription,
                default,
                amount,
                currency.Value,
                default,
                transactionType.Value);

            // Assert
            result.Error.Should().Be(expectedError);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Validate_ShouldReturnSuccessResult_WhenArgumentsAreValid(
            User user,
            Description description,
            Category category,
            Money money,
            DateTime occurredOn,
            TransactionType transactionType)
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result result = transactionDetailsValidator
                .Validate(user, description, category.Value, money.Amount, money.Currency.Value, occurredOn, transactionType.Value);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Validate_ShouldReturnTransactionDetailsWithProperValues_WhenArgumentsAreValid(
            User user,
            Description description,
            Category category,
            Money money,
            DateTime occurredOn,
            TransactionType transactionType)
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result<TransactionDetails> result = transactionDetailsValidator
                .Validate(user, description, category.Value, money.Amount, money.Currency.Value, occurredOn, transactionType.Value);

            // Assert
            result.Value.Description.Should().Be(description);
            result.Value.Category.Should().Be(category);
            result.Value.Money.Should().Be(money);
            result.Value.OccurredOn.Should().Be(occurredOn);
            result.Value.TransactionType.Should().Be(transactionType);
        }
    }
}
