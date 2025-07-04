using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

namespace ricaun.AutoCAD.UI.Example
{
    public static class Commands
    {
        [CommandMethod("ThemeChange")]
        public static void ThemeChange()
        {
            Application.SetSystemVariable("COLORTHEME", 1 ^ (short)Application.GetSystemVariable("COLORTHEME"));
        }
    }
}
