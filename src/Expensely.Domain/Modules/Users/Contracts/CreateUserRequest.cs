namespace Expensely.Domain.Modules.Users.Contracts
{
    /// <summary>
    /// Represents the request for creating a new user.
    /// </summary>
    public sealed record CreateUserRequest(string FirstName, string LastName, string Email, string Password);
}
