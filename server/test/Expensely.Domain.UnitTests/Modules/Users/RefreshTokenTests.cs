using System;
using Expensely.Domain.Modules.Users;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Users
{
    public class RefreshTokenTests
    {
        public static TheoryData<string, DateTime, string> RefreshTokenInvalidArguments => new()
        {
            { null, default, "token" },
            { string.Empty, default, "token" },
            { "token", default, "expiresOnUtc" }
        };

        [Theory]
        [MemberData(nameof(RefreshTokenInvalidArguments))]
        public void Constructor_ShouldThrowArgumentException_WhenParametersAreInvalid(
            string token,
            DateTime expiresOnUtc,
            string paramName) =>
            FluentActions.Invoking(
                    () => new RefreshToken(token, expiresOnUtc))
                .Should()
                .Throw<ArgumentException>()
                .And
                .ParamName.Should().Be(paramName);

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
