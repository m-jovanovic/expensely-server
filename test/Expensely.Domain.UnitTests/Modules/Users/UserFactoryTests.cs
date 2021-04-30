using System.Threading;
using System.Threading.Tasks;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Modules.Users.Contracts;
using Expensely.Domain.UnitTests.TestData.Users;
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

        [Theory]
        [ClassData(typeof(CreateUserInvalidData))]
        public async Task CreateAsync_ShouldReturnFailureResult_WhenArgumentsAreInvalid(CreateUserRequest createUserRequest)
        {
            // Arrange
            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.CreateAsync(createUserRequest, default);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(CreateUserValidData))]
        public async Task CreateAsync_ShouldCallAnyWithEmailAsyncOnUserRepository_WithProvidedEmail(CreateUserRequest createUserRequest)
        {
            // Arrange
            _userRepositoryMock.Setup(
                    x => x.AnyWithEmailAsync(It.Is<Email>(e => e == createUserRequest.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            UserFactory userFactory = CreateFactory();

            // Act
            await userFactory.CreateAsync(createUserRequest, default);

            // Assert
            _userRepositoryMock.Verify(
                x => x.AnyWithEmailAsync(
                    It.Is<Email>(e => e == createUserRequest.Email),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [ClassData(typeof(CreateUserValidData))]
        public async Task CreateAsync_ShouldReturnFailureResult_WhenEmailIsAlreadyInUse(CreateUserRequest createUserRequest)
        {
            // Arrange
            _userRepositoryMock.Setup(
                    x => x.AnyWithEmailAsync(It.Is<Email>(e => e == createUserRequest.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            UserFactory userFactory = CreateFactory();

            // Act
            Result result = await userFactory.CreateAsync(createUserRequest, default);

            // Assert
            result.Error.Should().Be(DomainErrors.User.EmailAlreadyInUse);
        }

        [Theory]
        [ClassData(typeof(CreateUserValidData))]
        public async Task CreateAsync_ShouldCallGetStandardRolesOnRoleProver_WhenEmailIsNotAlreadyInUse(CreateUserRequest createUserRequest)
        {
            // Arrange
            _userRepositoryMock.Setup(
                    x => x.AnyWithEmailAsync(It.Is<Email>(e => e == createUserRequest.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            UserFactory userFactory = CreateFactory();

            // Act
            await userFactory.CreateAsync(createUserRequest, default);

            // Assert
            _roleProviderMock.Verify(x => x.GetStandardRoles(), Times.Once);
        }

        [Theory]
        [ClassData(typeof(CreateUserValidData))]
        public async Task CreateAsync_ShouldCreateUser_WhenEmailIsNotAlreadyInUse(CreateUserRequest createUserRequest)
        {
            // Arrange
            _userRepositoryMock.Setup(
                    x => x.AnyWithEmailAsync(It.Is<Email>(e => e == createUserRequest.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            UserFactory userFactory = CreateFactory();

            // Act
            Result<User> result = await userFactory.CreateAsync(createUserRequest, default);

            // Assert
            result.Value.Should().NotBeNull();
        }

        [Theory]
        [ClassData(typeof(CreateUserWithRolesValidData))]
        public async Task CreateAsync_ShouldCreateUserWithRolesReturnedByRoleProvider_WhenEmailIsNotAlreadyInUse(
            CreateUserRequest createUserRequest,
            string[] roles)
        {
            // Arrange
            _userRepositoryMock.Setup(
                    x => x.AnyWithEmailAsync(It.Is<Email>(e => e == createUserRequest.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _roleProviderMock.Setup(x => x.GetStandardRoles()).Returns(roles);

            UserFactory userFactory = CreateFactory();

            // Act
            Result<User> result = await userFactory.CreateAsync(createUserRequest, default);

            // Assert
            result.Value.Roles.Should().Contain(roles);
        }

        private UserFactory CreateFactory() => new(_userRepositoryMock.Object, _passwordHasherMock.Object, _roleProviderMock.Object);
    }
}
