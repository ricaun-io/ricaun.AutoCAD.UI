using Autodesk.AutoCAD.Windows;
using ricaun.AutoCAD.UI.Runtime;
using System;

namespace ricaun.AutoCAD.UI.Windows
{
    /// <summary>
    /// Provides utility methods for creating and managing AutoCAD PaletteSets.
    /// </summary>
    public static class PaletteSetUtils
    {
        /// <summary>
        /// Creates a new <see cref="PaletteSet"/> with the specified title, GUID, and visual.
        /// The command name is automatically generated based on the title.
        /// </summary>
        /// <param name="title">The title of the palette set.</param>
        /// <param name="guid">The unique identifier for the palette set.</param>
        /// <param name="visual">The WPF visual to add to the palette set.</param>
        /// <returns>The created <see cref="PaletteSet"/> instance.</returns>
        /// <remarks>
        /// The command name is used to toggle the palette and to remember the palette visibility from the last session.
        /// </remarks>
        public static PaletteSet Create(string title, Guid guid, System.Windows.Media.Visual visual)
        {
            var commandName = "Show_Palette_" + title.Replace(" ", "_").Replace("-", "_").Replace(".", "_");
            return Create(commandName.ToUpperInvariant(), title, guid, visual);
        }

        /// <summary>
        /// Creates a new <see cref="PaletteSet"/> with the specified command name, title, GUID, and visual.
        /// </summary>
        /// <param name="commandName">The command name to register for toggling the palette set visibility.</param>
        /// <param name="title">The title of the palette set.</param>
        /// <param name="guid">The unique identifier for the palette set.</param>
        /// <param name="visual">The WPF visual to add to the palette set.</param>
        /// <returns>The created <see cref="PaletteSet"/> instance.</returns>
        /// <remarks>
        /// The command name is used to toggle the palette and to remember the palette visibility from the last session.
        /// </remarks>
        public static PaletteSet Create(string commandName, string title, Guid guid, System.Windows.Media.Visual visual)
        {
            var paletteSet = new PaletteSet(title, commandName, guid);
            paletteSet.MinimumSize = new System.Drawing.Size(300, 300);
            paletteSet.DockEnabled = DockSides.Right | DockSides.Left;
            paletteSet.Dock = DockSides.Right;
            paletteSet.Style = PaletteSetStyles.ShowAutoHideButton
                                | PaletteSetStyles.ShowCloseButton
                                | PaletteSetStyles.ShowPropertiesMenu
                                | PaletteSetStyles.Snappable;

            paletteSet.KeepFocus = true;
            paletteSet.AddVisual(title, visual);
            CommandUtils.AddCommand(commandName, () => { paletteSet.ToggleVisible(); });
            return paletteSet;
        }

        /// <summary>
        /// Toggles the visibility of the specified <see cref="PaletteSet"/>.
        /// </summary>
        /// <param name="paletteSet">The palette set to toggle visibility for.</param>
        public static void ToggleVisible(this PaletteSet paletteSet)
        {
            paletteSet.Visible = !paletteSet.Visible;
        }
    }
}