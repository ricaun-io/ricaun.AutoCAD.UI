using System;
using System.Runtime.InteropServices;

namespace ricaun.AutoCAD.UI.Runtime
{
    /// <summary>
    /// Provides utility methods for interacting with AutoCAD commands via native interop.
    /// </summary>
    public static class AcadUtils
    {
        const string ACCORE_DLL = "accore.dll";

        /// <summary>
        /// Posts a command to the AutoCAD command stack using native interop.
        /// </summary>
        /// <param name="cmd">The command string to post.</param>
        /// <returns><c>true</c> if the command was posted successfully; otherwise, <c>false</c>.</returns>
        [DllImport(ACCORE_DLL, CallingConvention = CallingConvention.Cdecl,
           CharSet = CharSet.Unicode,
           EntryPoint = "?acedPostCommand@@YA_NPEB_W@Z")]
        private static extern bool acedPostCommand(string cmd);

        /// <summary>
        /// Posts the "CANCELCMD" command to AutoCAD, effectively canceling the current command.
        /// </summary>
        public static void PostCancel()
        {
            acedPostCommand("CANCELCMD");
        }

        /// <summary>
        /// Posts a specified command string to AutoCAD.
        /// </summary>
        /// <param name="command">The command string to post.</param>
        public static void PostCommand(string command)
        {
            acedPostCommand(command);
        }
    }
}