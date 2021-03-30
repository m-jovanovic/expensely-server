using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public class CreateUserValidData : TheoryData<FirstName, LastName, Email, Password>
    {
        public CreateUserValidData() => Add(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password);
    }
}
