using Autodesk.AutoCAD.Internal;
using Autodesk.Windows;
using System;

namespace ricaun.AutoCAD.UI.Utils
{
    /// <summary>
    /// ThemeChangedEventArgs
    /// </summary>
    public sealed class ThemeChangedEventArgs : EventArgs
    {
        internal ThemeChangedEventArgs(bool isLight)
        {
            IsLight = isLight;
        }
        /// <summary>
        /// Theme is Light
        /// </summary>
        public bool IsLight { get; private set; }
        /// <summary>
        /// Theme is Dark
        /// </summary>
        public bool IsDark => !IsLight;
    }

    /// <summary>
    /// ThemeChangedEventHandler for RibbonThemeUtils
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);

    /// <summary>
    /// Provides utility methods and events for handling the AutoCAD ribbon theme (light or dark).
    /// </summary>
    public static class RibbonThemeUtils
    {
        /// <summary>
        /// Occurs when the ribbon theme changes.
        /// </summary>
        public static event ThemeChangedEventHandler ThemeChanged;

        /// <summary>
        /// Gets a value indicating whether the current theme is light.
        /// </summary>
        public static bool IsLight => ActiveThemeColor.CurrentTheme == ColorThemeEnum.Light;

        /// <summary>
        /// Gets a value indicating whether the current theme is dark.
        /// </summary>
        public static bool IsDark => !IsLight;

        /// <summary>
        /// Initializes static members of the <see cref="RibbonThemeUtils"/> class.
        /// Subscribes to theme change events and executes the current theme logic.
        /// </summary>
        static RibbonThemeUtils()
        {
            ExecuteCurrentTheme();
            ActiveThemeColor.Instance().ColorThemeChanged += ActiveThemeColor_ColorThemeChanged;
        }

        /// <summary>
        /// Handles the <see cref="ActiveThemeColor.ColorThemeChanged"/> event.
        /// Executes the current theme logic when the color theme changes.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data.</param>
        private static void ActiveThemeColor_ColorThemeChanged(object sender, EventArgs e)
        {
            ExecuteCurrentTheme();
        }

        /// <summary>
        /// Disposes event handlers and clears the <see cref="ThemeChanged"/> event.
        /// </summary>
        internal static void Dispose()
        {
            ActiveThemeColor.Instance().ColorThemeChanged -= ActiveThemeColor_ColorThemeChanged;
            ThemeChanged = null;
        }

        /// <summary>
        /// Executes the current theme logic and raises the <see cref="ThemeChanged"/> event.
        /// </summary>
        private static void ExecuteCurrentTheme()
        {
            ThemeChanged?.Invoke(ComponentManager.Ribbon, new ThemeChangedEventArgs(IsLight));
        }
    }
}