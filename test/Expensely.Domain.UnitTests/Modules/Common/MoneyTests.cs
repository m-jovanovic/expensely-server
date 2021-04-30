using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.UnitTests.TestData.Currencies;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Common
{
    public class MoneyTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenCurrencyIsNull()
        {
            // Arrange
            Currency currency = null;

            // Act
            Action action = () => new Money(default, currency);

            // Assert
            FluentActions.Invoking(action).Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("currency");
        }

        [Fact]
        public void Format_ShouldProperlyFormatMoney()
        {
            // Arrange
            decimal amount = 15.997m;

            Currency currency = CurrencyTestData.DefaultCurrency;

            var money = new Money(amount, currency);

            // Act
            string formatted = money.Format();

            // Assert
            formatted.Should().Be(currency.Format(amount));
        }
    }
}
