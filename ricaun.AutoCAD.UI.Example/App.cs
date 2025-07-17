using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.Windows;
using ricaun.AutoCAD.UI.Busy;
using ricaun.AutoCAD.UI.Tasks;
using ricaun.AutoCAD.UI.Windows;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

[assembly: ExtensionApplication(typeof(ricaun.AutoCAD.UI.Example.App))]

namespace ricaun.AutoCAD.UI.Example
{
    public class App : ExtensionApplication
    {
        private const string PanelName = "Example";
        private const string TabName = "ricaun";

        public override void OnStartup(RibbonControl ribbonControl)
        {
            var ribbonPanel = ribbonControl.CreateOrSelectPanel(TabName, PanelName);

            ribbonPanel.RowLargeStackedItems(
                ribbonPanel.CreateButton("Theme")
                    .SetShowText(false)
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Grey-Light.tiff"),
                ribbonPanel.CreateButton("Theme")
                    .SetShowText(false)
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Red-Light.tiff"),
                ribbonPanel.CreateButton("Theme")
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Green-Light.ico"),
                ribbonPanel.CreateButton("Theme")
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Blue-32-Light.png")
            );

            ribbonPanel.AddSeparator();

            ribbonPanel.CreateButton("Circle\rCreate")
                .SetCommand(Commands.CircleCreate)
                .SetDescription("Create a circle with a random radius in the model space.")
                .SetToolTip("This button use the command 'CircleCreate'.")
                .SetLargeImage("Resources/Cube-Grey-Light.tiff");

            ribbonPanel.CreateButton("Show\rMessage")
                .SetCommand((item) => { Windows.MessageBox.ShowMessage(item.Text, "This is a custom message."); })
                .SetLargeImage("Resources/Cube-Grey-Light.tiff");

            ribbonPanel.CreateButton("Show\rBalloon")
                .SetCommand((item) => { Windows.InfoCenter.ShowBalloon(item.Text, "This is a custom message."); })
                .SetLargeImage("Resources/Cube-Grey-Light.tiff");

            ribbonButtonBusy = ribbonPanel.CreateButton("None")
                .SetLargeImage("Resources/Cube-Grey-Light.tiff");

            ribbonPanel.CreateButton("Task")
                .SetCommand((item) =>
                {
                    if (autoCADTask is null) return;
                    item.IsEnabled = false;
                    Task.Run(async () =>
                    {
                        try
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                await autoCADTask.Run(Commands.CircleCreate);
                                await Task.Delay(100);
                            }
                        }
                        finally
                        {
                            await autoCADTask.Run(() =>
                            {
                                item.IsEnabled = true;
                            });
                        }
                    });
                })
                .SetLargeImage("Resources/Cube-Grey-Light.tiff");

            ribbonPanel.CreateButton("Show\rPalette")
                .SetCommand(() => { paletteSet?.ToggleVisible(); })
                .SetLargeImage("Resources/Cube-Grey-Light.tiff");

            ribbonPanel.AddSeparator();

            ribbonPanel.CreateSplitButton("Split", 
                ribbonPanel.CreateButton("Grey")
                    .SetCommand(e=>Windows.MessageBox.ShowMessage(e.Text))
                    .SetLargeImage("Resources/Cube-Grey-Light.tiff"),
                ribbonPanel.CreateButton("Red")
                    .SetCommand(e => Windows.MessageBox.ShowMessage(e.Text))
                    .SetLargeImage("Resources/Cube-Red-Light.tiff"),
                ribbonPanel.CreateButton("Green")
                    .SetCommand(e => Windows.MessageBox.ShowMessage(e.Text))
                    .SetLargeImage("Resources/Cube-Green-Light.tiff")
            );

            ribbonPanel.CreatePulldownButton("Pulldown",
                ribbonPanel.CreateButton("Grey")
                    .SetCommand(e => Windows.MessageBox.ShowMessage(e.Text))
                    .SetLargeImage("Resources/Cube-Grey-Light.tiff"),
                ribbonPanel.CreateButton("Red")
                    .SetCommand(e => Windows.MessageBox.ShowMessage(e.Text))
                    .SetLargeImage("Resources/Cube-Red-Light.tiff"),
                ribbonPanel.CreateButton("Green")
                    .SetCommand(e => Windows.MessageBox.ShowMessage(e.Text))
                    .SetLargeImage("Resources/Cube-Green-Light.tiff")
            )
            .SetLargeImage("Resources/Cube-Green-Light.tiff")
            .SetDescription("This is a PulldownButton");

            ribbonControl.ActiveTab = ribbonPanel.Tab;
        }
        public override void OnShutdown(RibbonControl ribbonControl)
        {
            ribbonControl.RemovePanel(TabName, PanelName);
        }

        private RibbonButton ribbonButtonBusy;
        private PaletteSet paletteSet;
        private AutoCADBusyService busyService;
        private AutoCADTaskService taskService;
        public IAutoCADTask autoCADTask => taskService;
        public override void Initialize()
        {
            base.Initialize();
            busyService = new AutoCADBusyService();
            busyService.Initialize();
            busyService.PropertyChanged += (sender, e) =>
            {
                ribbonButtonBusy?.SetText(busyService.IsAutoCADBusy ? "Busy" : "Idle");
                var color = busyService.IsAutoCADBusy ? "Red" : "Green";
                ribbonButtonBusy?.SetLargeImage($"Resources/Cube-{color}-Light.tiff");
            };
            taskService = new AutoCADTaskService();
            taskService.Initialize();

            var visual = new System.Windows.Controls.Grid()
            {
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray),
            };
            paletteSet = PaletteSetUtils.Create("Example Palette Set", new Guid("360B945E-1EFF-4212-9C00-3E841A9F1B28"), visual);
        }

        public override void Terminate()
        {
            base.Terminate();
            busyService?.Dispose();
            taskService?.Dispose();
        }
    }
}
