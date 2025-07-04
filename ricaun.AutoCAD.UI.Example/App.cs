using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using ricaun.AutoCAD.UI.Busy;
using System;
using System.Diagnostics;

[assembly: ExtensionApplication(typeof(ricaun.AutoCAD.UI.Example.App))]

namespace ricaun.AutoCAD.UI.Example
{
    public class App : ExtensionApplication
    {
        private const string PanelName = "Example";
        private const string TabName = "ricaun";

        public override void OnStartup(RibbonControl ribbonControl)
        {
            var ribbonPanel = ribbonControl.CreateOrSelectPanel(PanelName, TabName);

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

            ribbonControl.ActiveTab = ribbonPanel.Tab;
        }
        public override void OnShutdown(RibbonControl ribbonControl)
        {
            ribbonControl.RemovePanel(PanelName, TabName);
        }

        private RibbonButton ribbonButtonBusy;
        private AutoCADBusyService busyService;
        public override void Initialize()
        {
            base.Initialize();
            busyService = new AutoCADBusyService();
            busyService.Initialize();
            busyService.PropertyChanged += (sender, e) =>
            {
                ribbonButtonBusy?.SetText(busyService.IsAutoCADBusy ? "Busy" : "Idle");
                var color = busyService.IsAutoCADBusy ? "Red" : "Green";
                Debug.WriteLine(color);
                ribbonButtonBusy?.SetLargeImage($"Resources/Cube-{color}-Light.tiff");
            };
        }

        public override void Terminate()
        {
            base.Terminate();
            busyService?.Dispose();
        }
    }
}
