using Autodesk.Windows;
using ricaun.AutoCAD.UI.Input;
using ricaun.AutoCAD.UI.Utils;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ricaun.AutoCAD.UI
{
    /// <summary>
    /// Provides extension methods for working with AutoCAD Ribbon UI elements.
    /// </summary>
    public static class RibbonExtension
    {
        /// <summary>
        /// Creates a new <see cref="RibbonButton"/> with default settings and the specified name.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the button.</param>
        /// <returns>A new <see cref="RibbonButton"/> instance.</returns>
        public static RibbonButton NewButton(this RibbonPanel ribbonPanel, string name)
        {
            return ribbonPanel.NewButton<RibbonButton>(name);
        }

        internal static T NewButton<T>(this RibbonPanel ribbonPanel, string name) where T : RibbonButton, new()
        {
            var ribbonButton = new T
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                ShowImage = true,
                ShowText = true,
                Name = name,
                Text = name,
            };
            return ribbonButton;
        }

        /// <summary>
        /// Creates a new <see cref="RibbonButton"/>, adds it to the panel, and assigns a command handler.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the button.</param>
        /// <param name="command">The command to execute when the button is clicked.</param>
        /// <returns>The created <see cref="RibbonButton"/>.</returns>
        public static RibbonButton CreateButton(this RibbonPanel ribbonPanel, string name, Action<RibbonButton> command)
        {
            return ribbonPanel.CreateButton(name)
                .SetCommand(command);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonButton"/>, adds it to the panel, and assigns a command handler.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the button.</param>
        /// <param name="command">The command to execute when the button is clicked.</param>
        /// <returns>The created <see cref="RibbonButton"/>.</returns>
        public static RibbonButton CreateButton(this RibbonPanel ribbonPanel, string name, Action command)
        {
            return ribbonPanel.CreateButton(name)
                .SetCommand(command);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonButton"/> and adds it to the panel.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the button.</param>
        /// <returns>The created <see cref="RibbonButton"/>.</returns>
        public static RibbonButton CreateButton(this RibbonPanel ribbonPanel, string name)
        {
            var ribbonItem = ribbonPanel.NewButton(name);
            ribbonPanel.AddItem(ribbonItem);
            return ribbonItem;
        }

        /// <summary>
        /// Adds a ribbon item to the panel.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="ribbonItem">The ribbon item to add.</param>
        /// <returns>The ribbon panel.</returns>
        public static RibbonPanel AddItem<TRibbonItem>(this RibbonPanel ribbonPanel, TRibbonItem ribbonItem) where TRibbonItem : RibbonItem
        {
            if (ribbonItem is null) return ribbonPanel;
            ribbonPanel.Source.Items.Add(ribbonItem);
            return ribbonPanel;
        }

        /// <summary>
        /// Removes the panel from its parent tab.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to remove.</param>
        /// <returns>The ribbon panel.</returns>
        public static RibbonPanel Remove(this RibbonPanel ribbonPanel)
        {
            if (ribbonPanel.Tab?.Panels.Remove(ribbonPanel) == true)
            {
                ribbonPanel.ThemeChangeDisable();
            }
            return ribbonPanel;
        }

        /// <summary>
        /// Removes a ribbon item from the panel.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="ribbonItem">The ribbon item to remove.</param>
        /// <returns>The ribbon panel.</returns>
        public static RibbonPanel Remove<TRibbonItem>(this RibbonPanel ribbonPanel, TRibbonItem ribbonItem) where TRibbonItem : RibbonItem
        {
            if (ribbonItem is null) return ribbonPanel;
            ribbonPanel.Source.Items.Remove(ribbonItem);
            return ribbonPanel;
        }

        /// <summary>
        /// Creates a new <see cref="RibbonButton"/> and adds it to the panel source.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel source to extend.</param>
        /// <param name="name">The name and text of the button.</param>
        /// <returns>The created <see cref="RibbonButton"/>.</returns>
        public static RibbonButton CreateButton(this RibbonPanelSource ribbonPanel, string name)
        {
            RibbonButton ribbonButton = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                ShowImage = true,
                ShowText = true,
                Name = name,
                Text = name,
            };

            ribbonPanel.Items.Add(ribbonButton);
            return ribbonButton;
        }

        /// <summary>
        /// Sets the command handler for a ribbon command item using an <see cref="System.Windows.Input.ICommand"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon command item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="command">The command to assign as the handler.</param>
        /// <returns>The ribbon item with the command handler set.</returns>
        public static TRibbonItem SetCommand<TRibbonItem>(this TRibbonItem ribbonItem, System.Windows.Input.ICommand command) where TRibbonItem : RibbonCommandItem
        {
            if (command is not null)
                ribbonItem.CommandHandler = command;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the command handler for a ribbon command item.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon command item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetCommand<TRibbonItem>(this TRibbonItem ribbonItem, Action command) where TRibbonItem : RibbonCommandItem
        {
            if (command is not null)
                ribbonItem.CommandHandler = new LockDocumentRelayCommand(command);

            return ribbonItem;
        }

        /// <summary>
        /// Sets the command handler for a ribbon command item with the item as a parameter.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon command item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="command">The command to execute with the ribbon item as a parameter.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetCommand<TRibbonItem>(this TRibbonItem ribbonItem, Action<TRibbonItem> command) where TRibbonItem : RibbonCommandItem
        {
            if (command is not null)
                ribbonItem.CommandHandler = new LockDocumentRelayCommand<TRibbonItem>(command);

            return ribbonItem;
        }

        /// <summary>
        /// Sets the text of a ribbon item and updates its tooltip title.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="value">The text value to set.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetText<TRibbonItem>(this TRibbonItem ribbonItem, string value) where TRibbonItem : RibbonItem
        {
            if (!string.IsNullOrEmpty(value))
            {
                ribbonItem.Text = value;
                if (ribbonItem.ToolTip is RibbonToolTip toolTip) toolTip.Title = value;
            }
            else
            {
                ribbonItem.ShowText = false;
            }
            return ribbonItem;
        }

        /// <summary>
        /// Sets whether the ribbon item should display text.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="showText">Whether to show text.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetShowText<TRibbonItem>(this TRibbonItem ribbonItem, bool showText = false) where TRibbonItem : RibbonItem
        {
            ribbonItem.ShowText = showText;
            return ribbonItem;
        }

        /// <summary>
        /// Sets the description of a ribbon item and updates its tooltip content.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="value">The description value to set.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetDescription<TRibbonItem>(this TRibbonItem ribbonItem, string value) where TRibbonItem : RibbonItem
        {
            ribbonItem.Description = value;
            if (ribbonItem.ToolTip is RibbonToolTip toolTip) toolTip.Content = value;
            return ribbonItem;
        }

        /// <summary>
        /// Sets the tooltip image for a ribbon item using an image path.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="toolTipImage">The image path.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, string toolTipImage) where TRibbonItem : RibbonItem
        {
            return ribbonItem.SetToolTipImage(toolTipImage.GetBitmapSource());
        }

        /// <summary>
        /// Sets the tooltip image for a ribbon item using an <see cref="ImageSource"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="toolTipImage">The image source.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetToolTipImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource toolTipImage) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToolTip is null)
                ribbonItem.SetToolTip(string.Empty);

            if (ribbonItem.ToolTip is RibbonToolTip toolTip) toolTip.ExpandedImage = toolTipImage;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the tooltip content for a ribbon item.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="toolTip">The tooltip content.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetToolTip<TRibbonItem>(this TRibbonItem ribbonItem, string toolTip) where TRibbonItem : RibbonItem
        {
            if (ribbonItem.ToolTip is null)
            {
                ribbonItem.ToolTip = new RibbonToolTip()
                {
                    Title = ribbonItem.Text,
                    Content = ribbonItem.Description
                };
            }

            if (ribbonItem.ToolTip is RibbonToolTip ribbonToolTip) ribbonToolTip.ExpandedContent = toolTip;

            return ribbonItem;
        }

        /// <summary>
        /// Sets the image for a ribbon item using an image path.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="image">The image path.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, string image) where TRibbonItem : RibbonItem
        {
            return ribbonItem.SetImage(image.GetBitmapSource());
        }

        /// <summary>
        /// Sets the image for a ribbon item using an <see cref="ImageSource"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="image">The image source.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource image) where TRibbonItem : RibbonItem
        {
            image = image.GetThemeImageSource(RibbonThemePanelUtils.IsLight);

            ribbonItem.Image = image?.GetBitmapFrame(16, (frame) => { ribbonItem.Image = frame; });
            return ribbonItem;
        }

        /// <summary>
        /// Sets the large image for a ribbon item using an image path.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="largeImage">The large image path.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, string largeImage) where TRibbonItem : RibbonItem
        {
            ribbonItem.SetLargeImage(largeImage.GetBitmapSource());
            if (ribbonItem.Image is null) ribbonItem.SetImage(largeImage);
            return ribbonItem;
        }

        /// <summary>
        /// Sets the large image for a ribbon item using an <see cref="ImageSource"/>.
        /// </summary>
        /// <typeparam name="TRibbonItem">The type of ribbon item.</typeparam>
        /// <param name="ribbonItem">The ribbon item to extend.</param>
        /// <param name="largeImage">The large image source.</param>
        /// <returns>The ribbon item.</returns>
        public static TRibbonItem SetLargeImage<TRibbonItem>(this TRibbonItem ribbonItem, ImageSource largeImage) where TRibbonItem : RibbonItem
        {
            largeImage = largeImage.GetThemeImageSource(RibbonThemePanelUtils.IsLight);

            ribbonItem.LargeImage = largeImage?.GetBitmapFrame(32, (frame) => { ribbonItem.LargeImage = frame; });
            if (ribbonItem.Image is null) ribbonItem.SetImage(largeImage);
            return ribbonItem;
        }

        #region RibbonTab/RibbonPanel
        private const string AddinTabName = "Add-Ins";

        /// <summary>
        /// Finds or creates a ribbon tab with the specified ID and title.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="tabId">The tab ID.</param>
        /// <param name="tabTitle">The tab title (optional).</param>
        /// <returns>The found or created <see cref="RibbonTab"/>.</returns>
        public static RibbonTab CreateOrSelectTab(this RibbonControl ribbon, string tabId = AddinTabName, string tabTitle = null)
        {
            RibbonTab ribbonTab = ribbon.FindTab(tabId);
            if (ribbonTab == null)
            {
                ribbonTab = new RibbonTab();
                ribbonTab.Title = tabTitle ?? tabId;
                ribbonTab.Id = tabId;
                ribbonTab.Name = ribbonTab.Title;
                ribbon.Tabs.Add(ribbonTab);
            }
            return ribbonTab;
        }

        /// <summary>
        /// Creates a new <see cref="RibbonPanel"/> in the specified tab of the ribbon control.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="tabName">The name of the tab to add the panel to.</param>
        /// <param name="panelName">The name of the panel to create.</param>
        /// <returns>The created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreatePanel(this RibbonControl ribbon, string tabName, string panelName)
        {
            RibbonTab ribbonTab = ribbon.CreateOrSelectTab(tabName);
            return ribbonTab.CreatePanel(panelName);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonPanel"/> in the default Add-Ins tab of the ribbon control.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="panelName">The name of the panel to create.</param>
        /// <returns>The created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreatePanel(this RibbonControl ribbon, string panelName)
        {
            return ribbon.CreatePanel(AddinTabName, panelName);
        }

        /// <summary>
        /// Finds or creates a <see cref="RibbonPanel"/> in the specified tab of the ribbon control.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="tabName">The name of the tab to find or create the panel in.</param>
        /// <param name="panelName">The name of the panel to find or create.</param>
        /// <returns>The found or created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreateOrSelectPanel(this RibbonControl ribbon, string tabName, string panelName)
        {
            RibbonTab ribbonTab = ribbon.CreateOrSelectTab(tabName);
            return ribbonTab.CreateOrSelectPanel(panelName);
        }

        /// <summary>
        /// Finds or creates a <see cref="RibbonPanel"/> in the default Add-Ins tab of the ribbon control.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="panelName">The name of the panel to find or create.</param>
        /// <returns>The found or created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreateOrSelectPanel(this RibbonControl ribbon, string panelName)
        {
            return ribbon.CreateOrSelectPanel(AddinTabName, panelName);
        }

        /// <summary>
        /// Removes a <see cref="RibbonPanel"/> from the specified tab in the ribbon control.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="tabName">The name of the tab containing the panel.</param>
        /// <param name="panelName">The name of the panel to remove.</param>
        /// <returns>The removed <see cref="RibbonPanel"/>, or null if not found.</returns>
        public static RibbonPanel RemovePanel(this RibbonControl ribbon, string tabName, string panelName)
        {
            RibbonTab ribbonTab = ribbon.FindTab(tabName);
            if (ribbonTab == null) return null;
            RibbonPanel ribbonPanel = ribbonTab.FindPanel(panelName);
            if (ribbonPanel != null)
            {
                ribbonPanel.Remove();
            }
            return ribbonPanel;
        }

        /// <summary>
        /// Removes a <see cref="RibbonPanel"/> from the default Add-Ins tab in the ribbon control.
        /// </summary>
        /// <param name="ribbon">The ribbon control to extend.</param>
        /// <param name="panelName">The name of the panel to remove.</param>
        /// <returns>The removed <see cref="RibbonPanel"/>, or null if not found.</returns>
        public static RibbonPanel RemovePanel(this RibbonControl ribbon, string panelName)
        {
            return ribbon.RemovePanel(AddinTabName, panelName);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonPanel"/> in the specified tab.
        /// </summary>
        /// <param name="ribbonTab">The ribbon tab to extend.</param>
        /// <param name="panelId">The ID of the panel to create.</param>
        /// <param name="panelTitle">The title of the panel (optional).</param>
        /// <returns>The created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreatePanel(this RibbonTab ribbonTab, string panelId, string panelTitle = null)
        {
            var ribbonPanelSource = new RibbonPanelSource();
            ribbonPanelSource.Title = panelTitle ?? panelId;
            ribbonPanelSource.Id = panelId;

            var ribbonPanel = new RibbonPanel();
            ribbonPanel.ThemeChangeEnable();
            ribbonPanel.Source = ribbonPanelSource;
            ribbonTab.Panels.Add(ribbonPanel);

            return ribbonPanel;
        }

        /// <summary>
        /// Finds or creates a ribbon panel with the specified ID and title in the tab.
        /// </summary>
        /// <param name="ribbonTab">The ribbon tab to extend.</param>
        /// <param name="panelId">The panel ID.</param>
        /// <param name="panelTitle">The panel title (optional).</param>
        /// <returns>The found or created <see cref="RibbonPanel"/>.</returns>
        public static RibbonPanel CreateOrSelectPanel(this RibbonTab ribbonTab, string panelId, string panelTitle = null)
        {
            RibbonPanel ribbonPanel = ribbonTab.FindPanel(panelId);
            if (ribbonPanel is null)
            {
                ribbonPanel = ribbonTab.CreatePanel(panelId, panelTitle);
            }
            return ribbonPanel;
        }

        /// <summary>
        /// Removes the ribbon tab from the ribbon control.
        /// </summary>
        /// <param name="ribbonTab">The ribbon tab to remove.</param>
        public static void Remove(this RibbonTab ribbonTab)
        {
            if (ribbonTab is null) return;
            var ribbonControl = ComponentManager.Ribbon;
            if (ribbonControl is null) return;
            ribbonControl.Tabs.Remove(ribbonTab);
        }

        /// <summary>
        /// Sets the title of the specified <see cref="RibbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to set the title for.</param>
        /// <param name="title">The title to assign to the ribbon panel.</param>
        /// <returns>The <see cref="RibbonPanel"/> with the updated title.</returns>
        public static RibbonPanel SetTitle(this RibbonPanel ribbonPanel, string title)
        {
            if (ribbonPanel is null) return ribbonPanel;
            ribbonPanel.Source.Title = title;
            return ribbonPanel;
        }
        #endregion

        /// <summary>
        /// Adds a separator to the ribbon panel.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <returns>The ribbon panel with the separator added.</returns>
        public static RibbonPanel AddSeparator(this RibbonPanel ribbonPanel)
        {
            if (ribbonPanel is null) return ribbonPanel;
            ribbonPanel.AddItem(new RibbonSeparator());
            return ribbonPanel;
        }

        #region RibbonListButton

        /// <summary>
        /// Creates a new <see cref="RibbonSplitButton"/>, adds it to the panel, and populates it with the specified ribbon items.
        /// Removes the added items from the panel after adding them to the split button.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the split button.</param>
        /// <param name="ribbonItems">The ribbon items to add to the split button.</param>
        /// <returns>The created <see cref="RibbonSplitButton"/>.</returns>
        public static RibbonSplitButton CreateSplitButton(this RibbonPanel ribbonPanel, string name, params RibbonItem[] ribbonItems)
        {
            var splitButton = ribbonPanel.NewSplitButton(name);
            ribbonPanel.AddItem(splitButton);

            return splitButton.AddItems(ribbonPanel, ribbonItems);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonSplitButton"/> configured as a pulldown button, adds it to the panel, and populates it with the specified ribbon items.
        /// Removes the added items from the panel after adding them to the pulldown button.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the pulldown button.</param>
        /// <param name="ribbonItems">The ribbon items to add to the pulldown button.</param>
        /// <returns>The created <see cref="RibbonSplitButton"/> configured as a pulldown button.</returns>
        /// <remarks>
        /// Pulldown is a <see cref="RibbonSplitButton"/> that have <see cref="RibbonListButton.IsSplit"/> false and <see cref="RibbonListButton.IsSynchronizedWithCurrentItem"/> false.
        /// </remarks>
        public static RibbonSplitButton CreatePulldownButton(this RibbonPanel ribbonPanel, string name, params RibbonItem[] ribbonItems)
        {
            var splitButton = ribbonPanel.NewPulldownButton(name);
            ribbonPanel.AddItem(splitButton);

            return splitButton.AddItems(ribbonPanel, ribbonItems);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonSplitButton"/> with default settings and the specified name.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the split button.</param>
        /// <returns>A new <see cref="RibbonSplitButton"/> instance.</returns>
        public static RibbonSplitButton NewSplitButton(this RibbonPanel ribbonPanel, string name)
        {
            return ribbonPanel.NewButton<RibbonSplitButton>(name);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonSplitButton"/> configured as a pulldown button with the specified name.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the pulldown button.</param>
        /// <returns>A new <see cref="RibbonSplitButton"/> configured as a pulldown button.</returns>
        /// <remarks>
        /// Pulldown is a <see cref="RibbonSplitButton"/> that have <see cref="RibbonListButton.IsSplit"/> false and <see cref="RibbonListButton.IsSynchronizedWithCurrentItem"/> false.
        /// </remarks>
        public static RibbonSplitButton NewPulldownButton(this RibbonPanel ribbonPanel, string name)
        {
            var pulldownButton = ribbonPanel.NewSplitButton(name);
            pulldownButton.IsSplit = false;
            pulldownButton.IsSynchronizedWithCurrentItem = false;
            return pulldownButton;
        }

        /// <summary>
        /// Sets the image size for the list items in a <see cref="RibbonSplitButton"/>.
        /// </summary>
        /// <typeparam name="T">The type of the ribbon split button.</typeparam>
        /// <param name="ribbonSplitButton">The ribbon split button to extend.</param>
        /// <param name="listImageSize">The image size to set for the list items. Default is <see cref="RibbonImageSize.Standard"/>.</param>
        /// <returns>The ribbon split button with the updated list image size.</returns>
        public static T SetListImageSize<T>(this T ribbonSplitButton, RibbonImageSize listImageSize = RibbonImageSize.Standard) where T : RibbonSplitButton
        {
            ribbonSplitButton.ListImageSize = listImageSize;
            return ribbonSplitButton;
        }

        /// <summary>
        /// Adds the specified ribbon items to the <see cref="RibbonListButton"/>.
        /// </summary>
        /// <typeparam name="T">The type of ribbon list button.</typeparam>
        /// <param name="ribbonListButton">The ribbon list button to extend.</param>
        /// <param name="ribbonItems">The ribbon items to add.</param>
        /// <returns>The ribbon list button with the items added.</returns>
        public static T AddItems<T>(this T ribbonListButton, params RibbonItem[] ribbonItems) where T : RibbonListButton
        {
            return ribbonListButton.AddItems(null, ribbonItems);
        }

        /// <summary>
        /// Adds the specified ribbon items to the <see cref="RibbonListButton"/> and removes them from the given <see cref="RibbonPanel"/>.
        /// </summary>
        /// <typeparam name="T">The type of ribbon list button.</typeparam>
        /// <param name="ribbonListButton">The ribbon list button to extend.</param>
        /// <param name="ribbonPanel">The ribbon panel from which to remove the items.</param>
        /// <param name="ribbonItems">The ribbon items to add to the list button.</param>
        /// <returns>
        /// The ribbon list button with the items added.
        /// </returns>
        public static T AddItems<T>(this T ribbonListButton, RibbonPanel ribbonPanel, params RibbonItem[] ribbonItems) where T : RibbonListButton
        {
            foreach (var ribbonItem in ribbonItems)
            {
                ribbonListButton.Items.Add(ribbonItem);
                ribbonPanel.Remove(ribbonItem);
            }
            return ribbonListButton;
        }

        #endregion
    }
}