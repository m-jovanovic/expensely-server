using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Users
{
    public sealed class CreateUserInvalidData : TheoryData<string, string, string, string>
    {
        public CreateUserInvalidData()
        {
            Add(null, UserTestData.LastName, UserTestData.Email, UserTestData.Password);

            Add(string.Empty, UserTestData.LastName, UserTestData.Email, UserTestData.Password);

            Add(UserTestData.FirstName, null, UserTestData.Email, UserTestData.Password);

            Add(UserTestData.FirstName, string.Empty, UserTestData.Email, UserTestData.Password);

            Add(UserTestData.FirstName, UserTestData.LastName, null, UserTestData.Password);

            Add(UserTestData.FirstName, UserTestData.LastName, string.Empty, UserTestData.Password);

            Add(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, null);

            Add(UserTestData.FirstName, UserTestData.LastName, UserTestData.Email, string.Empty);
        }
    }
}
