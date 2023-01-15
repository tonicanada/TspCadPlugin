using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace TspCadPlugin
{
    public partial class FormInterface : Form
    {


        public FormInterface()
        {
            InitializeComponent();
            comboBoxNumNodes.Text = "15";
        }


        public TSP.Node startNode;


        private void FormInterface_Load(object sender, EventArgs e)
        {

        }

        private void btnChristofides_Click(object sender, EventArgs e)
        {
            TSP.ComputeTsp("approxChristofides");
        }

        private void btnOptimalILP1_Click(object sender, EventArgs e)
        {
            TSP.ComputeTsp("optimalIlpWithoutTimeVars");
        }

        private void btnOptimalILP2_Click(object sender, EventArgs e)
        {
            TSP.ComputeTsp("optimalIlpWithTimeVars");
        }

        private void btnOptimalDP_Click(object sender, EventArgs e)
        {
            TSP.ComputeTsp("optimalDynamicProgramming");
        }

        private void btnDoubleTree_Click(object sender, EventArgs e)
        {
            TSP.ComputeTsp("approx2Tree");
        }


        private void btnInsertSampleNodes_Click(object sender, EventArgs e)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (DocumentLock docLock = acDoc.LockDocument())
            using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForWrite) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                Utils.PlotNodesFromCsv(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec, comboBoxNumNodes.SelectedItem.ToString());
                txtBoxStartNode.Text = "";
            }

        }


        private void btnOrTools_Click(object sender, EventArgs e)
        {

            
            int vehicleNumber = Utils.ParseTxtToInt(txtBoxVehicleNumber.Text);

            if (vehicleNumber == -1)
            {
                return;
            }
            if (txtBoxStartNode.Text == "")
            {
                MessageBox.Show("Please select a starting node.");
                return;
            }

            TSP.ComputeTspOrToolsMultipleVehicles(vehicleNumber, startNode.id);
            txtBoxStartNode.Text = "";
        }

        private void btnMst_Click(object sender, EventArgs e)
        {
            TSP.ComputeMst();

        }

        private void btnSelectStartNode_Click(object sender, EventArgs e)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (DocumentLock docLock = acDoc.LockDocument())
            using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForWrite) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                try
                {
                    startNode = Utils.GetStartNode(tr);
                    txtBoxStartNode.Text = startNode.label;
                }
                catch
                {
                    return;
                }

                tr.Commit();
            }
        }
    }
}
