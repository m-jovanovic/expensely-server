using Expensely.Domain.Modules.Users;
using Moq;

namespace Expensely.Domain.UnitTests.TestData.User
{
    public static class UserTestData
    {
        public static readonly FirstName FirstName = FirstName.Create("First").Value;

        public static readonly LastName LastName = LastName.Create("Last").Value;

        public static readonly Email Email = Email.Create("test@expensely.net").Value;

        public static readonly Domain.Modules.Users.Password Password = Domain.Modules.Users.Password.Create("123aA!").Value;

        public static Domain.Modules.Users.User ValidUser => Domain.Modules.Users.User.Create(FirstName, LastName, Email, Password, new Mock<IPasswordHasher>().Object);
    }
}
