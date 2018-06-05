using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

[assembly: CommandClass(typeof(MyFirstProject1.Class1))]

namespace MyFirstProject1
{
    public class Class1
    {
        [CommandMethod("AdskGreeting")]
        public void AdskGreeting()
        {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Starts a new transaction with the Transaction Manager
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                /* Creates a new MText object and assigns it a location,
                text value and text style */
                using (MText objText = new MText())
                {
                    // Specify the insertion point of the MText object
                    objText.Location = new Autodesk.AutoCAD.Geometry.Point3d(20, 40, 0);

                    // Set the text string for the MText object
                    objText.Contents = "Расчетная мощность - составляет (в киловатах) : ";

                    // Set the text style for the MText object
                    objText.TextStyleId = acCurDb.Textstyle;

                    // Appends the new MText object to model space
                    acBlkTblRec.AppendEntity(objText);

                    // Appends to new MText object to the active transaction
                    acTrans.AddNewlyCreatedDBObject(objText, true);
                }
                using (Line objLine = new Line())
                {
                    objLine.StartPoint = new Autodesk.AutoCAD.Geometry.Point3d(20, 35, 0);
                    objLine.EndPoint = new Autodesk.AutoCAD.Geometry.Point3d(40, 35, 0);
                    objLine.Color = new Autodesk.AutoCAD.Colors.Color();
                    acBlkTblRec.AppendEntity(objLine);
                    acTrans.AddNewlyCreatedDBObject(objLine, true);
                }
                using (Circle objCircle = new Circle())
                {
                    objCircle.Center = new Autodesk.AutoCAD.Geometry.Point3d(240, 35, 0);
                    objCircle.Radius = 10;
                    acBlkTblRec.AppendEntity(objCircle);
                    acTrans.AddNewlyCreatedDBObject(objCircle, true);
                }

                // Saves the changes to the database and closes the transaction
                acTrans.Commit();
            }
        }
    }
}
