using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using ricaun.AutoCAD.UI.Runtime;
using ricaun.AutoCAD.UI.Tasks.EventHandler;

namespace ricaun.AutoCAD.UI.Tasks
{
    /// <summary>
    /// Provides a service for managing and executing AutoCAD tasks.
    /// </summary>
    public class AutoCADTaskService : IAutoCADTask, IDisposable
    {
        /// <summary>
        /// List of external event handlers to be executed.
        /// </summary>
        private List<IExternalEventHandler> eventHandlers = new List<IExternalEventHandler>();

        /// <summary>
        /// Gets a value indicating whether the service has been initialized.
        /// </summary>
        public bool HasInitialized { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCADTaskService"/> class.
        /// </summary>
        public AutoCADTaskService()
        {
        }

        /// <summary>
        /// Initializes the service and subscribes to the AutoCAD application idle event.
        /// </summary>
        public void Initialize()
        {
            if (HasInitialized) return;
            HasInitialized = true;
            Autodesk.AutoCAD.ApplicationServices.Core.Application.Idle += Application_Idle;
        }

        /// <summary>
        /// Disposes the service, unsubscribes from the idle event, and clears event handlers.
        /// </summary>
        public void Dispose()
        {
            if (!HasInitialized) return;
            HasInitialized = false;
            Autodesk.AutoCAD.ApplicationServices.Core.Application.Idle -= Application_Idle;
            ClearEventHandlers();
        }

        /// <summary>
        /// Handles the AutoCAD application idle event to execute queued event handlers.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Application_Idle(object sender, EventArgs e)
        {
            if (Application.DocumentManager.IsApplicationContext == false) return;
            if (string.IsNullOrEmpty(Application.DocumentManager.MdiActiveDocument?.CommandInProgress) == false) return;
            if (eventHandlers.Count == 0) return;
            lock (eventHandlers)
            {
                foreach (var eventHandler in eventHandlers)
                {
                    if (eventHandler is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    using (new LockDocumentManager())
                    {
                        eventHandler.Execute();
                    }
                }
                eventHandlers.Clear();
            }
        }

        /// <summary>
        /// Clears all event handlers and disposes of any disposable handlers.
        /// </summary>
        private void ClearEventHandlers()
        {
            if (eventHandlers.Count == 0) return;
            lock (eventHandlers)
            {
                foreach (var eventHandler in eventHandlers)
                {
                    if (eventHandler is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
                eventHandlers.Clear();
            }
        }

        /// <summary>
        /// Runs a task with the specified function and cancellation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
        /// <param name="function">The function to execute.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
        {
            var externalEventHandler = new AsyncExternalEventHandlerCancellation<TResult>(function, cancellationToken);
            eventHandlers.Add(externalEventHandler);
            return externalEventHandler.AsyncResult();
        }
    }
}