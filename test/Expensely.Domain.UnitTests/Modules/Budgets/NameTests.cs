using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Budgets;
using Expensely.Domain.UnitTests.TestData.Names;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Budgets
{
    public class NameTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_ShouldReturnFailureResult_WhenNameIsNullOrEmpty(string name)
        {
            // Arrange
            // Act
            Result<Name> result = Name.Create(name);

            // Assert
            result.Error.Should().Be(DomainErrors.Name.NullOrEmpty);
        }

        [Fact]
        public void Create_ShouldReturnFailureResult_WhenDescriptionIsTooLong()
        {
            // Arrange
            string description = NameTestData.LongerThanAllowedName;

            // Act
            Result<Name> result = Name.Create(description);

            // Assert
            result.Error.Should().Be(DomainErrors.Name.LongerThanAllowed);
        }
    }
}
