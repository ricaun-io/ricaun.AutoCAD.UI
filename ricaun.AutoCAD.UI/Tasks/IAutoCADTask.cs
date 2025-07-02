using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.AutoCAD.UI.Tasks
{
    /// <summary>
    /// Represents an interface for running AutoCAD tasks.
    /// </summary>
    public interface IAutoCADTask
    {
        /// <summary>
        /// Runs a task with the specified function and cancellation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">The function to execute.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken);
    }
}