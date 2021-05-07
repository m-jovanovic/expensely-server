namespace Expensely.Application.Contracts.TimeZones
{
    /// <summary>
    /// Represents the time zone response.
    /// </summary>
    public sealed record TimeZoneResponse(string Id, string Name)
    {
    }
}
