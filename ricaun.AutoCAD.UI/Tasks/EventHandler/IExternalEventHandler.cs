namespace ricaun.AutoCAD.UI.Tasks.EventHandler
{
    /// <summary>
    /// Represents an interface for handling external events.
    /// </summary>
    public interface IExternalEventHandler
    {
        /// <summary>
        /// This method is called to handle the external event.
        /// </summary>
        void Execute();
    }
}