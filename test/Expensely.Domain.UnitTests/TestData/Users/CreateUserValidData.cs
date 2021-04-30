using Expensely.Domain.Modules.Users.Contracts;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public class CreateUserValidData : TheoryData<CreateUserRequest>
    {
        public CreateUserValidData() =>
            Add(new CreateUserRequest(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password));
    }
}
