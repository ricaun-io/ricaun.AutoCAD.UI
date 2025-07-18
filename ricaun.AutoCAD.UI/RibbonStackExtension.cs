﻿using Autodesk.Windows;
using ricaun.AutoCAD.UI.Utils;

namespace ricaun.AutoCAD.UI
{
    /// <summary>
    /// RibbonStackExtension
    /// </summary>
    public static class RibbonStackExtension
    {
        /// <summary>
        /// Create Row Panels and move the <paramref name="ribbonItems"/> to the new panels
        /// </summary>
        /// <param name="ribbonPanel"></param>
        /// <param name="ribbonItems"></param>
        /// <returns></returns>
        /// <remarks>The <paramref name="ribbonItems"/> is divided into groups of a maximum of 3 in each RibbonRowPanel with Image size.</remarks>
        public static Autodesk.Windows.RibbonRowPanel[] RowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(ribbonItems);
        }

        /// <summary>
        /// Create Row Panels and move the <paramref name="ribbonItems"/> to the new panels
        /// </summary>
        /// <param name="ribbonPanel"></param>
        /// <param name="ribbonItems"></param>
        /// <returns></returns>
        /// <remarks>The <paramref name="ribbonItems"/> is divided into groups of a maximum of 2 in each RibbonRowPanel with LargeImage size.</remarks>
        public static Autodesk.Windows.RibbonRowPanel[] RowLargeStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(2, ribbonItems).SetRibbonItemSize();
        }

        /// <summary>
        /// Create Flow Panels and move the <paramref name="ribbonItems"/> to the new panels
        /// </summary>
        /// <param name="ribbonPanel"></param>
        /// <param name="ribbonItems"></param>
        /// <returns></returns>
        /// <remarks>Each <paramref name="ribbonItems"/> is update to work with RibbonFlowPanel</remarks>
        public static Autodesk.Windows.RibbonFlowPanel FlowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateFlowStackedItems(ribbonItems);
        }
    }
}