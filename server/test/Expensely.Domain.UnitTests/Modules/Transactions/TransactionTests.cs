using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData;
using Expensely.Domain.UnitTests.TestData.Transactions;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Transactions
{
    public class TransactionTests
    {
        [Theory]
        [ClassData(typeof(CreateTransactionArgumentNullExceptionArguments))]
        public void Create_ShouldThrowArgumentNullException_WhenArgumentsAreInvalid(
            User user,
            ITransactionDetails transactionDetails,
            string paramName) =>
            FluentActions.Invoking(
                    () => Transaction.Create(user, transactionDetails))
                .Should()
                .Throw<ArgumentNullException>()
                .And
                .ParamName.Should().Be(paramName);

        [Theory]
        [ClassData(typeof(CreateTransactionArgumentExceptionArguments))]
        public void Create_ShouldThrowArgumentException_WhenArgumentsAreInvalid(
            User user,
            ITransactionDetails transactionDetails,
            string paramName) =>
            FluentActions.Invoking(
                    () => Transaction.Create(user, transactionDetails))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ParamName.Should().Be(paramName);

        [Theory]
        [ClassData(typeof(CreateTransactionValidArguments))]
        public void Create_ShouldCreateTransaction_WithProperValues(User user, ITransactionDetails transactionDetails)
        {
            // Arrange
            // Act
            var transaction = Transaction.Create(user, transactionDetails);

            // Assert
            transaction.UserId.Should().Be(Ulid.Parse(user.Id));
            transaction.Description.Should().Be(transactionDetails.Description);
            transaction.Category.Should().Be(transactionDetails.Category);
            transaction.Money.Should().Be(transactionDetails.Money);
            transaction.OccurredOn.Should().Be(transactionDetails.OccurredOn);
            transaction.TransactionType.Should().Be(transactionDetails.TransactionType);
            transaction.CreatedOnUtc.Should().Be(default);
            transaction.ModifiedOnUtc.Should().BeNull();
        }

        [Theory]
        [ClassData(typeof(CreateTransactionValidArguments))]
        public void ChangeDetails_ShouldChangeTransactionDetails_WhenArgumentsAreValid(User user, ITransactionDetails transactionDetails)
        {
            // Arrange
            var transaction = Transaction.Create(user, transactionDetails);

            decimal amount = transactionDetails.TransactionType == TransactionType.Expense ? -1 : 1;

            var newTransactionDetails = new TransactionDetails
            {
                Description = Description.Create("New description").Value,
                Category = Category.Bills,
                Money = new Money(amount, CurrencyTestData.DefaultCurrency),
                OccurredOn = DateTime.UtcNow.AddDays(5).Date
            };

            // Act
            transaction.ChangeDetails(newTransactionDetails);

            // Assert
            transaction.Description.Should().Be(newTransactionDetails.Description);
            transaction.Category.Should().Be(newTransactionDetails.Category);
            transaction.Money.Should().Be(newTransactionDetails.Money);
            transaction.OccurredOn.Should().Be(newTransactionDetails.OccurredOn);
        }
    }
}
