using Expensely.Application.Commands.Users;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using FluentAssertions;
using Xunit;

namespace Expensely.Application.UnitTests
{
    public class TransactionTypeTests
    {
        [Fact]
        public void Should_return_failure_for_expense_and_amount_equal_to_zero()
        {
            TransactionType transactionType = TransactionType.Income;

            Result result = transactionType.ValidateAmount(new Money(0, Currency.Usd));

            result.Error.Should().Be(DomainErrors.Transaction.IncomeAmountLessThanOrEqualToZero);
        }

        [Fact]
        public void Should_create_command()
        {
            var command = new CreateUserCommand(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            command.Should().NotBeNull();
        }
    }
}
