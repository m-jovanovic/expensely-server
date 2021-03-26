using System;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Modules.Users.Events;
using Expensely.Domain.UnitTests.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Users
{
    public class UserTests
    {
        [Fact]
        public void Create_ShouldCreateUser_WithProperValues()
        {
            // Arrange
            // Act
            var user = User.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                new Mock<IPasswordService>().Object);

            // Assert
            user.Should().NotBeNull();
            user.FirstName.Should().Be(UserTestData.FirstName);
            user.LastName.Should().Be(UserTestData.LastName);
            user.Email.Should().Be(UserTestData.Email);
            user.RefreshToken.Should().BeNull();
            user.CreatedOnUtc.Should().Be(default);
            user.ModifiedOnUtc.Should().BeNull();
        }

        [Fact]
        public void Create_ShouldCreateUser_WithUserCreatedEvent()
        {
            // Arrange
            // Act
            var user = User.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                new Mock<IPasswordService>().Object);

            // Assert
            user.GetEvents().Should().AllBeOfType<UserCreatedEvent>();
        }

        [Fact]
        public void GetFullName_ShouldReturnConcatenatedFirstAndLastName()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            string fullName = user.GetFullName();

            // Assert
            fullName.Should().Be($"{user.FirstName.Value} {user.LastName.Value}");
        }

        [Fact]
        public void HasCurrency_ShouldReturnFalse_WhenCurrencyIsNotUserCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            bool hasCurrency = user.HasCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            hasCurrency.Should().BeFalse();
        }

        [Fact]
        public void PrimaryCurrency_ShouldReturnNull_WhenPrimaryCurrencyIsNotSet()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Currency primaryCurrency = user.PrimaryCurrency;

            // Assert
            primaryCurrency.Should().BeNull();
        }

        [Fact]
        public void AddCurrency_ShouldAddCurrency_WhenCurrencyIsNotUserCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Result result = user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddCurrency_ShouldNotAddCurrency_WhenCurrencyIsAlreadyUserCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Act
            Result result = user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.CurrencyAlreadyExists);
        }

        [Fact]
        public void AddCurrency_ShouldChangePrimaryCurrency_WhenPrimaryCurrencyIsNull()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            user.PrimaryCurrency.Should().Be(CurrencyTestData.DefaultCurrency);
        }

        [Fact]
        public void AddCurrency_ShouldRaiseUserCurrencyAddedEvent_WhenCurrencyIsAdded()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.ClearEvents();

            // Act
            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            user.GetEvents().Should().AllBeOfType<UserCurrencyAddedEvent>();
        }

        [Fact]
        public void RemoveCurrency_ShouldNotRemoveCurrency_WhenCurrencyIsNotUserCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Result result = user.RemoveCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Fact]
        public void RemoveCurrency_ShouldNotRemoveCurrency_WhenCurrencyIsPrimaryCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Act
            Result result = user.RemoveCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.RemovingPrimaryCurrency);
        }

        [Fact]
        public void RemoveCurrency_ShouldRemoveCurrency_WhenCurrencyIsUserCurrencyAndIsNotPrimaryCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Act
            Result result = user.RemoveCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void RemoveCurrency_ShouldRaiseUserCurrencyRemovedEvent_WhenCurrencyIsRemoved()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.ClearEvents();

            // Act
            user.RemoveCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Assert
            user.GetEvents().Should().AllBeOfType<UserCurrencyRemovedEvent>();
        }

        [Fact]
        public void ChangePrimaryCurrency_ShouldNotChangePrimaryCurrency_WhenCurrencyIsNotUserCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Result result = user.ChangePrimaryCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Fact]
        public void ChangePrimaryCurrency_ShouldNotChangePrimaryCurrency_WhenCurrencyAndPrimaryCurrencyAreIdentical()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Act
            Result result = user.ChangePrimaryCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.PrimaryCurrencyIsIdentical);
        }

        [Fact]
        public void ChangePrimaryCurrency_ShouldChangePrimaryCurrency_WhenCurrencyIsUserCurrencyAndDifferentThanPrimaryCurrency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Act
            Result result = user.ChangePrimaryCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void ChangePrimaryCurrency_ShouldRaiseUserPrimaryCurrencyChangedEvent_WhenPrimaryCurrencyIsChanged()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.ClearEvents();

            // Act
            user.ChangePrimaryCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Assert
            user.GetEvents().Should().AllBeOfType<UserPrimaryCurrencyChangedEvent>();
        }

        [Fact]
        public void VerifyPassword_ShouldCallHashesMatchOnPasswordService_WithProvidedPassword()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(true);

            User user = UserTestData.ValidUser;

            // Act
            user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            // Assert
            passwordServiceMock.Verify(x => x.HashesMatch(It.Is<Password>(p => p == UserTestData.Password), It.IsAny<string>()));

            passwordServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordHashesMatch()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(true);

            User user = UserTestData.ValidUser;

            // Act
            bool result = user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordHashesDoNotMatch()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            // Act
            bool result = user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_ShouldRaiseUserPasswordVerificationFailedEvent_WhenPasswordHashesDoNotMatch()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            user.ClearEvents();

            // Act
            user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            // Assert
            user.GetEvents().Should().AllBeOfType<UserPasswordVerificationFailedEvent>();
        }

        [Fact]
        public void ChangePassword_ShouldNotChangePassword_WhenPasswordHashesDoNotMatch()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            // Act
            Result result = user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            // Assert
            result.Error.Should().Be(DomainErrors.User.InvalidEmailOrPassword);
        }

        [Fact]
        public void ChangePassword_ShouldNotChangePassword_WhenPasswordIsIdentical()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(true);

            User user = UserTestData.ValidUser;

            // Act
            Result result = user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            // Assert
            result.Error.Should().Be(DomainErrors.User.PasswordIsIdentical);
        }

        [Fact]
        public void ChangePassword_ShouldChangePassword_WhenPasswordHashesMatchAndPasswordIsDifferent()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            // Act
            Result result = user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void ChangePassword_ShouldCallHashPasswordOnPasswordService_WhenPasswordIsChanged()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            // Act
            user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            // Assert
            passwordServiceMock.Verify(x => x.Hash(It.Is<Password>(p => p == newPassword)));
        }

        [Fact]
        public void ChangePassword_ShouldRaiseUserPasswordChangedEvent_WhenPasswordIsChanged()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            user.ClearEvents();

            // Act
            user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            // Assert
            user.GetEvents().Should().ContainSingle().And.AllBeOfType<UserPasswordChangedEvent>();
        }

        [Theory]
        [InlineData("User")]
        public void AddRole_ShouldAddRole(string role)
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            user.AddRole(role);

            // Assert
            user.Roles.Should().Contain(role);
        }

        [Fact]
        public void ChangeRefreshToken_ShouldChangeRefreshToken()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            var newRefreshToken = new RefreshToken("token", DateTime.UtcNow);

            // Act
            user.ChangeRefreshToken(newRefreshToken);

            // Assert
            user.RefreshToken.Should().Be(newRefreshToken);
        }
    }
}
