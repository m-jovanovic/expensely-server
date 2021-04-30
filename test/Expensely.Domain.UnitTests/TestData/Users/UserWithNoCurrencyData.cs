using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.UnitTests.TestData.Currencies;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public class UserWithNoCurrencyData : TheoryData<User, Currency>
    {
        public UserWithNoCurrencyData() => Add(UserTestData.ValidUser, CurrencyTestData.DefaultCurrency);
    }
}
