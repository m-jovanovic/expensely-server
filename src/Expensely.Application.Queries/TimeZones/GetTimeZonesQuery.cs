using System.Collections.Generic;
using Expensely.Application.Contracts.TimeZone;
using Expensely.Common.Abstractions.Messaging;

namespace Expensely.Application.Queries.TimeZone
{
    /// <summary>
    /// Represents the query for getting the collection of supported time zones.
    /// </summary>
    public sealed class GetTimeZonesQuery : IQuery<IEnumerable<TimeZoneResponse>>
    {
    }
}
