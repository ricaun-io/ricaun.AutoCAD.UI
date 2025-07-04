using Autodesk.Windows;
using System.Collections.Generic;

namespace ricaun.AutoCAD.UI.Utils
{
    /// <summary>
    /// Provides utilities for managing ribbon panels and updating their appearance based on the current theme.
    /// </summary>
    public static class RibbonThemePanelUtils
    {
        /// <summary>
        /// Gets a value indicating whether the current theme is light.
        /// </summary>
        public static bool IsLight => RibbonThemeUtils.IsLight;

        /// <summary>
        /// Stores the ribbon panels that are registered for theme change updates.
        /// </summary>
        private static List<RibbonPanel> RibbonThemePanels = new List<RibbonPanel>();

        /// <summary>
        /// Static constructor. Subscribes to the theme changed event.
        /// </summary>
        static RibbonThemePanelUtils()
        {
            RibbonThemeUtils.ThemeChanged += RibbonThemeUtils_ThemeChanged;
        }

        /// <summary>
        /// Disposes the theme panel utils by unsubscribing from the theme changed event and clearing the registered panels.
        /// </summary>
        internal static void Dispose()
        {
            RibbonThemeUtils.ThemeChanged -= RibbonThemeUtils_ThemeChanged;
            RibbonThemePanels.Clear();
        }

        /// <summary>
        /// Disables theme change updates for the specified ribbon panel.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to disable theme change updates for.</param>
        /// <returns>The same ribbon panel instance.</returns>
        internal static RibbonPanel ThemeChangeDisable(this RibbonPanel ribbonPanel) => ThemeChangeEnable(ribbonPanel, false);

        /// <summary>
        /// Enables or disables theme change updates for the specified ribbon panel.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to enable or disable theme change updates for.</param>
        /// <param name="enable">True to enable; false to disable.</param>
        /// <returns>The same ribbon panel instance.</returns>
        internal static RibbonPanel ThemeChangeEnable(this RibbonPanel ribbonPanel, bool enable = true)
        {
            if (ribbonPanel is null) return ribbonPanel;

            RibbonThemePanels.Remove(ribbonPanel);

            if (enable)
            {
                RibbonThemePanels.Add(ribbonPanel);
            }

            return ribbonPanel;
        }

        /// <summary>
        /// Handles the theme changed event and updates the images of registered ribbon panels.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments containing theme information.</param>
        private static void RibbonThemeUtils_ThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            foreach (var ribbonPanel in RibbonThemePanels)
            {
                foreach (var ribbonItem in ribbonPanel.Source.Items)
                {
                    var isLight = e.IsLight;
                    UpdateImageThemes(ribbonItem, isLight);
                }
            }
        }

        /// <summary>
        /// Recursively updates the images of ribbon items based on the current theme.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item to update.</param>
        /// <param name="isLight">True if the theme is light; otherwise, false.</param>
        private static void UpdateImageThemes(RibbonItem ribbonItem, bool isLight)
        {
            void UpdateRibbonItemTheme(RibbonItem item)
            {
                var image = item.Image;
                item.SetLargeImage(item.LargeImage);
                if (image is not null) item.SetImage(image);
            }
            try
            {
                switch (ribbonItem)
                {
                    case RibbonListButton ribbonListButton:
                        UpdateRibbonItemTheme(ribbonListButton);
                        foreach (var item in ribbonListButton.Items)
                            UpdateImageThemes(item, isLight);
                        break;
                    case RibbonButton ribbonButton:
                        UpdateRibbonItemTheme(ribbonButton);
                        break;
                    case RibbonRowPanel ribbonRowPanel:
                        foreach (var item in ribbonRowPanel.Items)
                            UpdateImageThemes(item, isLight);
                        break;
                }
            }
            catch { }
        }
    }

}