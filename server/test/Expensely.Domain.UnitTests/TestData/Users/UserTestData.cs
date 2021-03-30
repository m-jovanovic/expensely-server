using Expensely.Domain.Modules.Users;
using Moq;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public static class UserTestData
    {
        public static readonly FirstName FirstName = FirstName.Create("First").Value;

        public static readonly LastName LastName = LastName.Create("Last").Value;

        public static readonly Email Email = Email.Create("test@expensely.net").Value;

        public static readonly Password Password = Password.Create("123aA!").Value;

        public static User ValidUser => User.Create(FirstName, LastName, Email, Password, new Mock<IPasswordHasher>().Object);
    }
}
