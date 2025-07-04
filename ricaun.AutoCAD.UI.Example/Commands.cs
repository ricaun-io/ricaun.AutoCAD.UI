using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;

namespace ricaun.AutoCAD.UI.Example
{
    public static class Commands
    {
        [CommandMethod("ThemeChange")]
        public static void ThemeChange()
        {
            Application.SetSystemVariable("COLORTHEME", 1 ^ (short)Application.GetSystemVariable("COLORTHEME"));
        }

        [CommandMethod("CircleCreate")]
        public static void CircleCreate()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;

            if (document is null) return;

            var database = document.Database;

            using (var transaction = database.TransactionManager.StartTransaction())
            {
                var blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                var blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                var circle = new Circle()
                {
                    Radius = new Random().NextDouble() * 100.0,
                    Center = new Point3d(0, 0, 0),
                };

                blockTableRecord.AppendEntity(circle);

                transaction.AddNewlyCreatedDBObject(circle, true);
                transaction.Commit();
            }
        }
    }
}
