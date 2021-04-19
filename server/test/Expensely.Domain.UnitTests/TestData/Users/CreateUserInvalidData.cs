using Expensely.Domain.Modules.Users.Contracts;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public sealed class CreateUserInvalidData : TheoryData<CreateUserRequest>
    {
        public CreateUserInvalidData()
        {
            Add(new CreateUserRequest(null, UserTestData.LastName, UserTestData.Email, UserTestData.Password));

            Add(new CreateUserRequest(string.Empty, UserTestData.LastName, UserTestData.Email, UserTestData.Password));

            Add(new CreateUserRequest(UserTestData.FirstName, null, UserTestData.Email, UserTestData.Password));

            Add(new CreateUserRequest(UserTestData.FirstName, string.Empty, UserTestData.Email, UserTestData.Password));

            Add(new CreateUserRequest(UserTestData.FirstName, UserTestData.LastName, null, UserTestData.Password));

            Add(new CreateUserRequest(UserTestData.FirstName, UserTestData.LastName, string.Empty, UserTestData.Password));

            Add(new CreateUserRequest(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, null));

            Add(new CreateUserRequest(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, string.Empty));
        }
    }
}
