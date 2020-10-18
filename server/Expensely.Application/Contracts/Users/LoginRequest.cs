﻿namespace Expensely.Application.Contracts.Users
{
    /// <summary>
    /// Represents the login request.
    /// </summary>
    public sealed class LoginRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
    }
}
