using System.Collections.Generic;
using Expensely.Application.Contracts.TimeZones;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.TimeZones
{
    /// <summary>
    /// Represents the query for getting the collection of supported time zones.
    /// </summary>
    public sealed class GetTimeZonesQuery : IQuery<IEnumerable<TimeZoneResponse>>
    {
    }
}
