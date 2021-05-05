namespace Expensely.Application.Contracts.TimeZone
{
    /// <summary>
    /// Represents the time zone response.
    /// </summary>
    public sealed record TimeZoneResponse(string Id, string Name)
    {
    }
}
