using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Users
{
    public class UserFactoryTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IRoleProvider> _roleProviderMock;

        public UserFactoryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _roleProviderMock = new Mock<IRoleProvider>();
        }

        public static TheoryData<string, string, string, string> CreateUserInvalidArguments => new()
        {
            { null, UserTestData.LastName, UserTestData.Email, UserTestData.Password },
            { string.Empty, UserTestData.LastName, UserTestData.Email, UserTestData.Password },
            { UserTestData.FirstName, null, UserTestData.Email, UserTestData.Password },
            { UserTestData.FirstName, string.Empty, UserTestData.Email, UserTestData.Password },
            { UserTestData.FirstName, UserTestData.LastName, null, UserTestData.Password },
            { UserTestData.FirstName, UserTestData.LastName, string.Empty, UserTestData.Password },
            { UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, null },
            { UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, string.Empty }
        };

        public static TheoryData<FirstName, LastName, Email, Password> CreateUserValidArguments => new()
        {
            { UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password }
        };

        public static TheoryData<FirstName, LastName, Email, Password, string[]> CreateUserValidArgumentsWithRoles => new()
        {
            { UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password, new[] { "Role1", "Role2" } }
        };

        [Theory]
        [MemberData(nameof(CreateUserInvalidArguments))]
        public async Task Create_ShouldReturnFailureResult_WhenArgumentsAreInvalid(
            string firstName,
            string lastName,
            string email,
            string password)
        {
            // Arrange
            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(firstName, lastName, email, password, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(CreateUserValidArguments))]
        public async Task Create_ShouldCallAnyWithEmailAsyncOnUserRepository_WithProvidedEmail(
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.Is<Email>(e => e == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            UserFactory userFactory = CreateFactory();

            // Act
            await userFactory.Create(firstName, lastName, email, password, default);

            // Assert
            _userRepositoryMock.Verify(
                x => x.AnyWithEmailAsync(
                    It.Is<Email>(e => e == email),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [MemberData(nameof(CreateUserValidArguments))]
        public async Task Create_ShouldReturnFailureResult_WhenEmailIsAlreadyInUse(
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.Is<Email>(e => e == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(firstName, lastName, email, password, default);

            // Assert
            result.Error.Should().Be(DomainErrors.User.EmailAlreadyInUse);
        }

        [Theory]
        [MemberData(nameof(CreateUserValidArguments))]
        public async Task Create_ShouldCallGetStandardRolesOnRoleProver_WhenEmailIsNotAlreadyInUse(
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.Is<Email>(e => e == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            UserFactory userFactory = CreateFactory();

            // Act
            await userFactory.Create(firstName, lastName, email, password, default);

            // Assert
            _roleProviderMock.Verify(x => x.GetStandardRoles(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(CreateUserValidArguments))]
        public async Task Create_ShouldCreateUser_WhenEmailIsNotAlreadyInUse(
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.Is<Email>(e => e == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            UserFactory userFactory = CreateFactory();

            // Act
            Result<User> result = await userFactory.Create(firstName, lastName, email, password, default);

            // Assert
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(CreateUserValidArgumentsWithRoles))]
        public async Task Create_ShouldCreateUserWithRolesReturnedByRoleProvider_WhenEmailIsNotAlreadyInUse(
            FirstName firstName,
            LastName lastName,
            Email email,
            Password password,
            string[] roles)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.Is<Email>(e => e == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _roleProviderMock.Setup(x => x.GetStandardRoles()).Returns(roles);

            UserFactory userFactory = CreateFactory();

            // Act
            Result<User> result = await userFactory.Create(firstName, lastName, email, password, default);

            // Assert
            result.Value.Roles.Should().Contain(roles);
        }

        private UserFactory CreateFactory()
            => new(_userRepositoryMock.Object, _passwordHasherMock.Object, _roleProviderMock.Object);
    }
}
