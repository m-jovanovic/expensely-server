using System;
using Expensely.Domain.Abstractions.Result;
using Expensely.Domain.Core;
using Expensely.Domain.Errors;
using Expensely.Domain.Events.Users;
using Expensely.Domain.Services;
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
            var user = User.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                new Mock<IPasswordService>().Object);

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
            var user = User.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                new Mock<IPasswordService>().Object);

            user.Events.Should().ContainSingle().And.AllBeOfType<UserCreatedEvent>();
        }

        [Fact]
        public void GetFullName_should_return_concatenated_first_and_last_name()
        {
            User user = UserTestData.ValidUser;

            user.GetFullName().Should().Be($"{user.FirstName.Value} {user.LastName.Value}");
        }

        [Fact]
        public void HasCurrency_should_return_false_when_currency_does_not_exist()
        {
            User user = UserTestData.ValidUser;

            user.HasCurrency(CurrencyTestData.DefaultCurrency).Should().BeFalse();
        }

        [Fact]
        public void GetPrimaryCurrency_should_return_nothing_when_primary_currency_does_not_exist()
        {
            User user = UserTestData.ValidUser;

            user.GetPrimaryCurrency().HasNoValue.Should().BeTrue();
        }

        [Fact]
        public void AddCurrency_should_add_currency_if_it_does_not_exist()
        {
            User user = UserTestData.ValidUser;

            Result result = user.AddCurrency(CurrencyTestData.DefaultCurrency);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddCurrency_should_not_add_currency_if_it_exists()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            Result result = user.AddCurrency(CurrencyTestData.DefaultCurrency);

            result.Error.Should().Be(DomainErrors.User.CurrencyAlreadyExists);
        }

        [Fact]
        public void AddCurrency_should_change_primary_currency_if_it_is_the_only_currency()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.GetPrimaryCurrency().Value.Should().Be(CurrencyTestData.DefaultCurrency);
        }

        [Fact]
        public void AddCurrency_should_raise_user_currency_added_event_when_currency_is_added()
        {
            User user = UserTestData.ValidUser;

            user.ClearEvents();

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.Events.Should().ContainSingle().And.AllBeOfType<UserCurrencyAddedEvent>();
        }

        [Fact]
        public void RemoveCurrency_should_not_remove_currency_if_it_does_not_exist()
        {
            User user = UserTestData.ValidUser;

            Result result = user.RemoveCurrency(CurrencyTestData.DefaultCurrency);

            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Fact]
        public void RemoveCurrency_should_not_remove_currency_if_it_is_the_primary_currency()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            Result result = user.RemoveCurrency(CurrencyTestData.DefaultCurrency);

            result.Error.Should().Be(DomainErrors.User.RemovingPrimaryCurrency);
        }

        [Fact]
        public void RemoveCurrency_should_remove_currency_if_it_exists_and_it_is_not_the_primary_currency()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            Result result = user.RemoveCurrency(CurrencyTestData.AuxiliaryCurrency);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void RemoveCurrency_should_raise_use_currency_removed_event_when_currency_is_removed()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.ClearEvents();

            user.RemoveCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.Events.Should().ContainSingle().And.AllBeOfType<UserCurrencyRemovedEvent>();
        }

        [Fact]
        public void ChangePrimaryCurrency_should_not_change_primary_currency_if_the_currency_was_not_added()
        {
            User user = UserTestData.ValidUser;

            Result result = user.ChangePrimaryCurrency(CurrencyTestData.DefaultCurrency);

            result.Error.Should().Be(DomainErrors.User.CurrencyDoesNotExist);
        }

        [Fact]
        public void ChangePrimaryCurrency_should_not_change_primary_currency_if_it_is_identical()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            Result result = user.ChangePrimaryCurrency(CurrencyTestData.DefaultCurrency);

            result.Error.Should().Be(DomainErrors.User.PrimaryCurrencyIsIdentical);
        }

        [Fact]
        public void ChangePrimaryCurrency_should_change_primary_currency_if_it_was_previously_added_and_is_different()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            Result result = user.ChangePrimaryCurrency(CurrencyTestData.AuxiliaryCurrency);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void GetPrimaryCurrency_should_return_the_new_primary_currency_after_it_has_been_changed()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.GetPrimaryCurrency().Value.Should().Be(CurrencyTestData.DefaultCurrency);
        }

        [Fact]
        public void ChangePrimaryCurrency_should_raise_user_primary_currency_changed_event_when_primary_currency_is_changed()
        {
            User user = UserTestData.ValidUser;

            user.AddCurrency(CurrencyTestData.DefaultCurrency);

            user.AddCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.ClearEvents();

            user.ChangePrimaryCurrency(CurrencyTestData.AuxiliaryCurrency);

            user.Events.Should().ContainSingle().And.AllBeOfType<UserPrimaryCurrencyChangedEvent>();
        }

        [Fact]
        public void VerifyPassword_should_call_hashes_match_on_password_service()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(true);

            User user = UserTestData.ValidUser;

            user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            passwordServiceMock.Verify(x => x.HashesMatch(It.Is<Password>(p => p == UserTestData.Password), It.IsAny<string>()));

            passwordServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void VerifyPassword_return_true_if_password_hashes_match()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(true);

            User user = UserTestData.ValidUser;

            bool result = user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_should_return_false_if_password_hashes_do_not_match()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            bool result = user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_should_raise_user_password_verification_failed_event_when_password_hashes_do_not_match()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            passwordServiceMock.Setup(x => x.HashesMatch(It.IsAny<Password>(), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            user.ClearEvents();

            user.VerifyPassword(UserTestData.Password, passwordServiceMock.Object);

            user.Events.Should().ContainSingle().And.AllBeOfType<UserPasswordVerificationFailedEvent>();
        }

        [Fact]
        public void ChangePassword_should_not_change_password_when_current_password_verification_fails()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            Result result = user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            result.Error.Should().Be(DomainErrors.User.InvalidEmailOrPassword);
        }

        [Fact]
        public void ChangePassword_should_not_change_password_if_new_password_is_identical()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(true);

            User user = UserTestData.ValidUser;

            Result result = user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            result.Error.Should().Be(DomainErrors.User.PasswordIsIdentical);
        }

        [Fact]
        public void ChangePassword_should_change_password_if_current_password_is_verified_and_new_password_is_different()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            Result result = user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void ChangePassword_should_call_hash_password_on_password_service_when_password_is_changed()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            passwordServiceMock.Verify(x => x.Hash(It.Is<Password>(p => p == newPassword)));
        }

        [Fact]
        public void ChangePassword_should_raise_user_password_changed_event_when_password_is_changed()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            Password currentPassword = UserTestData.Password;

            Password newPassword = Password.Create("123aA!!").Value;

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == currentPassword), It.IsAny<string>())).Returns(true);

            passwordServiceMock.Setup(x => x.HashesMatch(It.Is<Password>(p => p == newPassword), It.IsAny<string>())).Returns(false);

            User user = UserTestData.ValidUser;

            user.ClearEvents();

            user.ChangePassword(currentPassword, newPassword, passwordServiceMock.Object);

            user.Events.Should().ContainSingle().And.AllBeOfType<UserPasswordChangedEvent>();
        }

        [Fact]
        public void ChangeRefreshToken_should_change_refresh_token()
        {
            User user = UserTestData.ValidUser;

            var newRefreshToken = new RefreshToken("token", DateTime.UtcNow);

            user.ChangeRefreshToken(newRefreshToken);

            user.RefreshToken.Should().Be(newRefreshToken);
        }
    }
}
