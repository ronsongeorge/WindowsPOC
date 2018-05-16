using DataLayer;
using EntitiesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsPOC
{
    public partial class MonthlyAccountUpload : Form
    {
        public MonthlyAccountUpload()
        {
            InitializeComponent();
            LoadYear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvReportView.DataSource = null;
            cmbYear.SelectedIndex = -1;         
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Report_Load(object sender, EventArgs e)
        {

        }

        
        private void LoadYear()
        {
            int years = DateTime.Now.Year;
            cmbYear.Items.Add(years - 1);
            cmbYear.Items.Add(years);
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {                
                int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());  
                AccountMonthRevenue accMonthRevenue = new AccountMonthRevenue();
                DataSet ds;
                ds = accMonthRevenue.MonthDataExists(Convert.ToInt32(cmbYear.Text));
               
                dgvReportView.AutoGenerateColumns = true;
                dgvReportView.DataSource = ds.Tables[0];
                dgvReportView.AutoResizeColumns();
                dgvReportView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while generating the report.\nPlease try again in sometime.");
            }
            
        }

        private void rbAccounts_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Report_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Dashboard")
                    frm.Show();
            }  
        }

    }
}
