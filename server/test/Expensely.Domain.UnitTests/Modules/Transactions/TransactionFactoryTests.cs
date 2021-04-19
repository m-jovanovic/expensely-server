using System;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Transactions.Contracts;
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
            _transactionDetailsValidatorMock.Setup(x => x.Validate(It.IsAny<ValidateTransactionDetailsRequest>()))
                .Returns(Result.Failure<ITransactionDetails>(DomainErrors.User.CurrencyDoesNotExist));

            var transactionFactory = new TransactionFactory(_transactionDetailsValidatorMock.Object);

            var createTransactionRequest = new CreateTransactionRequest(default, default, default, default, default, default, default);

            // Act
            Result<Transaction> result = transactionFactory.Create(createTransactionRequest);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(CreateTransactionValidData))]
        public void Create_ShouldReturnSuccessResult_WhenTransactionDetailsValidatorReturnsSuccessResult(
            CreateTransactionRequest createTransactionRequest)
        {
            // Arrange
            ITransactionDetails transactionDetails = CreateTransactionDetails(createTransactionRequest);

            _transactionDetailsValidatorMock.Setup(x => x.Validate(It.IsAny<ValidateTransactionDetailsRequest>()))
                .Returns(Result.Success(transactionDetails));

            var transactionFactory = new TransactionFactory(_transactionDetailsValidatorMock.Object);

            // Act
            Result<Transaction> result = transactionFactory.Create(createTransactionRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(CreateTransactionValidData))]
        public void Create_ShouldCreateTransaction_WhenTransactionDetailsValidatorReturnsSuccessResult(
            CreateTransactionRequest createTransactionRequest)
        {
            // Arrange
            ITransactionDetails transactionDetails = CreateTransactionDetails(createTransactionRequest);

            _transactionDetailsValidatorMock.Setup(x => x.Validate(It.IsAny<ValidateTransactionDetailsRequest>()))
                .Returns(Result.Success(transactionDetails));

            var transactionFactory = new TransactionFactory(_transactionDetailsValidatorMock.Object);

            // Act
            Result<Transaction> result = transactionFactory.Create(createTransactionRequest);

            // Assert
            result.Value.UserId.Should().Be(Ulid.Parse(createTransactionRequest.User.Id));
            result.Value.Description.Should().Be(transactionDetails.Description);
            result.Value.Category.Should().Be(transactionDetails.Category);
            result.Value.Money.Should().Be(transactionDetails.Money);
            result.Value.OccurredOn.Should().Be(transactionDetails.OccurredOn);
            result.Value.TransactionType.Should().Be(transactionDetails.TransactionType);
        }

        private static ITransactionDetails CreateTransactionDetails(CreateTransactionRequest createTransactionRequest) =>
            new TransactionDetails
            {
                Description = Description.Create(createTransactionRequest.Description).Value,
                Category = Category.FromValue(createTransactionRequest.Category).Value,
                Money = new Money(createTransactionRequest.Amount, Currency.FromValue(createTransactionRequest.Currency).Value),
                OccurredOn = createTransactionRequest.OccurredOn,
                TransactionType = TransactionType.FromValue(createTransactionRequest.TransactionType).Value
            };
    }
}
