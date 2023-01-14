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


        private void PlotSampleGraphWithNNodes(Document acDoc, Database acCurDb, Transaction tr, BlockTable acBlkTbl, BlockTableRecord acBlkTblRec, string str)
        {
            Utils.PlotNodesFromCsv(acDoc, acCurDb, tr, acBlkTbl, acBlkTblRec, comboBoxNumNodes.SelectedItem.ToString());
        }

        private void FormInterface_Load(object sender, EventArgs e)
        {

        }

        private void btnChristofides_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeTsp, "approxChristofides");
        }

        private void btnOptimalILP1_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeTsp, "optimalIlpWithoutTimeVars");
        }

        private void btnOptimalILP2_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeTsp, "optimalIlpWithTimeVars");
        }

        private void btnOptimalDP_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeTsp, "optimalDynamicProgramming");
        }

        private void btnInsertSampleNodes_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(PlotSampleGraphWithNNodes, "");
        }

        private void btnDoubleTree_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeTsp, "approx2Tree");
        }

        private void btnOrTools_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeTsp, "ORTools");
        }

        private void btnMst_Click(object sender, EventArgs e)
        {
            Utils.GenerateCadTransaction(TSP.ComputeMST, "");

        }

    }
}
