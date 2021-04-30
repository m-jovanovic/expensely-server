using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.UnitTests.TestData.Descriptions;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Modules.Transactions
{
    public class DescriptionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_ShouldReturnSuccessResultWithEmptyDescription_WhenDescriptionIsNullOrEmpty(string description)
        {
            // Arrange
            // Act
            Result<Description> result = Description.Create(description);

            // Assert
            result.Value.Value.Should().BeEmpty();
        }

        [Fact]
        public void Create_ShouldReturnFailureResult_WhenDescriptionIsTooLong()
        {
            // Arrange
            string description = DescriptionTestData.LongerThanAllowedDescription;

            // Act
            Result<Description> result = Description.Create(description);

            // Assert
            result.Error.Should().Be(DomainErrors.Description.LongerThanAllowed);
        }
    }
}
