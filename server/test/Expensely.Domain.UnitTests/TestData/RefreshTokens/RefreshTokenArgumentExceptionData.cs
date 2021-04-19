using System;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.RefreshTokens
{
    public class RefreshTokenArgumentExceptionData : TheoryData<string, DateTime, string>
    {
        public RefreshTokenArgumentExceptionData()
        {
            Add(null, default, "token");

            Add(string.Empty, default, "token");

            Add("token", default, "expiresOnUtc");
        }
    }
}
