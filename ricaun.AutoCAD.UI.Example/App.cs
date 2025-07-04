using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System;

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

            ribbonPanel.RowStackedItems(
                ribbonPanel.CreateButton("Theme")
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Red-Light.tiff"),
                ribbonPanel.CreateButton("Theme")
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Green-Light.ico"),
                ribbonPanel.CreateButton("Theme")
                    .SetCommand(Commands.ThemeChange)
                    .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Blue-32-Light.png")
            );

            ribbonPanel.CreateButton("Circle")
                .SetCommand(Commands.CircleCreate)
                .SetLargeImage("Resources/Box-Cyan-Light.tiff");

            ribbonControl.ActiveTab = ribbonPanel.Tab;
        }
        public override void OnShutdown(RibbonControl ribbonControl)
        {
            ribbonControl.RemovePanel(PanelName, TabName);
        }
    }
}
