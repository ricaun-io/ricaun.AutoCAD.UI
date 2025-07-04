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
        /// <param name="message">The message to display in the balloon.</param>
        /// <param name="category">The category of the balloon message.</param>
        public static void ShowBalloon(string message, string category)
        {
            var infoCenterManager = new Autodesk.AutoCAD.AcInfoCenterConn.InfoCenterManager();
            var resultItem = new Autodesk.Internal.InfoCenter.ResultItem();
            resultItem.Category = category;
            resultItem.Title = message;
            infoCenterManager.PaletteManager.ShowBalloon(resultItem);
        }
    }
}