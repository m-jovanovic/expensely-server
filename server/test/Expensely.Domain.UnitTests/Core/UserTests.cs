using System;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Authentication;
using Expensely.Domain.Modules.Shared;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Modules.Users.Events;
using Expensely.Domain.UnitTests.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Domain.UnitTests.Core
{
    public class UserTests
    {
        [Fact]
        public void Create_should_create_user_and_properly_set_values()
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
        public void Create_should_create_user_and_raise_user_created_event()
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
            user.GetEvents().Should().ContainSingle().And.AllBeOfType<UserCreatedEvent>();
        }

        [Fact]
        public void GetFullName_should_return_concatenated_first_and_last_name()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            string fullName = user.GetFullName();

            // Assert
            fullName.Should().Be($"{user.FirstName.Value} {user.LastName.Value}");
        }

        [Fact]
        public void HasCurrency_should_return_false_when_currency_does_not_exist()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            bool hasCurrency = user.HasCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            hasCurrency.Should().BeFalse();
        }

        [Fact]
        public void GetPrimaryCurrency_should_return_nothing_when_primary_currency_does_not_exist()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Currency primaryCurrency = user.PrimaryCurrency;

            // Assert
            primaryCurrency.Should().BeNull();
        }

        [Fact]
        public void AddCurrency_should_add_currency_if_it_does_not_exist()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Result result = user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddCurrency_should_not_add_currency_if_it_exists()
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
        public void AddCurrency_should_change_primary_currency_if_it_is_the_only_currency()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Act
            Currency primaryCurrency = user.PrimaryCurrency;

            // Assert
            primaryCurrency.Should().Be(CurrencyTestData.DefaultCurrency);
        }

        [Fact]
        public void AddCurrency_should_raise_user_currency_added_event_when_currency_is_added()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.ClearEvents();

            // Act
            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            user.GetEvents().Should().Contain(@event => @event is UserCurrencyAddedEvent);
        }

        [Fact]
        public void RemoveCurrency_should_not_remove_currency_if_it_does_not_exist()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Result result = user.RemoveCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Fact]
        public void RemoveCurrency_should_not_remove_currency_if_it_is_the_primary_currency()
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
        public void RemoveCurrency_should_remove_currency_if_it_exists_and_it_is_not_the_primary_currency()
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
        public void RemoveCurrency_should_raise_use_currency_removed_event_when_currency_is_removed()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.ClearEvents();

            // Act
            user.RemoveCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Assert
            user.GetEvents().Should().ContainSingle().And.AllBeOfType<UserCurrencyRemovedEvent>();
        }

        [Fact]
        public void ChangePrimaryCurrency_should_not_change_primary_currency_if_the_currency_was_not_added()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            Result result = user.ChangePrimaryCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Fact]
        public void ChangePrimaryCurrency_should_not_change_primary_currency_if_it_is_identical()
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
        public void ChangePrimaryCurrency_should_change_primary_currency_if_it_was_previously_added_and_is_different()
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
        public void GetPrimaryCurrency_should_return_the_new_primary_currency_after_it_has_been_changed()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            // Act
            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            // Assert
            user.PrimaryCurrency.Should().Be(CurrencyTestData.DefaultCurrency);
        }

        [Fact]
        public void ChangePrimaryCurrency_should_raise_user_primary_currency_changed_event_when_primary_currency_is_changed()
        {
            // Arrange
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.ClearEvents();

            // Act
            user.ChangePrimaryCurrency(CurrencyTestData.AuxiliaryCurrency);

            // Assert
            user.GetEvents().Should().ContainSingle().And.AllBeOfType<UserPrimaryCurrencyChangedEvent>();
        }

        [Fact]
        public void VerifyPassword_should_call_hashes_match_on_password_service()
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
        public void VerifyPassword_return_true_if_password_hashes_match()
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
        public void VerifyPassword_should_return_false_if_password_hashes_do_not_match()
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
        public void VerifyPassword_should_raise_user_password_verification_failed_event_when_password_hashes_do_not_match()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            user.ClearEvents();

            // Act
            user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            // Assert
            user.GetEvents().Should().ContainSingle().And.AllBeOfType<UserPasswordVerificationFailedEvent>();
        }

        [Fact]
        public void ChangePassword_should_not_change_password_when_current_password_verification_fails()
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
        public void ChangePassword_should_not_change_password_if_new_password_is_identical()
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
        public void ChangePassword_should_change_password_if_current_password_is_verified_and_new_password_is_different()
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
        public void ChangePassword_should_call_hash_password_on_password_service_when_password_is_changed()
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
        public void ChangePassword_should_raise_user_password_changed_event_when_password_is_changed()
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

        [Fact]
        public void ChangeRefreshToken_should_change_refresh_token()
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
