using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Autodesk.AutoCAD.ApplicationServices;

namespace ricaun.AutoCAD.UI.Busy
{
    /// <summary>
    /// Provides a service to monitor AutoCAD's idle state and manage a timer to check idling updates.
    /// </summary>
    public class AutoCADBusyService : IDisposable, INotifyPropertyChanged
    {
        private const double IntervalMillis = 1000;
        private DispatcherTimer dispatcher;

        /// <summary>
        /// Gets a value indicating whether the service has been initialized.
        /// </summary>
        public bool HasInitialized { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCADBusyService"/> class.
        /// </summary>
        public AutoCADBusyService()
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
            InitializeDispatcherTimer();
        }

        #region Timer
        /// <summary>
        /// Sets the interval for the dispatcher timer.
        /// </summary>
        /// <param name="intervalMillis">The interval in milliseconds. Defaults to 1000 milliseconds.</param>
        public void SetInterval(double intervalMillis = IntervalMillis)
        {
            dispatcher.Interval = TimeSpan.FromMilliseconds(intervalMillis);
        }

        /// <summary>
        /// Initializes the dispatcher timer with a default interval and tick event.
        /// </summary>
        private void InitializeDispatcherTimer()
        {
            dispatcher = new DispatcherTimer(DispatcherPriority.Background);
            dispatcher.Interval = TimeSpan.FromMilliseconds(IntervalMillis);
            dispatcher.Tick += (s, e) =>
            {
                IsAutoCADBusy = countIdling == 0;
                countIdling = 0;
            };
            dispatcher.Start();
        }

        private int countIdling = 0;

        /// <summary>
        /// Handles the AutoCAD application idle event.
        /// Increments the idling counter and disposes the service if needed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void Application_Idle(object sender, EventArgs e)
        {
            if (Application.DocumentManager.IsApplicationContext == false) return;
            if (string.IsNullOrEmpty(Application.DocumentManager.MdiActiveDocument?.CommandInProgress) == false) return;
            countIdling++;
            if (NeedToDispose) Dispose();
        }
        #endregion

        #region Dispose
        private bool NeedToDispose;

        /// <summary>
        /// Disposes the service, unsubscribes from events, and stops the timer.
        /// </summary>
        public void Dispose()
        {
            if (!HasInitialized) return;
            HasInitialized = false;
            NeedToDispose = true;
            try
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.Idle -= Application_Idle;
                dispatcher.Stop();
            }
            catch { }
        }
        #endregion

        #region IsRevitBusy
        private bool isAutoCADBusy = true;

        /// <summary>
        /// Gets a value indicating whether AutoCAD is busy or idle.
        /// </summary>
        public bool IsAutoCADBusy
        {
            get { return isAutoCADBusy; }
            private set
            {
                var changed = isAutoCADBusy != value;
                isAutoCADBusy = value;
                if (changed)
                {
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region PropertyChanged
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Defaults to the caller member name.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
