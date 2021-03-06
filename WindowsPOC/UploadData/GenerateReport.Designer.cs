﻿namespace WindowsPOC
{
    partial class GenerateReport
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
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.lblUploadEmpDetails = new System.Windows.Forms.Label();
            this.lblUploadBillingDetails = new System.Windows.Forms.Label();
            this.btnUploadEmp = new System.Windows.Forms.Button();
            this.txtEmpDetailsExcellName = new System.Windows.Forms.TextBox();
            this.txtUploadBillingName = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblBillingCycle = new System.Windows.Forms.Label();
            this.cmbBillingCycle = new System.Windows.Forms.ComboBox();
            this.cmbAccountName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnUploadBilling = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.btnDisplayEmployeeModel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReport.Location = new System.Drawing.Point(509, 8);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(133, 27);
            this.btnGenerateReport.TabIndex = 0;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // lblUploadEmpDetails
            // 
            this.lblUploadEmpDetails.AutoSize = true;
            this.lblUploadEmpDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadEmpDetails.Location = new System.Drawing.Point(21, 126);
            this.lblUploadEmpDetails.Name = "lblUploadEmpDetails";
            this.lblUploadEmpDetails.Size = new System.Drawing.Size(108, 13);
            this.lblUploadEmpDetails.TabIndex = 1;
            this.lblUploadEmpDetails.Text = "Upload Cost File :";
            // 
            // lblUploadBillingDetails
            // 
            this.lblUploadBillingDetails.AutoSize = true;
            this.lblUploadBillingDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadBillingDetails.Location = new System.Drawing.Point(12, 167);
            this.lblUploadBillingDetails.Name = "lblUploadBillingDetails";
            this.lblUploadBillingDetails.Size = new System.Drawing.Size(117, 13);
            this.lblUploadBillingDetails.TabIndex = 2;
            this.lblUploadBillingDetails.Text = "Upload Billing File :";
            // 
            // btnUploadEmp
            // 
            this.btnUploadEmp.Location = new System.Drawing.Point(424, 121);
            this.btnUploadEmp.Name = "btnUploadEmp";
            this.btnUploadEmp.Size = new System.Drawing.Size(25, 23);
            this.btnUploadEmp.TabIndex = 3;
            this.btnUploadEmp.Text = "...";
            this.btnUploadEmp.UseVisualStyleBackColor = true;
            this.btnUploadEmp.Click += new System.EventHandler(this.btnUploadEmp_Click);
            // 
            // txtEmpDetailsExcellName
            // 
            this.txtEmpDetailsExcellName.Location = new System.Drawing.Point(173, 121);
            this.txtEmpDetailsExcellName.Name = "txtEmpDetailsExcellName";
            this.txtEmpDetailsExcellName.ReadOnly = true;
            this.txtEmpDetailsExcellName.Size = new System.Drawing.Size(245, 20);
            this.txtEmpDetailsExcellName.TabIndex = 4;
            // 
            // txtUploadBillingName
            // 
            this.txtUploadBillingName.Location = new System.Drawing.Point(173, 159);
            this.txtUploadBillingName.Name = "txtUploadBillingName";
            this.txtUploadBillingName.ReadOnly = true;
            this.txtUploadBillingName.Size = new System.Drawing.Size(245, 20);
            this.txtUploadBillingName.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(666, 319);
            this.dataGridView1.TabIndex = 7;
            // 
            // lblBillingCycle
            // 
            this.lblBillingCycle.AutoSize = true;
            this.lblBillingCycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingCycle.Location = new System.Drawing.Point(41, 86);
            this.lblBillingCycle.Name = "lblBillingCycle";
            this.lblBillingCycle.Size = new System.Drawing.Size(88, 13);
            this.lblBillingCycle.TabIndex = 8;
            this.lblBillingCycle.Text = "Billing Cycle : ";
            // 
            // cmbBillingCycle
            // 
            this.cmbBillingCycle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBillingCycle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBillingCycle.Location = new System.Drawing.Point(173, 83);
            this.cmbBillingCycle.Name = "cmbBillingCycle";
            this.cmbBillingCycle.Size = new System.Drawing.Size(269, 21);
            this.cmbBillingCycle.TabIndex = 9;
            this.cmbBillingCycle.SelectedIndexChanged += new System.EventHandler(this.cmbBillingCycle_SelectedIndexChanged);
            // 
            // cmbAccountName
            // 
            this.cmbAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAccountName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountName.Location = new System.Drawing.Point(173, 12);
            this.cmbAccountName.Name = "cmbAccountName";
            this.cmbAccountName.Size = new System.Drawing.Size(269, 21);
            this.cmbAccountName.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Account Name :";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportExcel.Location = new System.Drawing.Point(552, 167);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(91, 27);
            this.btnExportExcel.TabIndex = 12;
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportPdf.Location = new System.Drawing.Point(470, 167);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(76, 27);
            this.btnExportPdf.TabIndex = 14;
            this.btnExportPdf.Text = "Export to Pdf";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // btnUploadBilling
            // 
            this.btnUploadBilling.Location = new System.Drawing.Point(424, 160);
            this.btnUploadBilling.Name = "btnUploadBilling";
            this.btnUploadBilling.Size = new System.Drawing.Size(25, 23);
            this.btnUploadBilling.TabIndex = 6;
            this.btnUploadBilling.Text = "...";
            this.btnUploadBilling.UseVisualStyleBackColor = true;
            this.btnUploadBilling.Click += new System.EventHandler(this.btnUploadBilling_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.cmbYear);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnDisplayEmployeeModel);
            this.splitContainer1.Panel1.Controls.Add(this.lblBillingCycle);
            this.splitContainer1.Panel1.Controls.Add(this.btnClear);
            this.splitContainer1.Panel1.Controls.Add(this.lblUploadBillingDetails);
            this.splitContainer1.Panel1.Controls.Add(this.lblUploadEmpDetails);
            this.splitContainer1.Panel1.Controls.Add(this.btnExportPdf);
            this.splitContainer1.Panel1.Controls.Add(this.btnExportExcel);
            this.splitContainer1.Panel1.Controls.Add(this.txtEmpDetailsExcellName);
            this.splitContainer1.Panel1.Controls.Add(this.btnUploadEmp);
            this.splitContainer1.Panel1.Controls.Add(this.cmbAccountName);
            this.splitContainer1.Panel1.Controls.Add(this.cmbBillingCycle);
            this.splitContainer1.Panel1.Controls.Add(this.btnGenerateReport);
            this.splitContainer1.Panel1.Controls.Add(this.btnUploadBilling);
            this.splitContainer1.Panel1.Controls.Add(this.txtUploadBillingName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(666, 519);
            this.splitContainer1.SplitterDistance = 196;
            this.splitContainer1.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(41, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Periodic Year : ";
            // 
            // cmbYear
            // 
            this.cmbYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.Location = new System.Drawing.Point(173, 48);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(269, 21);
            this.cmbYear.TabIndex = 18;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            // 
            // btnDisplayEmployeeModel
            // 
            this.btnDisplayEmployeeModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayEmployeeModel.Location = new System.Drawing.Point(510, 86);
            this.btnDisplayEmployeeModel.Name = "btnDisplayEmployeeModel";
            this.btnDisplayEmployeeModel.Size = new System.Drawing.Size(133, 27);
            this.btnDisplayEmployeeModel.TabIndex = 17;
            this.btnDisplayEmployeeModel.Text = "Show Employee Data";
            this.btnDisplayEmployeeModel.UseVisualStyleBackColor = true;
            this.btnDisplayEmployeeModel.Click += new System.EventHandler(this.btnDisplayEmployeeModel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(510, 44);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(133, 27);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear All Data";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // GenerateReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(666, 519);
            this.Controls.Add(this.splitContainer1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateReport";
            this.Text = "Add Monthly Account Data";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GenerateReport_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Label lblUploadEmpDetails;
        private System.Windows.Forms.Label lblUploadBillingDetails;
        private System.Windows.Forms.Button btnUploadEmp;
        private System.Windows.Forms.TextBox txtEmpDetailsExcellName;
        private System.Windows.Forms.TextBox txtUploadBillingName;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblBillingCycle;
        private System.Windows.Forms.ComboBox cmbBillingCycle;
        private System.Windows.Forms.ComboBox cmbAccountName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnUploadBilling;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDisplayEmployeeModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
    }
}

