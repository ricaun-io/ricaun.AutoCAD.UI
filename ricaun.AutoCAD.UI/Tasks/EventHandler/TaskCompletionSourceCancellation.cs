using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.AutoCAD.UI.Tasks.EventHandler
{
    /// <summary>
    /// A <see cref="TaskCompletionSource{TResult}"/> implementation with cancellation token support.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
    internal class TaskCompletionSourceCancellation<TResult> : TaskCompletionSource<TResult>, IDisposable
    {
        private readonly IDisposable registration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCompletionSourceCancellation{TResult}"/> class.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        public TaskCompletionSourceCancellation(CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = CancellationToken.None;

            if (cancellationToken.IsCancellationRequested)
            {
                TrySetCanceled(cancellationToken);
                return;
            }
            registration = cancellationToken.Register(() => TrySetCanceled(cancellationToken));
        }

        /// <summary>
        /// Disposes the resources used by the task completion source.
        /// </summary>
        public void Dispose()
        {
            registration?.Dispose();
        }
    }
}