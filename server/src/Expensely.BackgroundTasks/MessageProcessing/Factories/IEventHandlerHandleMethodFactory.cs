using System.Threading.Tasks;

namespace Expensely.BackgroundTasks.MessageProcessing.Factories
{
    /// <summary>
    /// Represents the event handler handle method factory.
    /// </summary>
    public interface IEventHandlerHandleMethodFactory
    {
        /// <summary>
        /// Gets the event handler handle method task.
        /// </summary>
        /// <param name="eventHandler">The event handler instance.</param>
        /// <param name="handleMethodArguments">The handle method arguments.</param>
        /// <returns>The task of the event handler handle method that can be awaited.</returns>
        Task GetHandleMethodTask(object eventHandler, object[] handleMethodArguments);
    }
}
