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
            comboBoxFirstSolutionStrategy.Text = "AUTOMATIC";
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

            if (vehicleNumber == -1 || vehicleNumber == 0)
            {
                MessageBox.Show("Please an integer greater or equal to 1 as the number of vehicles.");
                return;
            }
            if (txtBoxStartNode.Text == "" && vehicleNumber != 1)
            {
                MessageBox.Show("Please select a starting node.");
                return;
            } 
            
            try
            {
                TSP.ComputeTspOrToolsMultipleVehicles(vehicleNumber, startNode, comboBoxFirstSolutionStrategy.SelectedItem.ToString());
            } catch
            {
                MessageBox.Show("No solution found");
            }
            
            txtBoxStartNode.Text = "";
            startNode.label = null;
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

        private void comboBoxFirstSolutionStrategy_DropDown(object sender, EventArgs e)
        {
            Graphics g = (sender as ComboBox).CreateGraphics();
            float highest = 0;
            for (int i =0; i< (sender as ComboBox).Items.Count; i++)
            {
                SizeF textLength = g.MeasureString((sender as ComboBox).Items[i].ToString(), (sender as ComboBox).Font);
                if (textLength.Width > highest)
                {
                    highest = textLength.Width;
                }

            }

            if (highest > 0)
            {
                (sender as ComboBox).DropDownWidth = (int)highest;
            }
        }


        private void comboBoxFirstSolutionStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(comboBoxFirstSolutionStrategy, comboBoxFirstSolutionStrategy.SelectedItem.ToString());
            ToolTip1.AutoPopDelay = 5000;
            ToolTip1.InitialDelay = 1000;
            ToolTip1.ReshowDelay = 500;
            ToolTip1.ShowAlways = true;
        }
    }
}
