namespace BearingCAD22
{
    partial class frmProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProject));
            this.lblBorder = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmdImport_XLRadial = new System.Windows.Forms.Button();
            this.cmdImport_DDR = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPartNo = new System.Windows.Forms.TextBox();
            this.cmbEndConfig_Front = new System.Windows.Forms.ComboBox();
            this.cmbSONo_Part1 = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbEndConfig_Back = new System.Windows.Forms.ComboBox();
            this.grpSalesType = new System.Windows.Forms.GroupBox();
            this.optOrder = new System.Windows.Forms.RadioButton();
            this.optProposal = new System.Windows.Forms.RadioButton();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSONo_Part1 = new System.Windows.Forms.MaskedTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSONo_Part3 = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbUnitSystem = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLabel4 = new System.Windows.Forms.Label();
            this.txtSONo_Part2 = new System.Windows.Forms.MaskedTextBox();
            this.txtRelatedSONo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.grpCustomer1 = new System.Windows.Forms.GroupBox();
            this.txtCustName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCustMachineName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCustOrderNo = new System.Windows.Forms.TextBox();
            this.lblSplitter1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.cmbDesign = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpSalesType.SuspendLayout();
            this.grpCustomer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBorder.BackColor = System.Drawing.Color.Black;
            this.lblBorder.Location = new System.Drawing.Point(1, 1);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(522, 532);
            this.lblBorder.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.grpCustomer1);
            this.panel1.Controls.Add(this.lblSplitter1);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.cmbDesign);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.cmbProduct);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 530);
            this.panel1.TabIndex = 7;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmdImport_XLRadial);
            this.groupBox3.Controls.Add(this.cmdImport_DDR);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(249, 32);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(252, 60);
            this.groupBox3.TabIndex = 622;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Import Data:";
            // 
            // cmdImport_XLRadial
            // 
            this.cmdImport_XLRadial.BackColor = System.Drawing.Color.Silver;
            this.cmdImport_XLRadial.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdImport_XLRadial.Image = ((System.Drawing.Image)(resources.GetObject("cmdImport_XLRadial.Image")));
            this.cmdImport_XLRadial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdImport_XLRadial.Location = new System.Drawing.Point(134, 19);
            this.cmdImport_XLRadial.Name = "cmdImport_XLRadial";
            this.cmdImport_XLRadial.Size = new System.Drawing.Size(101, 32);
            this.cmdImport_XLRadial.TabIndex = 610;
            this.cmdImport_XLRadial.Text = "    &XLRADIAL ";
            this.cmdImport_XLRadial.UseVisualStyleBackColor = false;
            this.cmdImport_XLRadial.Click += new System.EventHandler(this.cmdImport_XLRadial_Click);
            // 
            // cmdImport_DDR
            // 
            this.cmdImport_DDR.BackColor = System.Drawing.Color.Silver;
            this.cmdImport_DDR.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdImport_DDR.Image = ((System.Drawing.Image)(resources.GetObject("cmdImport_DDR.Image")));
            this.cmdImport_DDR.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdImport_DDR.Location = new System.Drawing.Point(17, 19);
            this.cmdImport_DDR.Name = "cmdImport_DDR";
            this.cmdImport_DDR.Size = new System.Drawing.Size(101, 32);
            this.cmdImport_DDR.TabIndex = 609;
            this.cmdImport_DDR.Text = "&DDR";
            this.cmdImport_DDR.UseVisualStyleBackColor = false;
            this.cmdImport_DDR.Click += new System.EventHandler(this.cmdImport_DDR_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 366);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(478, 2);
            this.label4.TabIndex = 621;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPartNo);
            this.groupBox2.Controls.Add(this.cmbEndConfig_Front);
            this.groupBox2.Controls.Add(this.cmbSONo_Part1);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.cmbEndConfig_Back);
            this.groupBox2.Controls.Add(this.grpSalesType);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtSONo_Part1);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtSONo_Part3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbUnitSystem);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblLabel4);
            this.groupBox2.Controls.Add(this.txtSONo_Part2);
            this.groupBox2.Controls.Add(this.txtRelatedSONo);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(20, 247);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 227);
            this.groupBox2.TabIndex = 619;
            this.groupBox2.TabStop = false;
            // 
            // txtPartNo
            // 
            this.txtPartNo.BackColor = System.Drawing.SystemColors.Window;
            this.txtPartNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartNo.ForeColor = System.Drawing.Color.Black;
            this.txtPartNo.Location = new System.Drawing.Point(165, 142);
            this.txtPartNo.Name = "txtPartNo";
            this.txtPartNo.Size = new System.Drawing.Size(90, 21);
            this.txtPartNo.TabIndex = 623;
            // 
            // cmbEndConfig_Front
            // 
            this.cmbEndConfig_Front.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cmbEndConfig_Front.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndConfig_Front.Enabled = false;
            this.cmbEndConfig_Front.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEndConfig_Front.FormattingEnabled = true;
            this.cmbEndConfig_Front.Location = new System.Drawing.Point(163, 190);
            this.cmbEndConfig_Front.Name = "cmbEndConfig_Front";
            this.cmbEndConfig_Front.Size = new System.Drawing.Size(69, 21);
            this.cmbEndConfig_Front.TabIndex = 327;
            this.cmbEndConfig_Front.SelectedIndexChanged += new System.EventHandler(this.cmbEndConfig_SelectedIndexChanged);
            // 
            // cmbSONo_Part1
            // 
            this.cmbSONo_Part1.BackColor = System.Drawing.Color.White;
            this.cmbSONo_Part1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSONo_Part1.FormattingEnabled = true;
            this.cmbSONo_Part1.Location = new System.Drawing.Point(130, 84);
            this.cmbSONo_Part1.Name = "cmbSONo_Part1";
            this.cmbSONo_Part1.Size = new System.Drawing.Size(42, 21);
            this.cmbSONo_Part1.TabIndex = 619;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label15.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(97, 194);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 326;
            this.label15.Text = "End Plate";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbEndConfig_Back
            // 
            this.cmbEndConfig_Back.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cmbEndConfig_Back.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndConfig_Back.Enabled = false;
            this.cmbEndConfig_Back.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEndConfig_Back.FormattingEnabled = true;
            this.cmbEndConfig_Back.Items.AddRange(new object[] {
            "End Seal",
            "T/L Thrust Bearing"});
            this.cmbEndConfig_Back.Location = new System.Drawing.Point(279, 190);
            this.cmbEndConfig_Back.Name = "cmbEndConfig_Back";
            this.cmbEndConfig_Back.Size = new System.Drawing.Size(69, 21);
            this.cmbEndConfig_Back.TabIndex = 328;
            this.cmbEndConfig_Back.SelectedIndexChanged += new System.EventHandler(this.cmbEndConfig_SelectedIndexChanged);
            // 
            // grpSalesType
            // 
            this.grpSalesType.Controls.Add(this.optOrder);
            this.grpSalesType.Controls.Add(this.optProposal);
            this.grpSalesType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSalesType.ForeColor = System.Drawing.Color.Black;
            this.grpSalesType.Location = new System.Drawing.Point(43, 17);
            this.grpSalesType.Name = "grpSalesType";
            this.grpSalesType.Size = new System.Drawing.Size(171, 46);
            this.grpSalesType.TabIndex = 612;
            this.grpSalesType.TabStop = false;
            this.grpSalesType.Text = "Sales Type:";
            // 
            // optOrder
            // 
            this.optOrder.AutoSize = true;
            this.optOrder.Checked = true;
            this.optOrder.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optOrder.Location = new System.Drawing.Point(19, 20);
            this.optOrder.Name = "optOrder";
            this.optOrder.Size = new System.Drawing.Size(58, 17);
            this.optOrder.TabIndex = 615;
            this.optOrder.TabStop = true;
            this.optOrder.Text = "Order";
            this.optOrder.UseVisualStyleBackColor = true;
            this.optOrder.CheckedChanged += new System.EventHandler(this.optButton_CheckedChanged);
            // 
            // optProposal
            // 
            this.optProposal.AutoSize = true;
            this.optProposal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optProposal.Location = new System.Drawing.Point(80, 21);
            this.optProposal.Name = "optProposal";
            this.optProposal.Size = new System.Drawing.Size(74, 17);
            this.optProposal.TabIndex = 614;
            this.optProposal.Text = "Proposal";
            this.optProposal.UseVisualStyleBackColor = true;
            this.optProposal.CheckedChanged += new System.EventHandler(this.optButton_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(179, 173);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 13);
            this.label18.TabIndex = 329;
            this.label18.Text = "Front";
            this.label18.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtSONo_Part1
            // 
            this.txtSONo_Part1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSONo_Part1.Location = new System.Drawing.Point(140, 84);
            this.txtSONo_Part1.Mask = "LL";
            this.txtSONo_Part1.Name = "txtSONo_Part1";
            this.txtSONo_Part1.PromptChar = 'x';
            this.txtSONo_Part1.Size = new System.Drawing.Size(22, 21);
            this.txtSONo_Part1.TabIndex = 618;
            this.txtSONo_Part1.Text = "SA";
            this.txtSONo_Part1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label19.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(296, 173);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 13);
            this.label19.TabIndex = 330;
            this.label19.Text = "Back";
            this.label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 259;
            this.label1.Text = "Sales Order No.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSONo_Part3
            // 
            this.txtSONo_Part3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSONo_Part3.Location = new System.Drawing.Point(233, 84);
            this.txtSONo_Part3.Mask = "00";
            this.txtSONo_Part3.Name = "txtSONo_Part3";
            this.txtSONo_Part3.Size = new System.Drawing.Size(22, 21);
            this.txtSONo_Part3.TabIndex = 617;
            this.txtSONo_Part3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(104, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 604;
            this.label6.Text = "Part No.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbUnitSystem
            // 
            this.cmbUnitSystem.BackColor = System.Drawing.Color.White;
            this.cmbUnitSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnitSystem.Enabled = false;
            this.cmbUnitSystem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUnitSystem.FormattingEnabled = true;
            this.cmbUnitSystem.Items.AddRange(new object[] {
            "English",
            "Metric"});
            this.cmbUnitSystem.Location = new System.Drawing.Point(279, 142);
            this.cmbUnitSystem.Name = "cmbUnitSystem";
            this.cmbUnitSystem.Size = new System.Drawing.Size(69, 21);
            this.cmbUnitSystem.TabIndex = 5;
            this.cmbUnitSystem.SelectedIndexChanged += new System.EventHandler(this.cmbUnit_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(299, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 263;
            this.label3.Text = "Unit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLabel4
            // 
            this.lblLabel4.AutoSize = true;
            this.lblLabel4.Location = new System.Drawing.Point(221, 87);
            this.lblLabel4.Name = "lblLabel4";
            this.lblLabel4.Size = new System.Drawing.Size(11, 14);
            this.lblLabel4.TabIndex = 616;
            this.lblLabel4.Text = "-";
            // 
            // txtSONo_Part2
            // 
            this.txtSONo_Part2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSONo_Part2.Location = new System.Drawing.Point(179, 84);
            this.txtSONo_Part2.Mask = "00000";
            this.txtSONo_Part2.Name = "txtSONo_Part2";
            this.txtSONo_Part2.Size = new System.Drawing.Size(42, 21);
            this.txtSONo_Part2.TabIndex = 615;
            this.txtSONo_Part2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRelatedSONo
            // 
            this.txtRelatedSONo.BackColor = System.Drawing.SystemColors.Window;
            this.txtRelatedSONo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRelatedSONo.ForeColor = System.Drawing.Color.Black;
            this.txtRelatedSONo.Location = new System.Drawing.Point(279, 84);
            this.txtRelatedSONo.Name = "txtRelatedSONo";
            this.txtRelatedSONo.Size = new System.Drawing.Size(164, 21);
            this.txtRelatedSONo.TabIndex = 607;
            this.txtRelatedSONo.MouseEnter += new System.EventHandler(this.txtRelatedSONo_MouseEnter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(309, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 608;
            this.label10.Text = "Related SO No.";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // grpCustomer1
            // 
            this.grpCustomer1.Controls.Add(this.txtCustName);
            this.grpCustomer1.Controls.Add(this.label5);
            this.grpCustomer1.Controls.Add(this.label7);
            this.grpCustomer1.Controls.Add(this.txtCustMachineName);
            this.grpCustomer1.Controls.Add(this.label8);
            this.grpCustomer1.Controls.Add(this.txtCustOrderNo);
            this.grpCustomer1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCustomer1.Location = new System.Drawing.Point(19, 114);
            this.grpCustomer1.Name = "grpCustomer1";
            this.grpCustomer1.Size = new System.Drawing.Size(482, 130);
            this.grpCustomer1.TabIndex = 601;
            this.grpCustomer1.TabStop = false;
            this.grpCustomer1.Text = "Customer:";
            // 
            // txtCustName
            // 
            this.txtCustName.BackColor = System.Drawing.SystemColors.Window;
            this.txtCustName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustName.ForeColor = System.Drawing.Color.Black;
            this.txtCustName.Location = new System.Drawing.Point(131, 26);
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.Size = new System.Drawing.Size(233, 21);
            this.txtCustName.TabIndex = 608;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(85, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 477;
            this.label5.Text = "Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(62, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 475;
            this.label7.Text = "Order No.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustMachineName
            // 
            this.txtCustMachineName.BackColor = System.Drawing.SystemColors.Window;
            this.txtCustMachineName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustMachineName.ForeColor = System.Drawing.Color.Black;
            this.txtCustMachineName.Location = new System.Drawing.Point(131, 93);
            this.txtCustMachineName.Name = "txtCustMachineName";
            this.txtCustMachineName.Size = new System.Drawing.Size(233, 21);
            this.txtCustMachineName.TabIndex = 6;
            this.txtCustMachineName.MouseEnter += new System.EventHandler(this.txtCustMachineName_MouseEnter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(35, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Machine Name";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCustOrderNo
            // 
            this.txtCustOrderNo.BackColor = System.Drawing.SystemColors.Window;
            this.txtCustOrderNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustOrderNo.ForeColor = System.Drawing.Color.Black;
            this.txtCustOrderNo.Location = new System.Drawing.Point(131, 59);
            this.txtCustOrderNo.Name = "txtCustOrderNo";
            this.txtCustOrderNo.Size = new System.Drawing.Size(233, 21);
            this.txtCustOrderNo.TabIndex = 3;
            this.txtCustOrderNo.MouseEnter += new System.EventHandler(this.txtCustOrderNo_MouseEnter);
            // 
            // lblSplitter1
            // 
            this.lblSplitter1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblSplitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSplitter1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSplitter1.Location = new System.Drawing.Point(0, 104);
            this.lblSplitter1.Name = "lblSplitter1";
            this.lblSplitter1.Size = new System.Drawing.Size(750, 2);
            this.lblSplitter1.TabIndex = 599;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.SteelBlue;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAdd,
            this.tsbEdit,
            this.tsbSave,
            this.tsbDelete,
            this.tsbCopy});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(518, 25);
            this.toolStrip1.TabIndex = 589;
            this.toolStrip1.Text = "ToolStrip1";
            // 
            // tsbAdd
            // 
            this.tsbAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAdd.Enabled = false;
            this.tsbAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbAdd.Image")));
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(23, 22);
            this.tsbAdd.Text = "ToolStripButton1";
            this.tsbAdd.ToolTipText = "Add";
            // 
            // tsbEdit
            // 
            this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEdit.Enabled = false;
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(23, 22);
            this.tsbEdit.Text = "Edit";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Enabled = false;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Save";
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Enabled = false;
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(23, 22);
            this.tsbDelete.Text = "Delete";
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Enabled = false;
            this.tsbCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopy.Image")));
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "Copy";
            // 
            // cmbDesign
            // 
            this.cmbDesign.BackColor = System.Drawing.Color.White;
            this.cmbDesign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDesign.Enabled = false;
            this.cmbDesign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDesign.FormattingEnabled = true;
            this.cmbDesign.Location = new System.Drawing.Point(119, 65);
            this.cmbDesign.Name = "cmbDesign";
            this.cmbDesign.Size = new System.Drawing.Size(107, 21);
            this.cmbDesign.TabIndex = 1;
            this.cmbDesign.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(149, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 13);
            this.label14.TabIndex = 300;
            this.label14.Text = "Design";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbProduct
            // 
            this.cmbProduct.BackColor = System.Drawing.Color.White;
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.Enabled = false;
            this.cmbProduct.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(19, 65);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(77, 21);
            this.cmbProduct.TabIndex = 0;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(32, 49);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 298;
            this.label13.Text = "Product";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdCancel
            // 
            this.cmdCancel.BackColor = System.Drawing.Color.Silver;
            this.cmdCancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(421, 490);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 32);
            this.cmdCancel.TabIndex = 278;
            this.cmdCancel.Text = " &Cancel";
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.BackColor = System.Drawing.Color.Silver;
            this.cmdOK.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Location = new System.Drawing.Point(323, 490);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 32);
            this.cmdOK.TabIndex = 277;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = false;
            this.cmdOK.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolTip1
            // 
            this.toolTip1.ForeColor = System.Drawing.Color.Blue;
            // 
            // frmProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(524, 536);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBorder);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Details";
            this.Activated += new System.EventHandler(this.frmProject_Activated);
            this.Load += new System.EventHandler(this.frmProject_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpSalesType.ResumeLayout(false);
            this.grpSalesType.PerformLayout();
            this.grpCustomer1.ResumeLayout(false);
            this.grpCustomer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbUnitSystem;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ComboBox cmbProduct;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbDesign;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label15;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cmbEndConfig_Back;
        private System.Windows.Forms.ComboBox cmbEndConfig_Front;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.GroupBox grpCustomer1;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox txtCustMachineName;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtCustOrderNo;
        internal System.Windows.Forms.Label lblSplitter1;
        internal System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.ToolStripButton tsbAdd;
        internal System.Windows.Forms.ToolStripButton tsbEdit;
        internal System.Windows.Forms.ToolStripButton tsbSave;
        internal System.Windows.Forms.ToolStripButton tsbDelete;
        internal System.Windows.Forms.ToolStripButton tsbCopy;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.TextBox txtRelatedSONo;
        private System.Windows.Forms.Button cmdImport_DDR;
        private System.Windows.Forms.GroupBox grpSalesType;
        private System.Windows.Forms.RadioButton optOrder;
        private System.Windows.Forms.RadioButton optProposal;
        internal System.Windows.Forms.MaskedTextBox txtSONo_Part3;
        internal System.Windows.Forms.Label lblLabel4;
        internal System.Windows.Forms.MaskedTextBox txtSONo_Part2;
        internal System.Windows.Forms.MaskedTextBox txtSONo_Part1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbSONo_Part1;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button cmdImport_XLRadial;
        private System.Windows.Forms.ToolTip toolTip2;
        internal System.Windows.Forms.TextBox txtPartNo;
        internal System.Windows.Forms.TextBox txtCustName;
        private System.Windows.Forms.ToolTip toolTip3;
    }
}