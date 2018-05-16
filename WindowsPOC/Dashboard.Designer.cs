namespace WindowsPOC
{
    partial class Dashboard
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMonthlyDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createBusinessUnitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapAccountsToBusinessUnitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monthlyDataConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.costFileMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revenueFileMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCurrencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managerWiseRevenueReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupWiseReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountWiseReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marginPerPersonYTDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personMarginReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salaryBasedReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.DefaultGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.addMonthlyDataToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.reportsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(706, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.homeToolStripMenuItem.Text = "Home";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // addMonthlyDataToolStripMenuItem
            // 
            this.addMonthlyDataToolStripMenuItem.Name = "addMonthlyDataToolStripMenuItem";
            this.addMonthlyDataToolStripMenuItem.Size = new System.Drawing.Size(116, 20);
            this.addMonthlyDataToolStripMenuItem.Text = "Add Monthly Data";
            this.addMonthlyDataToolStripMenuItem.Click += new System.EventHandler(this.addMonthlyDataToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createAccountToolStripMenuItem,
            this.createBusinessUnitsToolStripMenuItem,
            this.mapAccountsToBusinessUnitsToolStripMenuItem,
            this.monthlyDataConfigurationToolStripMenuItem,
            this.costFileMappingToolStripMenuItem,
            this.revenueFileMappingToolStripMenuItem,
            this.addCurrencyToolStripMenuItem,
            this.verticalMappingToolStripMenuItem,
            this.reportConfigurationToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // createAccountToolStripMenuItem
            // 
            this.createAccountToolStripMenuItem.Name = "createAccountToolStripMenuItem";
            this.createAccountToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.createAccountToolStripMenuItem.Text = "Create Account";
            this.createAccountToolStripMenuItem.Click += new System.EventHandler(this.createAccountToolStripMenuItem_Click);
            // 
            // createBusinessUnitsToolStripMenuItem
            // 
            this.createBusinessUnitsToolStripMenuItem.Name = "createBusinessUnitsToolStripMenuItem";
            this.createBusinessUnitsToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.createBusinessUnitsToolStripMenuItem.Text = "Create Business Units";
            this.createBusinessUnitsToolStripMenuItem.Click += new System.EventHandler(this.createBusinessUnitsToolStripMenuItem_Click);
            // 
            // mapAccountsToBusinessUnitsToolStripMenuItem
            // 
            this.mapAccountsToBusinessUnitsToolStripMenuItem.Name = "mapAccountsToBusinessUnitsToolStripMenuItem";
            this.mapAccountsToBusinessUnitsToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.mapAccountsToBusinessUnitsToolStripMenuItem.Text = "Map Accounts to Business Units";
            this.mapAccountsToBusinessUnitsToolStripMenuItem.Click += new System.EventHandler(this.mapAccountsToBusinessUnitsToolStripMenuItem_Click);
            // 
            // monthlyDataConfigurationToolStripMenuItem
            // 
            this.monthlyDataConfigurationToolStripMenuItem.Name = "monthlyDataConfigurationToolStripMenuItem";
            this.monthlyDataConfigurationToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.monthlyDataConfigurationToolStripMenuItem.Text = "Monthly Data Configuration";
            // 
            // costFileMappingToolStripMenuItem
            // 
            this.costFileMappingToolStripMenuItem.Name = "costFileMappingToolStripMenuItem";
            this.costFileMappingToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.costFileMappingToolStripMenuItem.Text = "Cost File Mapping";
            // 
            // revenueFileMappingToolStripMenuItem
            // 
            this.revenueFileMappingToolStripMenuItem.Name = "revenueFileMappingToolStripMenuItem";
            this.revenueFileMappingToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.revenueFileMappingToolStripMenuItem.Text = "Revenue File Mapping";
            // 
            // addCurrencyToolStripMenuItem
            // 
            this.addCurrencyToolStripMenuItem.Name = "addCurrencyToolStripMenuItem";
            this.addCurrencyToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.addCurrencyToolStripMenuItem.Text = "Add Currency";
            // 
            // verticalMappingToolStripMenuItem
            // 
            this.verticalMappingToolStripMenuItem.Name = "verticalMappingToolStripMenuItem";
            this.verticalMappingToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.verticalMappingToolStripMenuItem.Text = "Vertical Mapping";
            // 
            // reportConfigurationToolStripMenuItem
            // 
            this.reportConfigurationToolStripMenuItem.Name = "reportConfigurationToolStripMenuItem";
            this.reportConfigurationToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.reportConfigurationToolStripMenuItem.Text = "Report Configuration";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managerWiseRevenueReportToolStripMenuItem,
            this.groupWiseReportToolStripMenuItem,
            this.accountWiseReportToolStripMenuItem,
            this.marginPerPersonYTDToolStripMenuItem,
            this.personMarginReportToolStripMenuItem,
            this.salaryBasedReportToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // managerWiseRevenueReportToolStripMenuItem
            // 
            this.managerWiseRevenueReportToolStripMenuItem.Name = "managerWiseRevenueReportToolStripMenuItem";
            this.managerWiseRevenueReportToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.managerWiseRevenueReportToolStripMenuItem.Text = "Manager wise report";
            this.managerWiseRevenueReportToolStripMenuItem.Click += new System.EventHandler(this.managerWiseRevenueReportToolStripMenuItem_Click);
            // 
            // groupWiseReportToolStripMenuItem
            // 
            this.groupWiseReportToolStripMenuItem.Name = "groupWiseReportToolStripMenuItem";
            this.groupWiseReportToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.groupWiseReportToolStripMenuItem.Text = "Group wise Report";
            this.groupWiseReportToolStripMenuItem.Click += new System.EventHandler(this.groupWiseReportToolStripMenuItem_Click);
            // 
            // accountWiseReportToolStripMenuItem
            // 
            this.accountWiseReportToolStripMenuItem.Name = "accountWiseReportToolStripMenuItem";
            this.accountWiseReportToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.accountWiseReportToolStripMenuItem.Text = "Account wise Report";
            this.accountWiseReportToolStripMenuItem.Click += new System.EventHandler(this.accountWiseReportToolStripMenuItem_Click);
            // 
            // marginPerPersonYTDToolStripMenuItem
            // 
            this.marginPerPersonYTDToolStripMenuItem.Name = "marginPerPersonYTDToolStripMenuItem";
            this.marginPerPersonYTDToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.marginPerPersonYTDToolStripMenuItem.Text = "Margin Per Person YTD";
            this.marginPerPersonYTDToolStripMenuItem.Click += new System.EventHandler(this.marginPerPersonYTDToolStripMenuItem_Click);
            // 
            // personMarginReportToolStripMenuItem
            // 
            this.personMarginReportToolStripMenuItem.Name = "personMarginReportToolStripMenuItem";
            this.personMarginReportToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.personMarginReportToolStripMenuItem.Text = "Person Margin Report";
            this.personMarginReportToolStripMenuItem.Click += new System.EventHandler(this.personMarginReportToolStripMenuItem_Click);
            // 
            // salaryBasedReportToolStripMenuItem
            // 
            this.salaryBasedReportToolStripMenuItem.Name = "salaryBasedReportToolStripMenuItem";
            this.salaryBasedReportToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.salaryBasedReportToolStripMenuItem.Text = "Salary Based Report";
            this.salaryBasedReportToolStripMenuItem.Click += new System.EventHandler(this.salaryBasedReportToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(706, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // DefaultGridView
            // 
            this.DefaultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DefaultGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DefaultGridView.Location = new System.Drawing.Point(0, 24);
            this.DefaultGridView.Name = "DefaultGridView";
            this.DefaultGridView.Size = new System.Drawing.Size(706, 407);
            this.DefaultGridView.TabIndex = 4;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 453);
            this.Controls.Add(this.DefaultGridView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMonthlyDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createBusinessUnitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapAccountsToBusinessUnitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monthlyDataConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem costFileMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revenueFileMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCurrencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticalMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managerWiseRevenueReportToolStripMenuItem;
        private System.Windows.Forms.DataGridView DefaultGridView;
        private System.Windows.Forms.ToolStripMenuItem groupWiseReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountWiseReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem marginPerPersonYTDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personMarginReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salaryBasedReportToolStripMenuItem;
    }
}



