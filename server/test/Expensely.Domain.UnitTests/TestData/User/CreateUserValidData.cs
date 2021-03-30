using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.User
{
    public class CreateUserValidData : TheoryData<FirstName, LastName, Email, Domain.Modules.Users.Password>
    {
        public CreateUserValidData() => Add(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, UserTestData.Password);
    }
}
