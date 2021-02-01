using Expensely.Domain.Core;
using Expensely.Domain.Services;
using Moq;

namespace Expensely.Domain.UnitTests.Infrastructure
{
    public static class UserTestData
    {
        public static readonly FirstName FirstName = FirstName.Create("First").Value;

        public static readonly LastName LastName = LastName.Create("Last").Value;

        public static readonly Email Email = Email.Create("test@expensely.net").Value;

        public static readonly Password Password = Password.Create("123aA!").Value;

        public static readonly Currency DefaultCurrency = Currency.Usd;

        public static readonly Currency AuxiliaryCurrency = Currency.Eur;

        public static User ValidUser => User.Create(FirstName, LastName, Email, Password, new Mock<IPasswordService>().Object);

        public static User GetValidUserWith(Mock<IPasswordService> passwordServiceMock) =>
            User.Create(FirstName, LastName, Email, Password, passwordServiceMock.Object);
    }
}
