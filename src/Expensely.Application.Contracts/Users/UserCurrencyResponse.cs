namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the user currency response.
    /// </summary>
    public sealed record UserCurrencyResponse(int Id, string Name, string Code, bool IsPrimary);
}
