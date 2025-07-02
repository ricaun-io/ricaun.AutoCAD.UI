using Autodesk.Windows;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.AutoCAD.UI.Utils
{
    /// <summary>
    /// RibbonItemPanelExtension
    /// </summary>
    public static class RibbonItemPanelExtension
    {
        #region Row
        private const int MaxRowNumber = 3;
        internal static RibbonRowPanel[] CreateRowStackedItemsWithMax(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            return ribbonPanel.CreateRowStackedItemsWithMax(MaxRowNumber, ribbonItems);
        }
        internal static RibbonRowPanel[] CreateRowStackedItemsWithMax(this RibbonPanel ribbonPanel, int maxRowNumber, params RibbonItem[] ribbonItems)
        {
            var ribbonFlowPanels = new List<RibbonRowPanel>();

            if (ribbonItems.Length == 0)
                return ribbonFlowPanels.ToArray();

            var list = new List<RibbonItem>();
            for (int i = 0; i < ribbonItems.Length; i++)
            {
                var ribbonItem = ribbonItems[i];
                list.Add(ribbonItem);
                if (list.Count == maxRowNumber)
                {
                    var ribbonFlowPanel = ribbonPanel.CreateRowStackedItems(list.ToArray());
                    ribbonFlowPanels.Add(ribbonFlowPanel);
                    list.Clear();
                }
            }

            if (list.Any())
                ribbonFlowPanels.Add(ribbonPanel.CreateRowStackedItems(list.ToArray()));

            return ribbonFlowPanels.ToArray();
        }
        internal static RibbonRowPanel CreateRowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            if (ribbonItems.Length == 0) return null;

            var ribbonFlowPanel = new RibbonRowPanel();
            var ribbonItemLast = ribbonItems.LastOrDefault();
            foreach (var ribbonItem in ribbonItems)
            {
                ribbonPanel.Remove(ribbonItem);
                ribbonFlowPanel.AddRibbonItem(ribbonItem);

                if (ribbonItemLast != ribbonItem)
                    ribbonFlowPanel.Items.Add(new RibbonRowBreak());
            }
            ribbonPanel.AddItem(ribbonFlowPanel);
            return ribbonFlowPanel;
        }
        /// <summary>
        /// Add <paramref name="ribbonItem"/> to <paramref name="ribbonRowPanel"/> updating properties of the RibbonItem.
        /// </summary>
        /// <param name="ribbonRowPanel"></param>
        /// <param name="ribbonItem"></param>
        /// <returns></returns>
        public static RibbonRowPanel AddRibbonItem(this RibbonRowPanel ribbonRowPanel, RibbonItem ribbonItem)
        {
            ribbonRowPanel.Items.Add(ribbonItem.UpdateForRibbonRowPanel());
            return ribbonRowPanel;
        }
        /// <summary>
        /// Set each <paramref name="ribbonRowPanels"/> items to <paramref name="ribbonItemSize"/>
        /// </summary>
        /// <param name="ribbonRowPanels"></param>
        /// <param name="ribbonItemSize"></param>
        /// <returns></returns>
        public static RibbonRowPanel[] SetRibbonItemSize(
            this RibbonRowPanel[] ribbonRowPanels,
            RibbonItemSize ribbonItemSize = RibbonItemSize.Large)
        {
            foreach (var ribbonRowPanel in ribbonRowPanels)
            {
                ribbonRowPanel.SetRibbonItemSize(ribbonItemSize);
            }
            return ribbonRowPanels;
        }

        /// <summary>
        /// Set <paramref name="ribbonRowPanel"/> items to <paramref name="ribbonItemSize"/>
        /// </summary>
        /// <param name="ribbonRowPanel"></param>
        /// <param name="ribbonItemSize"></param>
        /// <returns></returns>
        public static RibbonRowPanel SetRibbonItemSize(
            this RibbonRowPanel ribbonRowPanel,
            RibbonItemSize ribbonItemSize = RibbonItemSize.Large)
        {
            foreach (var ribbonItem in ribbonRowPanel.Items)
            {
                ribbonItem.Size = ribbonItemSize;
            }
            return ribbonRowPanel;
        }

        internal static RibbonItem UpdateForRibbonRowPanel(this RibbonItem ribbonItem)
        {
            ribbonItem.Size = RibbonItemSize.Standard;
            ribbonItem.AllowInStatusBar = false;
            ribbonItem.SetTextOrientation(System.Windows.Controls.Orientation.Horizontal);
            return ribbonItem;
        }
        #region Utils
        internal static T SetTextOrientation<T>(this T ribbonItem, System.Windows.Controls.Orientation orientation) where T : RibbonItem
        {
            try
            {
                var propriety = ribbonItem.GetType().GetProperty(nameof(Autodesk.Private.Windows.IRibbonTextProperties.Orientation));
                propriety?.SetValue(ribbonItem, orientation);
            }
            catch { }
            return ribbonItem;
        }

        #endregion
        #endregion
        #region Flow
        internal static RibbonFlowPanel CreateFlowStackedItems(this RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems)
        {
            if (ribbonItems.Length == 0) return null;

            var ribbonFlowPanel = new RibbonFlowPanel();
            foreach (var ribbonItem in ribbonItems)
            {
                ribbonPanel.Remove(ribbonItem);
                ribbonFlowPanel.AddRibbonItem(ribbonItem);
            }

            ribbonPanel.AddItem(ribbonFlowPanel);
            return ribbonFlowPanel;
        }
        /// <summary>
        /// Add <paramref name="ribbonItem"/> to <paramref name="ribbonFlowPanel"/> updating properties of the RibbonItem.
        /// </summary>
        /// <param name="ribbonFlowPanel"></param>
        /// <param name="ribbonItem"></param>
        /// <returns></returns>
        public static RibbonFlowPanel AddRibbonItem(this RibbonFlowPanel ribbonFlowPanel, RibbonItem ribbonItem)
        {
            ribbonFlowPanel.Items.Add(ribbonItem.UpdateForRibbonFlowPanel());
            return ribbonFlowPanel;
        }
        internal static RibbonItem UpdateForRibbonFlowPanel(this RibbonItem ribbonItem)
        {
            ribbonItem.Size = RibbonItemSize.Standard;
            ribbonItem.ShowImage = true;
            ribbonItem.ShowText = false;
            ribbonItem.AllowInStatusBar = false;
            return ribbonItem;
        }

        #endregion
    }
}