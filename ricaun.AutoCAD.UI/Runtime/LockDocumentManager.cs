using System;
using Autodesk.AutoCAD.ApplicationServices;

namespace ricaun.AutoCAD.UI.Runtime
{
    /// <summary>
    /// Manages the locking and unlocking of the active AutoCAD document.
    /// Ensures that the document is properly locked during operations and released when disposed.
    /// </summary>
    public class LockDocumentManager : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="DocumentLock"/> instance representing the lock on the document.
        /// </summary>
        public DocumentLock DocumentLock { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockDocumentManager"/> class
        /// and locks the currently active MDI document.
        /// </summary>
        public LockDocumentManager()
        {
            DocumentLock = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument?.LockDocument();
        }

        /// <summary>
        /// Releases the document lock and updates the AutoCAD screen.
        /// </summary>
        public void Dispose()
        {
            if (DocumentLock is DocumentLock)
            {
                DocumentLock.Dispose();
                Autodesk.AutoCAD.ApplicationServices.Core.Application.UpdateScreen();
            }
        }
    }
}