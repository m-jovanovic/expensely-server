using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.User
{
    public class CreateUserWithRolesValidData : TheoryData<FirstName, LastName, Email, Domain.Modules.Users.Password, string[]>
    {
        public CreateUserWithRolesValidData() =>
            Add(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password, new[] { "Role1", "Role2" });
    }
}
