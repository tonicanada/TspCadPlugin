namespace TspCadPlugin
{
    partial class FormInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxNumNodes = new System.Windows.Forms.ComboBox();
            this.btnInsertSampleNodes = new System.Windows.Forms.Button();
            this.labelNumNodes = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOptimal = new System.Windows.Forms.TabPage();
            this.btnOptimalILP2 = new System.Windows.Forms.Button();
            this.btnOptimalDP = new System.Windows.Forms.Button();
            this.btnOptimalILP1 = new System.Windows.Forms.Button();
            this.tabApprox = new System.Windows.Forms.TabPage();
            this.btnMst = new System.Windows.Forms.Button();
            this.btnDoubleTree = new System.Windows.Forms.Button();
            this.btnChristofides = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSelectStartNode = new System.Windows.Forms.Button();
            this.txtBoxStartNode = new System.Windows.Forms.TextBox();
            this.labelVehicleNumber = new System.Windows.Forms.Label();
            this.txtBoxVehicleNumber = new System.Windows.Forms.TextBox();
            this.btnOrTools = new System.Windows.Forms.Button();
            this.ToolTipBtnOptimalDP = new System.Windows.Forms.ToolTip(this.components);
            this.ToolTipBtnOptimalILP1 = new System.Windows.Forms.ToolTip(this.components);
            this.ToolTipBtnOptimalILP2 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabOptimal.SuspendLayout();
            this.tabApprox.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxNumNodes);
            this.groupBox3.Controls.Add(this.btnInsertSampleNodes);
            this.groupBox3.Controls.Add(this.labelNumNodes);
            this.groupBox3.Location = new System.Drawing.Point(13, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 130);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Insert Sample Nodes";
            // 
            // comboBoxNumNodes
            // 
            this.comboBoxNumNodes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumNodes.FormattingEnabled = true;
            this.comboBoxNumNodes.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "22",
            "23",
            "24",
            "25",
            "30",
            "50",
            "100",
            "200",
            "300",
            "500",
            "1000"});
            this.comboBoxNumNodes.Location = new System.Drawing.Point(43, 90);
            this.comboBoxNumNodes.Name = "comboBoxNumNodes";
            this.comboBoxNumNodes.Size = new System.Drawing.Size(131, 21);
            this.comboBoxNumNodes.TabIndex = 0;
            // 
            // btnInsertSampleNodes
            // 
            this.btnInsertSampleNodes.Location = new System.Drawing.Point(43, 25);
            this.btnInsertSampleNodes.Name = "btnInsertSampleNodes";
            this.btnInsertSampleNodes.Size = new System.Drawing.Size(131, 38);
            this.btnInsertSampleNodes.TabIndex = 3;
            this.btnInsertSampleNodes.Text = "Insert Sample Nodes";
            this.btnInsertSampleNodes.UseVisualStyleBackColor = true;
            this.btnInsertSampleNodes.Click += new System.EventHandler(this.btnInsertSampleNodes_Click);
            // 
            // labelNumNodes
            // 
            this.labelNumNodes.AutoSize = true;
            this.labelNumNodes.Location = new System.Drawing.Point(40, 74);
            this.labelNumNodes.Name = "labelNumNodes";
            this.labelNumNodes.Size = new System.Drawing.Size(92, 13);
            this.labelNumNodes.TabIndex = 1;
            this.labelNumNodes.Text = "Number Of Nodes";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabOptimal);
            this.tabControl1.Controls.Add(this.tabApprox);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(13, 146);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(215, 222);
            this.tabControl1.TabIndex = 7;
            // 
            // tabOptimal
            // 
            this.tabOptimal.Controls.Add(this.btnOptimalILP2);
            this.tabOptimal.Controls.Add(this.btnOptimalDP);
            this.tabOptimal.Controls.Add(this.btnOptimalILP1);
            this.tabOptimal.Location = new System.Drawing.Point(4, 22);
            this.tabOptimal.Name = "tabOptimal";
            this.tabOptimal.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptimal.Size = new System.Drawing.Size(207, 196);
            this.tabOptimal.TabIndex = 0;
            this.tabOptimal.Text = "Optimal";
            this.tabOptimal.UseVisualStyleBackColor = true;
            // 
            // btnOptimalILP2
            // 
            this.btnOptimalILP2.Location = new System.Drawing.Point(39, 79);
            this.btnOptimalILP2.Name = "btnOptimalILP2";
            this.btnOptimalILP2.Size = new System.Drawing.Size(131, 38);
            this.btnOptimalILP2.TabIndex = 6;
            this.btnOptimalILP2.Text = "Run \n ILP - formulation 2";
            this.ToolTipBtnOptimalILP2.SetToolTip(this.btnOptimalILP2, "Solve TSP using Integer Linear Programming, formulation with time variables");
            this.btnOptimalILP2.UseVisualStyleBackColor = true;
            this.btnOptimalILP2.Click += new System.EventHandler(this.btnOptimalILP2_Click);
            // 
            // btnOptimalDP
            // 
            this.btnOptimalDP.Location = new System.Drawing.Point(39, 123);
            this.btnOptimalDP.Name = "btnOptimalDP";
            this.btnOptimalDP.Size = new System.Drawing.Size(131, 38);
            this.btnOptimalDP.TabIndex = 5;
            this.btnOptimalDP.Text = "Run \n Dynamic Programming";
            this.ToolTipBtnOptimalDP.SetToolTip(this.btnOptimalDP, "Solve TSP using Dynamic Programming");
            this.btnOptimalDP.UseVisualStyleBackColor = true;
            this.btnOptimalDP.Click += new System.EventHandler(this.btnOptimalDP_Click);
            // 
            // btnOptimalILP1
            // 
            this.btnOptimalILP1.Location = new System.Drawing.Point(39, 35);
            this.btnOptimalILP1.Name = "btnOptimalILP1";
            this.btnOptimalILP1.Size = new System.Drawing.Size(131, 38);
            this.btnOptimalILP1.TabIndex = 4;
            this.btnOptimalILP1.Text = "Run \n ILP - formulation 1";
            this.ToolTipBtnOptimalILP1.SetToolTip(this.btnOptimalILP1, "Solve TSP using Integer Linear Programming, formulation without time variables");
            this.btnOptimalILP1.UseVisualStyleBackColor = true;
            this.btnOptimalILP1.Click += new System.EventHandler(this.btnOptimalILP1_Click);
            // 
            // tabApprox
            // 
            this.tabApprox.Controls.Add(this.btnMst);
            this.tabApprox.Controls.Add(this.btnDoubleTree);
            this.tabApprox.Controls.Add(this.btnChristofides);
            this.tabApprox.Location = new System.Drawing.Point(4, 22);
            this.tabApprox.Name = "tabApprox";
            this.tabApprox.Padding = new System.Windows.Forms.Padding(3);
            this.tabApprox.Size = new System.Drawing.Size(207, 196);
            this.tabApprox.TabIndex = 1;
            this.tabApprox.Text = "Approx";
            this.tabApprox.UseVisualStyleBackColor = true;
            // 
            // btnMst
            // 
            this.btnMst.Location = new System.Drawing.Point(39, 35);
            this.btnMst.Name = "btnMst";
            this.btnMst.Size = new System.Drawing.Size(131, 38);
            this.btnMst.TabIndex = 4;
            this.btnMst.Text = "Run MST";
            this.btnMst.UseVisualStyleBackColor = true;
            this.btnMst.Click += new System.EventHandler(this.btnMst_Click);
            // 
            // btnDoubleTree
            // 
            this.btnDoubleTree.Location = new System.Drawing.Point(39, 123);
            this.btnDoubleTree.Name = "btnDoubleTree";
            this.btnDoubleTree.Size = new System.Drawing.Size(131, 38);
            this.btnDoubleTree.TabIndex = 3;
            this.btnDoubleTree.Text = "Run \n Double-Tree (2.0)";
            this.btnDoubleTree.UseVisualStyleBackColor = true;
            this.btnDoubleTree.Click += new System.EventHandler(this.btnDoubleTree_Click);
            // 
            // btnChristofides
            // 
            this.btnChristofides.Location = new System.Drawing.Point(39, 79);
            this.btnChristofides.Name = "btnChristofides";
            this.btnChristofides.Size = new System.Drawing.Size(131, 38);
            this.btnChristofides.TabIndex = 2;
            this.btnChristofides.Text = "Run \n Christofides (1.5)";
            this.btnChristofides.UseVisualStyleBackColor = true;
            this.btnChristofides.Click += new System.EventHandler(this.btnChristofides_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSelectStartNode);
            this.tabPage1.Controls.Add(this.txtBoxStartNode);
            this.tabPage1.Controls.Add(this.labelVehicleNumber);
            this.tabPage1.Controls.Add(this.txtBoxVehicleNumber);
            this.tabPage1.Controls.Add(this.btnOrTools);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(207, 196);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "OR-Tools";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSelectStartNode
            // 
            this.btnSelectStartNode.Location = new System.Drawing.Point(33, 68);
            this.btnSelectStartNode.Name = "btnSelectStartNode";
            this.btnSelectStartNode.Size = new System.Drawing.Size(102, 20);
            this.btnSelectStartNode.TabIndex = 8;
            this.btnSelectStartNode.Text = "Start Node";
            this.btnSelectStartNode.UseVisualStyleBackColor = true;
            this.btnSelectStartNode.Click += new System.EventHandler(this.btnSelectStartNode_Click);
            // 
            // txtBoxStartNode
            // 
            this.txtBoxStartNode.Location = new System.Drawing.Point(141, 68);
            this.txtBoxStartNode.Name = "txtBoxStartNode";
            this.txtBoxStartNode.ReadOnly = true;
            this.txtBoxStartNode.Size = new System.Drawing.Size(38, 20);
            this.txtBoxStartNode.TabIndex = 6;
            this.txtBoxStartNode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelVehicleNumber
            // 
            this.labelVehicleNumber.AutoSize = true;
            this.labelVehicleNumber.Location = new System.Drawing.Point(30, 45);
            this.labelVehicleNumber.Name = "labelVehicleNumber";
            this.labelVehicleNumber.Size = new System.Drawing.Size(101, 13);
            this.labelVehicleNumber.TabIndex = 5;
            this.labelVehicleNumber.Text = "Number Of Vehicles";
            // 
            // txtBoxVehicleNumber
            // 
            this.txtBoxVehicleNumber.Location = new System.Drawing.Point(142, 42);
            this.txtBoxVehicleNumber.Name = "txtBoxVehicleNumber";
            this.txtBoxVehicleNumber.Size = new System.Drawing.Size(37, 20);
            this.txtBoxVehicleNumber.TabIndex = 4;
            this.txtBoxVehicleNumber.Text = "1";
            this.txtBoxVehicleNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnOrTools
            // 
            this.btnOrTools.Location = new System.Drawing.Point(33, 118);
            this.btnOrTools.Name = "btnOrTools";
            this.btnOrTools.Size = new System.Drawing.Size(146, 38);
            this.btnOrTools.TabIndex = 3;
            this.btnOrTools.Text = "Run \n Vehicle Routing";
            this.btnOrTools.UseVisualStyleBackColor = true;
            this.btnOrTools.Click += new System.EventHandler(this.btnOrTools_Click);
            // 
            // FormInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 377);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormInterface";
            this.Text = "TSP Cad Plugin";
            this.Load += new System.EventHandler(this.FormInterface_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabOptimal.ResumeLayout(false);
            this.tabApprox.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnInsertSampleNodes;
        private System.Windows.Forms.Label labelNumNodes;
        private System.Windows.Forms.ComboBox comboBoxNumNodes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabOptimal;
        private System.Windows.Forms.Button btnOptimalDP;
        private System.Windows.Forms.Button btnOptimalILP1;
        private System.Windows.Forms.TabPage tabApprox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnOrTools;
        private System.Windows.Forms.Button btnDoubleTree;
        private System.Windows.Forms.Button btnChristofides;
        private System.Windows.Forms.Button btnOptimalILP2;
        private System.Windows.Forms.ToolTip ToolTipBtnOptimalILP2;
        private System.Windows.Forms.ToolTip ToolTipBtnOptimalDP;
        private System.Windows.Forms.ToolTip ToolTipBtnOptimalILP1;
        private System.Windows.Forms.Button btnMst;
        private System.Windows.Forms.Button btnSelectStartNode;
        private System.Windows.Forms.TextBox txtBoxStartNode;
        private System.Windows.Forms.Label labelVehicleNumber;
        private System.Windows.Forms.TextBox txtBoxVehicleNumber;
    }
}