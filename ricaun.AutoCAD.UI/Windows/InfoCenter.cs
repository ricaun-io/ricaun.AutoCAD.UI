namespace ricaun.AutoCAD.UI.Windows
{
    /// <summary>
    /// Provides methods to display balloon messages in the AutoCAD InfoCenter.
    /// </summary>
    public static class InfoCenter
    {
        /// <summary>
        /// Displays a balloon message in the AutoCAD InfoCenter.
        /// </summary>
        /// <param name="categoryTitle">The category of the balloon message.</param>
        /// <param name="message">The message to display in the balloon.</param>
        public static void ShowBalloon(string categoryTitle, string message)
        {
            var infoCenterManager = new Autodesk.AutoCAD.AcInfoCenterConn.InfoCenterManager();
            var resultItem = new Autodesk.Internal.InfoCenter.ResultItem();
            resultItem.Category = categoryTitle ?? nameof(InfoCenter);
            resultItem.Title = message;
            infoCenterManager.PaletteManager.ShowBalloon(resultItem);
        }

        /// <summary>
        /// Displays a balloon message in the AutoCAD InfoCenter with a default category.
        /// </summary>
        /// <param name="message">The message to display in the balloon.</param>
        public static void ShowBalloon(string message)
        {
            ShowBalloon(null, message);
        }
    }
}