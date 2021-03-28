using System;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Transactions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Transactions
{
    public class TransactionFactoryTests
    {
        private readonly Mock<ITransactionDetailsValidator> _transactionDetailsValidatorMock;

        public TransactionFactoryTests() => _transactionDetailsValidatorMock = new Mock<ITransactionDetailsValidator>();

        [Fact]
        public void Create_ShouldReturnFailureResult_WhenTransactionDetailsValidatorReturnsFailureResult()
        {
            // Arrange
            _transactionDetailsValidatorMock.Setup(x =>
                    x.Validate(
                        It.IsAny<User>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<decimal>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(Result.Failure<ITransactionDetails>(DomainErrors.User.CurrencyDoesNotExist));

            var transactionFactory = new TransactionFactory(_transactionDetailsValidatorMock.Object);

            // Act
            Result<Transaction> result = transactionFactory.Create(default, default, default, default, default, default, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(CreateTransactionValidArguments))]
        public void Create_ShouldReturnSuccessResult_WhenTransactionDetailsValidatorReturnsSuccessResult(
            User user,
            ITransactionDetails transactionDetails)
        {
            // Arrange
            _transactionDetailsValidatorMock.Setup(x =>
                    x.Validate(
                        It.Is<User>(u => u == user),
                        It.Is<string>(d => d == transactionDetails.Description),
                        It.Is<int>(c => c == transactionDetails.Category.Value),
                        It.Is<decimal>(a => a == transactionDetails.Money.Amount),
                        It.Is<int>(c => c == transactionDetails.Money.Currency.Value),
                        It.Is<DateTime>(o => o == transactionDetails.OccurredOn),
                        It.Is<int>(t => t == transactionDetails.TransactionType.Value)))
                .Returns(Result.Success(transactionDetails));

            var transactionFactory = new TransactionFactory(_transactionDetailsValidatorMock.Object);

            // Act
            Result<Transaction> result = transactionFactory.Create(
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
        [ClassData(typeof(CreateTransactionValidArguments))]
        public void Create_ShouldCreateTransaction_WhenTransactionDetailsValidatorReturnsSuccessResult(
            User user,
            ITransactionDetails transactionDetails)
        {
            // Arrange
            _transactionDetailsValidatorMock.Setup(x =>
                    x.Validate(
                        It.Is<User>(u => u == user),
                        It.Is<string>(d => d == transactionDetails.Description),
                        It.Is<int>(c => c == transactionDetails.Category.Value),
                        It.Is<decimal>(a => a == transactionDetails.Money.Amount),
                        It.Is<int>(c => c == transactionDetails.Money.Currency.Value),
                        It.Is<DateTime>(o => o == transactionDetails.OccurredOn),
                        It.Is<int>(t => t == transactionDetails.TransactionType.Value)))
                .Returns(Result.Success(transactionDetails));

            var transactionFactory = new TransactionFactory(_transactionDetailsValidatorMock.Object);

            // Act
            Result<Transaction> result = transactionFactory.Create(
                user,
                transactionDetails.Description,
                transactionDetails.Category.Value,
                transactionDetails.Money.Amount,
                transactionDetails.Money.Currency.Value,
                transactionDetails.OccurredOn,
                transactionDetails.TransactionType.Value);

            // Assert
            result.Value.UserId.Should().Be(Ulid.Parse(user.Id));
            result.Value.Description.Should().Be(transactionDetails.Description);
            result.Value.Category.Should().Be(transactionDetails.Category);
            result.Value.Money.Should().Be(transactionDetails.Money);
            result.Value.OccurredOn.Should().Be(transactionDetails.OccurredOn);
            result.Value.TransactionType.Should().Be(transactionDetails.TransactionType);
        }
    }
}
