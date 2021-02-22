using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.UnitTests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Core
{
    public class IncomeTransactionTypeTests
    {
        [Fact]
        public void Should_have_proper_values()
        {
            // Arrange
            // Act
            TransactionType transactionType = TransactionType.Income;

            // Assert
            transactionType.Value.Should().Be(2);
            transactionType.Name.Should().Be("Income");
        }

        [Theory]
        [InlineData(1)]
        public void ValidateAmount_should_return_true_for_amount_greater_than_zero(decimal amount)
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
        public void ValidateAmount_should_return_false_for_amount_less_than_or_equal_to_zero(decimal amount)
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
