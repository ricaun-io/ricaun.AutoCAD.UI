using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.AutoCAD.UI.Tasks.EventHandler
{
    /// <summary>
    /// Handles asynchronous external events with cancellation support.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
    public class AsyncExternalEventHandlerCancellation<TResult> : IExternalEventHandler, IDisposable
    {
        private readonly Func<TResult> function;
        private readonly TaskCompletionSourceCancellation<TResult> tcs;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncExternalEventHandlerCancellation{TResult}"/> class.
        /// </summary>
        /// <param name="function">The function to execute when the event is handled.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        public AsyncExternalEventHandlerCancellation(Func<TResult> function, CancellationToken cancellationToken)
        {
            this.function = function;
            tcs = new TaskCompletionSourceCancellation<TResult>(cancellationToken);
        }

        /// <summary>
        /// Gets the asynchronous result of the external event handler.
        /// </summary>
        /// <returns>A task representing the result of the external event handler.</returns>
        public Task<TResult> AsyncResult()
        {
            return tcs.Task;
        }

        /// <summary>
        /// Executes the external event handler.
        /// </summary>
        public void Execute()
        {
            try
            {
                if (AsyncResult().IsCompleted)
                    return;

                var result = function.Invoke();
                tcs.TrySetResult(result);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        /// <summary>
        /// Disposes the resources used by the external event handler.
        /// </summary>
        public void Dispose()
        {
            tcs.Dispose();
        }
    }
}