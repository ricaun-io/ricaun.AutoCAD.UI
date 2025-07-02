using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.AutoCAD.UI.Tasks
{
    /// <summary>
    /// Provides extension methods for the <see cref="IAutoCADTask"/> interface.
    /// </summary>
    public static class AutoCADTaskExtensions
    {
        /// <summary>
        /// Runs a task with the specified function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="task">The AutoCAD task service.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task<TResult> Run<TResult>(this IAutoCADTask task, Func<TResult> function)
        {
            return task.Run(function, CancellationToken.None);
        }

        /// <summary>
        /// Runs a task with the specified action and cancellation token.
        /// </summary>
        /// <param name="task">The AutoCAD task service.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task Run(this IAutoCADTask task, Action action, CancellationToken cancellationToken)
        {
            return task.Run<object>(() => { action(); return null; }, cancellationToken);
        }

        /// <summary>
        /// Runs a task with the specified action.
        /// </summary>
        /// <param name="task">The AutoCAD task service.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static Task Run(this IAutoCADTask task, Action action)
        {
            return task.Run(action, CancellationToken.None);
        }
    }
}