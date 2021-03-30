using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Password
{
    public class ChangePasswordData : TheoryData<Domain.Modules.Users.Password, Domain.Modules.Users.Password>
    {
        public ChangePasswordData() => Add(Domain.Modules.Users.Password.Create("123aA!").Value, Domain.Modules.Users.Password.Create("123aA!!").Value);
    }
}
