using System;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.RefreshTokens;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Users
{
    public class RefreshTokenTests
    {
        [Theory]
        [ClassData(typeof(RefreshTokenArgumentExceptionData))]
        public void Constructor_ShouldThrowArgumentException_WhenParametersAreInvalid(
            string token,
            DateTime expiresOnUtc,
            string paramName)
        {
            // Arrange
            // Act
            Action action = () => new RefreshToken(token, expiresOnUtc);

            // Assert
            FluentActions.Invoking(action).Should().Throw<ArgumentException>().And.ParamName.Should().Be(paramName);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenRefreshTokenValuesAreTheSame()
        {
            // Arrange
            DateTime expiresOnUtc = DateTime.UtcNow;
            var refreshToken1 = new RefreshToken("token", expiresOnUtc);
            var refreshToken2 = new RefreshToken("token", expiresOnUtc);

            // Act
            bool equals = refreshToken1 == refreshToken2;

            // Assert
            equals.Should().BeTrue();
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenProvidedDateTimeIsLessThanExpirationDateTime()
        {
            // Arrange
            DateTime utcNow = DateTime.UtcNow;
            DateTime expiresOnUtc = utcNow.AddMinutes(1);
            var refreshToken = new RefreshToken("token", expiresOnUtc);

            // Act
            bool isExpired = refreshToken.IsExpired(utcNow);

            // Assert
            isExpired.Should().BeFalse();
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenProvidedDateTimeIsEqualToExpirationDateTime()
        {
            // Arrange
            DateTime utcNow = DateTime.UtcNow;
            DateTime expiresOnUtc = utcNow;
            var refreshToken = new RefreshToken("token", expiresOnUtc);

            // Act
            bool isExpired = refreshToken.IsExpired(utcNow);

            // Assert
            isExpired.Should().BeFalse();
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenProvidedDateTimeIsGreaterThanExpirationDateTime()
        {
            // Arrange
            DateTime utcNow = DateTime.UtcNow;
            DateTime expiresOnUtc = utcNow.AddMinutes(-1);
            var refreshToken = new RefreshToken("token", expiresOnUtc);

            // Act
            bool isExpired = refreshToken.IsExpired(utcNow);

            // Assert
            isExpired.Should().BeTrue();
        }
    }
}
