﻿using System;
using Expensely.Application.Abstractions.Common;

namespace Expensely.Infrastructure.Common
{
    /// <summary>
    /// Represents the current machine date and time.
    /// </summary>
    internal sealed class MachineDateTime : IDateTime
    {
        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;
    }
}