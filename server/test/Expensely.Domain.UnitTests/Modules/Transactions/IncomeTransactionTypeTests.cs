using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.UnitTests.TestData.Currencies;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Transactions
{
    public class IncomeTransactionTypeTests
    {
        [Theory]
        [InlineData(2, "Income")]
        public void ShouldHaveSpecifiedValueAndName(int value, string name)
        {
            // Arrange
            // Act
            TransactionType transactionType = TransactionType.Income;

            // Assert
            transactionType.Value.Should().Be(value);
            transactionType.Name.Should().Be(name);
        }

        [Theory]
        [InlineData(1)]
        public void ValidateAmount_ShouldReturnSuccessResult_WhenAmountIsGreaterThanZero(decimal amount)
        {
            // Arrange
            TransactionType transactionType = TransactionType.Income;

            // Act
            Result result = transactionType.ValidateAmount(new Money(amount, CurrencyTestData.DefaultCurrency));

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1.0)]
        public void ValidateAmount_ShouldReturnFailureResult_WhenAmountIsLessThanOrEqualToZero(decimal amount)
        {
            // Arrange
            TransactionType transactionType = TransactionType.Income;

            // Act
            Result result = transactionType.ValidateAmount(new Money(amount, CurrencyTestData.DefaultCurrency));

            // Assert
            result.Error.Should().Be(DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }
    }
}
