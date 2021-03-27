using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.Infrastructure;
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

        public static TheoryData<string[]> RolesData => new()
        {
            new[] { "Role1", "Role2", "Role3" }
        };

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldReturnFailureResult_WhenFirstNameIsNullOrEmpty(string password)
        {
            // Arrange
            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(password, UserTestData.LastName, UserTestData.Email, UserTestData.Password, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldReturnFailureResult_WhenLastNameIsNullOrEmpty(string lastName)
        {
            // Arrange
            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(UserTestData.FirstName, lastName, UserTestData.Email, UserTestData.Password, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldReturnFailureResult_WhenEmailIsNullOrEmpty(string email)
        {
            // Arrange
            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(UserTestData.FirstName, UserTestData.LastName, email, UserTestData.Password, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldReturnFailureResult_WhenPasswordIsNullOrEmpty(string password)
        {
            // Arrange
            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, password, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Create_ShouldCallAnyWithEmailAsyncOnUserRepository_WithProvidedEmail()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            UserFactory userFactory = CreateFactory();

            // Act
            await userFactory.Create(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password, default);

            // Assert
            _userRepositoryMock.Verify(
                x => x.AnyWithEmailAsync(
                    It.Is<Email>(email => email == UserTestData.Email),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnFailureResult_WhenEmailIsAlreadyInUse()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                default);

            // Assert
            result.Error.Should().Be(DomainErrors.User.EmailAlreadyInUse);
        }

        [Fact]
        public async Task Create_ShouldCallGetStandardRolesOnRoleProver_WhenEmailIsNotAlreadyInUse()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            UserFactory userFactory = CreateFactory();

            // Act
            await userFactory.Create(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password, default);

            // Assert
            _roleProviderMock.Verify(x => x.GetStandardRoles(), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldCreateUser_WhenEmailIsNotAlreadyInUse()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            UserFactory userFactory = CreateFactory();

            // Act
            Result<User> result = await userFactory.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                default);

            // Assert
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(RolesData))]
        public async Task Create_ShouldCreateUserWithRolesReturnedByRoleProvider_WhenEmailIsNotAlreadyInUse(string[] roles)
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.AnyWithEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            _roleProviderMock.Setup(x => x.GetStandardRoles()).Returns(roles);

            UserFactory userFactory = CreateFactory();

            // Act
            Result<User> result = await userFactory.Create(
                UserTestData.FirstName,
                UserTestData.LastName,
                UserTestData.Email,
                UserTestData.Password,
                default);

            // Assert
            result.Value.Roles.Should().Contain(roles);
        }

        private UserFactory CreateFactory()
            => new(_userRepositoryMock.Object, _passwordHasherMock.Object, _roleProviderMock.Object);
    }
}
