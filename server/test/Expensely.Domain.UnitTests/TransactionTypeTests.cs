using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests
{
    public class TransactionTypeTests
    {
        [Fact]
        public void Should_return_success_for_expense_and_amount_less_than_zero()
        {
            TransactionType transactionType = TransactionType.Expense;

            Result result = transactionType.ValidateAmount(new Money(-15, Currency.Usd));

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
        }
    }
}