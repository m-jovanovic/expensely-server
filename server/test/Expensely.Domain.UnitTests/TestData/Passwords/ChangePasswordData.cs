using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Passwords
{
    public class ChangePasswordData : TheoryData<Password, Password>
    {
        public ChangePasswordData() => Add(Password.Create("123aA!").Value, Password.Create("123aA!!").Value);
    }
}
