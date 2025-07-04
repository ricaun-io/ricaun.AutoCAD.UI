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
            ribbonPanel.CreateButton("My Button")
                .SetCommand(Commands.MyCommand)
                .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Cyan-Light.tiff");

            ribbonPanel.CreateButton("My Button")
                .SetCommand(Commands.MyCommand)
                .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Cyan-Light.ico");

            ribbonPanel.CreateButton("My Button")
                .SetCommand(Commands.MyCommand)
                .SetLargeImage("https://github.com/ricaun-io/Autodesk.Icon.Example/releases/download/2.0.0/Box-Cyan-32-Light.png");

            ribbonPanel.CreateButton("My Button")
                .SetCommand(Commands.MyCommand)
                .SetLargeImage("Resources/Box-Cyan-Light.tiff");

            ribbonControl.ActiveTab = ribbonPanel.Tab;
        }
        public override void OnShutdown(RibbonControl ribbonControl)
        {
            ribbonControl.RemovePanel(PanelName, TabName);
        }
    }

    public static class Commands
    {
        [CommandMethod("MyCommand")]
        public static void MyCommand()
        {
            
        }
    }
}
