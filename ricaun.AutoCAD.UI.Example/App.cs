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
                .SetLargeImage("Resources/Box-Cyan-Light.tiff");

            ribbonPanel.CreateButton("ShowMessage")
                .SetCommand((item) => { Windows.MessageBox.ShowMessage(item.Text, "This is a custom message."); })
                .SetLargeImage("Resources/Box-Cyan-Light.tiff");

            ribbonPanel.CreateButton("ShowBalloon")
                .SetCommand((item) => { Windows.InfoCenter.ShowBalloon(item.Text, "This is a custom message."); })
                .SetLargeImage("Resources/Box-Cyan-Light.tiff");

            ribbonControl.ActiveTab = ribbonPanel.Tab;
        }
        public override void OnShutdown(RibbonControl ribbonControl)
        {
            ribbonControl.RemovePanel(PanelName, TabName);
        }
    }
}
