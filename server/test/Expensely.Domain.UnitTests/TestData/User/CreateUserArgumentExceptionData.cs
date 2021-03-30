using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.User
{
    public class CreateUserArgumentExceptionData : TheoryData<FirstName, LastName, Email, Domain.Modules.Users.Password, string>
    {
        public CreateUserArgumentExceptionData()
        {
            Add(null, UserTestData.LastName, UserTestData.Email, UserTestData.Password, "firstName");

            Add(UserTestData.FirstName, null, UserTestData.Email, UserTestData.Password, "lastName");

            Add(UserTestData.FirstName, UserTestData.LastName, null, UserTestData.Password, "email");

            Add(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, null, "password");
        }
    }
}
