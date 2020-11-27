﻿using System;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Primitives.Result;

namespace Expensely.Application.Users.Commands.ChangeUserPrimaryCurrency
{
    /// <summary>
    /// Represents the command for changing a user's primary currency.
    /// </summary>
    public sealed class ChangeUserPrimaryCurrencyCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserPrimaryCurrencyCommand"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="currency">The currency value.</param>
        public ChangeUserPrimaryCurrencyCommand(Guid userId, int currency)
        {
            UserId = userId;
            Currency = currency;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the currency value.
        /// </summary>
        public int Currency { get; }
    }
}