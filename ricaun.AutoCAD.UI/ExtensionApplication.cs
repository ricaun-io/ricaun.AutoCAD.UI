using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Ribbon;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System;

[assembly: ExtensionApplication(null)]

namespace ricaun.AutoCAD.UI
{
    /// <summary>
    /// Abstract base class for implementing an AutoCAD Extension Application.
    /// Provides methods for initialization, termination, and handling AutoCAD events.
    /// </summary>
    /// <remarks>
    /// Example implementation:
    /// <code>
    /// [assembly: ExtensionApplication(MyExtensionApp)]
    /// 
    /// public class MyExtensionApp : ExtensionApplication
    /// {
    ///     public override void OnStartup(RibbonControl ribbonControl)
    ///     {
    ///         ribbonControl.CreateOrSelectPanel("MyPanel", "MyTab")
    ///     }
    ///
    ///     public override void OnShutdown(RibbonControl ribbonControl)
    ///     {
    ///         ribbonControl.RemovePanel("MyPanel", "MyTab");
    ///     }
    /// }
    /// </code>
    /// </remarks>
    public abstract class ExtensionApplication : IExtensionApplication
    {
        private bool isInitialized = false;

        /// <summary>
        /// Initializes the ExtensionApplication and subscribes to necessary AutoCAD events.
        /// </summary>
        public virtual void Initialize()
        {
            RibbonServices.RibbonPaletteSetCreated += RibbonServices_RibbonPaletteSetCreated;
            Application.QuitWillStart += Application_QuitWillStart;

            if (ComponentManager.Ribbon is not null) ComponentManager_ItemInitialized(null, null); // Trigger OnStartup when AutoCAD LoadOnAppearance
        }

        /// <summary>
        /// Terminates the ExtensionApplication, unsubscribes from events, and executes shutdown logic.
        /// </summary>
        public virtual void Terminate()
        {
            RibbonServices.RibbonPaletteSetCreated -= RibbonServices_RibbonPaletteSetCreated;
            ComponentManager.ItemInitialized -= ComponentManager_ItemInitialized;
            Application.QuitWillStart -= Application_QuitWillStart;

            if (isInitialized) Application_QuitWillStart(null, null); // Trigger OnShutdown when AutoCAD terminates before QuitWillStart

            if (RibbonServices.RibbonPaletteSet is not null)
            {
                RibbonServices.RibbonPaletteSet.WorkspaceLoaded -= RibbonPaletteSet_WorkspaceLoaded;
                RibbonServices.RibbonPaletteSet.WorkspaceUnloading -= RibbonPaletteSet_WorkspaceUnloading;
            }
        }

        /// <summary>
        /// Event handler for when the RibbonPaletteSet is created.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void RibbonServices_RibbonPaletteSetCreated(object sender, EventArgs e)
        {
            RibbonServices.RibbonPaletteSetCreated -= RibbonServices_RibbonPaletteSetCreated;
            ComponentManager.ItemInitialized += ComponentManager_ItemInitialized;
        }

        /// <summary>
        /// Event handler for when the RibbonControl is initialized and ready to add items.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void ComponentManager_ItemInitialized(object sender, RibbonItemEventArgs e)
        {
            ComponentManager.ItemInitialized -= ComponentManager_ItemInitialized;
            RibbonServices.RibbonPaletteSetCreated -= RibbonServices_RibbonPaletteSetCreated;
            RibbonServices.RibbonPaletteSet.WorkspaceLoaded += RibbonPaletteSet_WorkspaceLoaded;
            RibbonServices.RibbonPaletteSet.WorkspaceUnloading += RibbonPaletteSet_WorkspaceUnloading;
            ExecuteStartup();
        }

        /// <summary>
        /// Event handler for when the RibbonPaletteSet workspace is loaded.
        /// Triggered after using commands like CUI, MENULOAD, MENUUNLOAD, and CUSTOMIZE.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void RibbonPaletteSet_WorkspaceLoaded(object sender, EventArgs e)
        {
            ExecuteStartup();
        }

        /// <summary>
        /// Event handler for when the RibbonPaletteSet workspace is unloading.
        /// Triggered after using commands like CUI, MENULOAD, MENUUNLOAD, and CUSTOMIZE.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void RibbonPaletteSet_WorkspaceUnloading(object sender, EventArgs e)
        {
            ExecuteShutdown();
        }

        /// <summary>
        /// Event handler for when AutoCAD is closing.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        private void Application_QuitWillStart(object sender, EventArgs e)
        {
            Application.QuitWillStart -= Application_QuitWillStart;
            ExecuteShutdown();
        }

        /// <summary>
        /// Executes the startup logic for the ExtensionApplication.
        /// </summary>
        private void ExecuteStartup()
        {
            if (isInitialized) return;
            isInitialized = true;
            try
            {
                OnStartup(ComponentManager.Ribbon);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                ShowBalloon(ex.ToString());
            }
        }

        /// <summary>
        /// Executes the shutdown logic for the ExtensionApplication.
        /// </summary>
        private void ExecuteShutdown()
        {
            if (!isInitialized) return;
            isInitialized = false;
            try
            {
                OnShutdown(ComponentManager.Ribbon);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                ShowBalloon(ex.ToString());
            }
        }

        /// <summary>
        /// Called during startup when AutoCAD is loaded and the RibbonControl is ready to add items.
        /// </summary>
        /// <param name="ribbonControl">The RibbonControl instance.</param>
        public abstract void OnStartup(RibbonControl ribbonControl);

        /// <summary>
        /// Called during shutdown when AutoCAD unloads the RibbonControl.
        /// </summary>
        /// <param name="ribbonControl">The RibbonControl instance. May have no tabs.</param>
        public abstract void OnShutdown(RibbonControl ribbonControl);

        /// <summary>
        /// Displays a balloon message in the AutoCAD InfoCenter.
        /// </summary>
        /// <param name="message">The message to display.</param>
        internal void ShowBalloon(string message)
        {
            var infoCenterManager = new Autodesk.AutoCAD.AcInfoCenterConn.InfoCenterManager();
            var resultItem = new Autodesk.Internal.InfoCenter.ResultItem();
            resultItem.Category = this.GetType().Assembly.GetName().Name;
            resultItem.Title = message;
            infoCenterManager.PaletteManager.ShowBalloon(resultItem);
        }
    }
}