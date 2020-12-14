﻿using System.Reflection;

namespace Expensely.Api.Controllers
{
    /// <summary>
    /// Represents the controllers assembly.
    /// </summary>
    public static class ControllersAssembly
    {
        /// <summary>
        /// The controllers assembly.
        /// </summary>
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}