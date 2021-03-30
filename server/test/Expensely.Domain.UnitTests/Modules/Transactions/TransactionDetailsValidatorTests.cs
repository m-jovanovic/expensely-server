using Expensely.Common.Primitives.Errors;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Description;
using Expensely.Domain.UnitTests.TestData.Transaction;
using Expensely.Domain.UnitTests.TestData.User;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Transactions
{
    public class TransactionDetailsValidatorTests
    {
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
        [ClassData(typeof(UserWithNoCurrencyData))]
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
        [ClassData(typeof(TransactionTypeWithInvalidAmountData))]
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
        [ClassData(typeof(CreateTransactionValidData))]
        public void Validate_ShouldReturnSuccessResult_WhenArgumentsAreValid(User user, ITransactionDetails transactionDetails)
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result result = transactionDetailsValidator.Validate(
                user,
                transactionDetails.Description,
                transactionDetails.Category.Value,
                transactionDetails.Money.Amount,
                transactionDetails.Money.Currency.Value,
                transactionDetails.OccurredOn,
                transactionDetails.TransactionType.Value);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(CreateTransactionValidData))]
        public void Validate_ShouldReturnTransactionDetailsWithProperValues_WhenArgumentsAreValid(
            User user,
            ITransactionDetails transactionDetails)
        {
            // Arrange
            var transactionDetailsValidator = new TransactionDetailsValidator();

            // Act
            Result<ITransactionDetails> result = transactionDetailsValidator.Validate(
                user,
                transactionDetails.Description,
                transactionDetails.Category.Value,
                transactionDetails.Money.Amount,
                transactionDetails.Money.Currency.Value,
                transactionDetails.OccurredOn,
                transactionDetails.TransactionType.Value);

            // Assert
            result.Value.Description.Should().Be(transactionDetails.Description);
            result.Value.Category.Should().Be(transactionDetails.Category);
            result.Value.Money.Should().Be(transactionDetails.Money);
            result.Value.OccurredOn.Should().Be(transactionDetails.OccurredOn);
            result.Value.TransactionType.Should().Be(transactionDetails.TransactionType);
        }
    }
}
