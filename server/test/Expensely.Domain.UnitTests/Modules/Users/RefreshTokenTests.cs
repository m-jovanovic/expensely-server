using System;
using Expensely.Domain.Modules.Users;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Users
{
    public class RefreshTokenTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ShouldThrowArgumentException_WhenTokenIsNullOrEmpty(string token) =>
            FluentActions.Invoking(
                    () => new RefreshToken(token, default))
                .Should()
                .Throw<ArgumentException>()
                .And.ParamName.Should().Be("token");

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenExpiresOnUtcIsEmpty() =>
            FluentActions.Invoking(
                    () => new RefreshToken("token", default))
                .Should()
                .Throw<ArgumentException>()
                .And.ParamName.Should().Be("expiresOnUtc");

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
        public void IsExpired_ShouldReturnTrue_WhenProvidedDateTimeIsGreaterThanExpirationTime()
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
