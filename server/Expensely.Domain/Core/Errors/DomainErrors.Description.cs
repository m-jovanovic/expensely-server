﻿using Expensely.Domain.Primitives;

namespace Expensely.Domain.Core.Errors
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static partial class DomainErrors
    {
        /// <summary>
        /// Contains the description errors.
        /// </summary>
        public static class Description
        {
            /// <summary>
            /// Gets the description is longer than allowed error.
            /// </summary>
            public static Error LongerThanAllowed => new Error("Description.LongerThanAllowed", "The description is longer than allowed.");
        }
    }
}