using Autodesk.Windows;
using ricaun.AutoCAD.UI.Input;
using ricaun.AutoCAD.UI.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public static RibbonButton NewButton(this RibbonPanel ribbonPanel, string name = null)
        {
            return NewButton<RibbonButton>(name);
        }

        /// <summary>
        /// Creates a new <see cref="RibbonButton"/> of the specified type <typeparamref name="T"/> with default settings and the specified name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="RibbonButton"/> to create. Must have a parameterless constructor.</typeparam>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the button. Optional.</param>
        /// <returns>A new instance of <typeparamref name="T"/>.</returns>
        public static T NewButton<T>(this RibbonPanel ribbonPanel, string name = null) where T : RibbonButton, new()
        {
            return NewButton<T>(name);
        }

        internal static T NewButton<T>(string name = null) where T : RibbonButton, new()
        {
            if (string.IsNullOrWhiteSpace(name))
                name = typeof(T).Name;

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
        /// Creates a new <see cref="RibbonButton"/> and adds it to the panel.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the button.</param>
        /// <returns>The created <see cref="RibbonButton"/>.</returns>
        public static RibbonButton CreateButton(this RibbonPanel ribbonPanel, string name = null)
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
        public static RibbonButton CreateButton(this RibbonPanelSource ribbonPanel, string name = null)
        {
            RibbonButton ribbonButton = NewButton<RibbonButton>(name);
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
            return ribbonItem.SetCommand(new LockDocumentRelayCommand(command));
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
            return ribbonItem.SetCommand(new LockDocumentRelayCommand<TRibbonItem>(command));
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
                ribbonItem.ShowText = true;
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

        /// <summary>
        /// Adds a slide-out (panel break) to the ribbon panel.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <returns>The ribbon panel with the slide-out added.</returns>
        public static RibbonPanel AddSlideOut(this RibbonPanel ribbonPanel)
        {
            if (ribbonPanel is null) return ribbonPanel;
            ribbonPanel.AddItem(new RibbonPanelBreak());
            return ribbonPanel;
        }

        /// <summary>
        /// Sets the dialog launcher for the specified <see cref="RibbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to set the dialog launcher for.</param>
        /// <param name="ribbonItem">The <see cref="RibbonCommandItem"/> to use as the dialog launcher.</param>
        /// <returns>The <see cref="RibbonPanel"/> with the dialog launcher set.</returns>
        public static RibbonPanel SetDialogLauncher(this RibbonPanel ribbonPanel, RibbonCommandItem ribbonItem)
        {
            if (ribbonPanel is null) return ribbonPanel;
            ribbonPanel.Source.DialogLauncher = ribbonItem;
            ribbonPanel.Remove(ribbonItem);
            return ribbonPanel;
        }

        /// <summary>
        /// Gets the dialog launcher <see cref="RibbonCommandItem"/> for the specified <see cref="RibbonPanel"/>.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to retrieve the dialog launcher from.</param>
        /// <returns>
        /// The <see cref="RibbonCommandItem"/> set as the dialog launcher for the panel, or <c>null</c> if the panel is <c>null</c>.
        /// </returns>
        public static RibbonCommandItem GetDialogLauncher(this RibbonPanel ribbonPanel)
        {
            if (ribbonPanel is null) return null;
            return ribbonPanel.Source.DialogLauncher;
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
        public static RibbonSplitButton NewSplitButton(this RibbonPanel ribbonPanel, string name = null)
        {
            return NewButton<RibbonSplitButton>(name);
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
        public static RibbonSplitButton NewPulldownButton(this RibbonPanel ribbonPanel, string name = null)
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

        #region ToggleButton
        /// <summary>
        /// Creates a new <see cref="RibbonToggleButton"/>, adds it to the panel, and returns it.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the toggle button. Optional.</param>
        /// <returns>The created <see cref="RibbonToggleButton"/>.</returns>
        public static RibbonToggleButton CreateToggleButton(this RibbonPanel ribbonPanel, string name = null)
        {
            var ribbonItem = NewButton<RibbonToggleButton>(name);
            ribbonPanel.AddItem(ribbonItem);
            return ribbonItem;
        }
        #endregion

        #region TextBox
        /// <summary>
        /// Creates a new <see cref="RibbonTextBox"/> with default settings and the specified name.
        /// </summary>
        /// <param name="name">The name and text of the text box. Optional.</param>
        /// <returns>A new <see cref="RibbonTextBox"/> instance.</returns>
        public static RibbonTextBox NewTextBox(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = nameof(RibbonTextBox);

            var ribbonTextBox = new RibbonTextBox
            {
                Size = RibbonItemSize.Standard,
                ShowImage = true,
                Name = name,
                Text = name,
            };
            return ribbonTextBox;
        }

        /// <summary>
        /// Creates a new <see cref="RibbonTextBox"/>, adds it to the specified <see cref="RibbonPanel"/>, and returns it.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the text box. Optional.</param>
        /// <returns>The created <see cref="RibbonTextBox"/>.</returns>
        public static RibbonTextBox CreateTextBox(this RibbonPanel ribbonPanel, string name = null)
        {
            var ribbonItem = NewTextBox(name);
            ribbonPanel.AddItem(ribbonItem);
            return ribbonItem;
        }

        /// <summary>
        /// Sets whether the <see cref="RibbonTextBox"/> should display its image as a button.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="showImageAsButton">Whether to show the image as a button. Default is <c>true</c>.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the updated setting.</returns>
        public static RibbonTextBox SetShowImageAsButton(this RibbonTextBox textBox, bool showImageAsButton = true)
        {
            textBox.ShowImageAsButton = showImageAsButton;
            return textBox;
        }

        /// <summary>
        /// Sets whether the text in the <see cref="RibbonTextBox"/> should be selected when it receives focus.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="selectTextOnFocus">Whether to select text on focus. Default is <c>true</c>.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the updated setting.</returns>
        public static RibbonTextBox SetSelectTextOnFocus(this RibbonTextBox textBox, bool selectTextOnFocus = true)
        {
            textBox.SelectTextOnFocus = selectTextOnFocus;
            return textBox;
        }

        /// <summary>
        /// Sets the prompt text for the <see cref="RibbonTextBox"/>.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="prompt">The prompt text to display.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the updated prompt.</returns>
        public static RibbonTextBox SetPrompt(this RibbonTextBox textBox, string prompt)
        {
            textBox.Prompt = prompt;
            return textBox;
        }

        /// <summary>
        /// Sets the width of the <see cref="RibbonTextBox"/>.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="width">The width to set.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the updated width.</returns>
        public static RibbonTextBox SetWidth(this RibbonTextBox textBox, double width)
        {
            textBox.Width = width;
            return textBox;
        }

        /// <summary>
        /// Sets the value of the <see cref="RibbonTextBox"/>.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="value">The value to set. Optional.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the updated value.</returns>
        public static RibbonTextBox SetValue(this RibbonTextBox textBox, object value = null)
        {
            textBox.Value = value;
            return textBox;
        }

        /// <summary>
        /// Sets the command handler for the <see cref="RibbonTextBox"/> using an <see cref="System.Windows.Input.ICommand"/>.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="command">The command to assign as the handler.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the command handler set.</returns>
        public static RibbonTextBox SetCommand(this RibbonTextBox textBox, System.Windows.Input.ICommand command)
        {
            textBox.InvokesCommand = true;
            textBox.CommandHandler = command;
            return textBox;
        }

        /// <summary>
        /// Sets the command handler for the <see cref="RibbonTextBox"/> using an <see cref="Action"/>.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="command">The action to execute.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the command handler set.</returns>
        public static RibbonTextBox SetCommand(this RibbonTextBox textBox, Action command)
        {
            return textBox.SetCommand(new LockDocumentRelayCommand(command));
        }

        /// <summary>
        /// Sets the command handler for the <see cref="RibbonTextBox"/> using an <see cref="Action{RibbonTextBox}"/>.
        /// </summary>
        /// <param name="textBox">The ribbon text box to extend.</param>
        /// <param name="command">The action to execute with the <see cref="RibbonTextBox"/> as a parameter.</param>
        /// <returns>The <see cref="RibbonTextBox"/> with the command handler set.</returns>
        public static RibbonTextBox SetCommand(this RibbonTextBox textBox, Action<RibbonTextBox> command)
        {
            return textBox.SetCommand(new LockDocumentRelayCommand<RibbonTextBox>(command));
        }

        #endregion

        #region Label
        /// <summary>
        /// Creates a new <see cref="RibbonLabel"/> with default settings and the specified name.
        /// </summary>
        /// <param name="name">The name and text of the label. Optional.</param>
        /// <returns>A new <see cref="RibbonLabel"/> instance.</returns>
        public static RibbonLabel NewLabel(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = nameof(RibbonLabel);

            var label = new RibbonLabel
            {
                Size = RibbonItemSize.Standard,
                ShowImage = true,
                Name = name,
                Text = name,
            };
            return label;
        }

        /// <summary>
        /// Creates a new <see cref="RibbonLabel"/>, adds it to the specified <see cref="RibbonPanel"/>, and returns it.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the label. Optional.</param>
        /// <returns>The created <see cref="RibbonLabel"/>.</returns>
        public static RibbonLabel CreateLabel(this RibbonPanel ribbonPanel, string name = null)
        {
            var ribbonItem = NewLabel(name);
            ribbonPanel.AddItem(ribbonItem);
            return ribbonItem;
        }
        #endregion

        #region ComboBox

        /// <summary>
        /// Creates a new <see cref="RibbonCombo"/> with default settings and the specified name.
        /// </summary>
        /// <param name="name">The name and text of the combo box. Optional.</param>
        /// <returns>A new <see cref="RibbonCombo"/> instance.</returns>
        public static RibbonCombo NewComboBox(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = nameof(RibbonCombo);

            var comboBox = new RibbonCombo
            {
                Size = RibbonItemSize.Standard,
                ShowImage = true,
                Name = name,
                Text = name,
            };
            return comboBox;
        }

        /// <summary>
        /// Creates a new <see cref="RibbonCombo"/>, adds it to the specified <see cref="RibbonPanel"/>, and returns it.
        /// </summary>
        /// <param name="ribbonPanel">The ribbon panel to extend.</param>
        /// <param name="name">The name and text of the combo box. Optional.</param>
        /// <returns>The created <see cref="RibbonCombo"/>.</returns>
        public static RibbonCombo CreateComboBox(this RibbonPanel ribbonPanel, string name = null)
        {
            var ribbonItem = NewComboBox(name);
            ribbonPanel.AddItem(ribbonItem);
            return ribbonItem;
        }

        /// <summary>
        /// Sets the width of the <see cref="RibbonCombo"/>.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to extend.</param>
        /// <param name="width">The width to set.</param>
        /// <returns>The <see cref="RibbonCombo"/> with the updated width.</returns>
        public static RibbonCombo SetWidth(this RibbonCombo comboBox, double width)
        {
            comboBox.Width = width;
            return comboBox;
        }

        /// <summary>
        /// Sets the items of the <see cref="RibbonCombo"/>. Accepts <see cref="RibbonItem"/>s or objects (converted to <see cref="RibbonTextBox"/>).
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to extend.</param>
        /// <param name="items">The items to add to the combo box.</param>
        /// <returns>The <see cref="RibbonCombo"/> with the items set.</returns>
        public static RibbonCombo SetItems(this RibbonCombo comboBox, params object[] items)
        {
            if (comboBox is null)
                return comboBox;

            comboBox.Items.Clear();
            foreach (var item in items)
            {
                if (item is RibbonItem ribbonItem)
                    comboBox.Items.Add(ribbonItem);
                else
                    comboBox.Items.Add(NewTextBox(item?.ToString()));

                if (comboBox.Current is null)
                    comboBox.Current = comboBox.Items[0];
            }
            return comboBox;
        }

        /// <summary>
        /// Gets the list of <see cref="RibbonItem"/>s contained in the <see cref="RibbonCombo"/>.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to query.</param>
        /// <returns>A list of <see cref="RibbonItem"/>s in the combo box.</returns>
        public static IList<RibbonItem> GetItems(this RibbonCombo comboBox)
        {
            var list = new List<RibbonItem>();
            if (comboBox is null) return list;
            foreach (var item in comboBox.Items)
            {
                if (item is RibbonItem ribbonItem)
                    list.Add(ribbonItem);
            }
            return list;
        }

        /// <summary>
        /// Sets the current selected item of the <see cref="RibbonCombo"/>.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to extend.</param>
        /// <param name="current">The item to set as current.</param>
        /// <returns>The <see cref="RibbonCombo"/> with the updated current item.</returns>
        public static RibbonCombo SetCurrent(this RibbonCombo comboBox, object current)
        {
            comboBox.Current = current;
            return comboBox;
        }

        /// <summary>
        /// Gets the current selected <see cref="RibbonItem"/> from the <see cref="RibbonCombo"/>.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to query.</param>
        /// <returns>The current <see cref="RibbonItem"/>, or null if not set.</returns>
        public static RibbonItem GetCurrentRibbon(this RibbonCombo comboBox)
        {
            return comboBox.Current as RibbonItem;
        }

        /// <summary>
        /// Gets the text of the current selected <see cref="RibbonItem"/> in the <see cref="RibbonCombo"/>.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to query.</param>
        /// <returns>The text of the current item, or null if not set.</returns>
        public static string GetCurrentText(this RibbonCombo comboBox)
        {
            return comboBox.GetCurrentRibbon()?.Text;
        }

        /// <summary>
        /// Handles the <see cref="RibbonList.CurrentChanged"/> event and executes the assigned command handler.
        /// </summary>
        /// <param name="sender">The sender of the event (should be a <see cref="RibbonCombo"/>).</param>
        /// <param name="e">The event arguments.</param>
        private static void ComboBox_CurrentChanged(object sender, RibbonPropertyChangedEventArgs e)
        {
            try
            {
                if (sender is RibbonCombo comboBox)
                {
                    if (comboBox.CommandHandler is System.Windows.Input.ICommand command)
                    {
                        command.Execute(comboBox);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Sets the command to be executed when the current item of the <see cref="RibbonCombo"/> changes.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to extend.</param>
        /// <param name="command">The command to execute on current item change.</param>
        /// <returns>The <see cref="RibbonCombo"/> with the command handler set.</returns>
        public static RibbonCombo SetCommandChanged(this RibbonCombo comboBox, System.Windows.Input.ICommand command)
        {
            comboBox.CurrentChanged -= ComboBox_CurrentChanged;
            comboBox.CurrentChanged += ComboBox_CurrentChanged;
            comboBox.CommandHandler = command;
            return comboBox;
        }

        /// <summary>
        /// Sets the action to be executed when the current item of the <see cref="RibbonCombo"/> changes.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to extend.</param>
        /// <param name="command">The action to execute on current item change.</param>
        /// <returns>The <see cref="RibbonCombo"/> with the command handler set.</returns>
        public static RibbonCombo SetCommandChanged(this RibbonCombo comboBox, Action command)
        {
            return comboBox.SetCommandChanged(new LockDocumentRelayCommand(command));
        }

        /// <summary>
        /// Sets the action to be executed with the <see cref="RibbonCombo"/> as a parameter when the current item changes.
        /// </summary>
        /// <param name="comboBox">The ribbon combo box to extend.</param>
        /// <param name="command">The action to execute with the combo box as a parameter.</param>
        /// <returns>The <see cref="RibbonCombo"/> with the command handler set.</returns>
        public static RibbonCombo SetCommandChanged(this RibbonCombo comboBox, Action<RibbonCombo> command)
        {
            return comboBox.SetCommandChanged(new LockDocumentRelayCommand<RibbonCombo>(command));
        }

        #endregion
    }
}