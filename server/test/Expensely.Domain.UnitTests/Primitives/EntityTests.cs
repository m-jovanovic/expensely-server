using System;
using Expensely.Domain.Primitives;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Expensely.Domain.UnitTests.TestData.Transactions;
using Expensely.Domain.UnitTests.TestData.Users;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Primitives
{
    public class EntityTests
    {
        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherEntityIsNull()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            bool result = entity.Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherEntityIsDifferentType()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            bool result = entity.Equals(TransactionTestData.ValidExpense);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenOtherEntityIsTheSameEntity()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            bool result = entity.Equals(entity);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherObjectIsNull()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            bool result = entity.Equals((object)null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenOtherObjectIsTheSameEntity()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            bool result = entity.Equals((object)entity);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherObjectIsNotAnEntity()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            bool result = entity.Equals(CurrencyTestData.DefaultCurrency);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_ShouldReturnProperHashCode()
        {
            // Arrange
            Entity entity = UserTestData.ValidUser;

            // Act
            int hashcode = entity.GetHashCode();

            // Assert
            hashcode.Should().Be(entity.Id.GetHashCode(StringComparison.InvariantCulture) * 41);
        }

        [Fact]
        public void EqualityOperator_ShouldReturnTrue_WhenBothEntitiesAreNull()
        {
            // Arrange
            Entity entity1 = null;
            Entity entity2 = null;

            // Act
            bool result = entity1 == entity2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void EqualityOperator_ShouldReturnFalse_WhenFirstEntityIsNull()
        {
            // Arrange
            Entity entity1 = UserTestData.ValidUser;
            Entity entity2 = null;

            // Act
            bool result = entity1 == entity2;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void EqualityOperator_ShouldReturnFalse_WhenSecondEntityIsNull()
        {
            // Arrange
            Entity entity1 = null;
            Entity entity2 = UserTestData.ValidUser;

            // Act
            bool result = entity1 == entity2;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void EqualityOperator_ShouldReturnTrue_WhenEntitiesAreEqual()
        {
            // Arrange
            Entity entity1 = UserTestData.ValidUser;
            Entity entity2 = entity1;

            // Act
            bool result = entity1 == entity2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void UnEqualityOperator_ShouldReturnTrue_WhenEntitiesAreNotEqual()
        {
            // Arrange
            Entity entity1 = UserTestData.ValidUser;
            Entity entity2 = UserTestData.ValidUser;

            // Act
            bool result = entity1 != entity2;

            // Assert
            result.Should().BeTrue();
        }
    }
}
