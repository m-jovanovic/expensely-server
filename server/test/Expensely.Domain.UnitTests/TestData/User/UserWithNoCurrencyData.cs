using Expensely.Domain.UnitTests.TestData.Currency;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.User
{
    public class UserWithNoCurrencyData : TheoryData<Domain.Modules.Users.User, Domain.Modules.Common.Currency>
    {
        public UserWithNoCurrencyData() => Add(UserTestData.ValidUser, CurrencyTestData.DefaultCurrency);
    }
}
