using Expensely.Domain.Modules.Users.Contracts;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public class CreateUserWithRolesValidData : TheoryData<CreateUserRequest, string[]>
    {
        public CreateUserWithRolesValidData() =>
            Add(
                new CreateUserRequest(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password),
                new[] { "Role1", "Role2" });
    }
}
