using System.Reflection;

namespace Expensely.Presentation.Api
{
    /// <summary>
    /// Represents the presentation assembly.
    /// </summary>
    public static class PresentationAssembly
    {
        /// <summary>
        /// The presentation assembly.
        /// </summary>
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
