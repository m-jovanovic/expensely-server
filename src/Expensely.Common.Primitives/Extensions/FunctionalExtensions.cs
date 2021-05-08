using System;
using System.Threading.Tasks;

namespace Expensely.Common.Primitives.Extensions
{
    /// <summary>
    /// Contains functional extension methods.
    /// </summary>
    public static class FunctionalExtensions
    {
        /// <summary>
        /// Maps the result of the task based on the specified mapping function and return it.
        /// </summary>
        /// <typeparam name="TIn">The result type.</typeparam>
        /// <typeparam name="TOut">The output result type.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="func">The mapping function.</param>
        /// <returns>The mapped value.</returns>
        public static async Task<TOut> Map<TIn, TOut>(this Task<TIn> task, Func<TIn, TOut> func) => func(await task);
    }
}
