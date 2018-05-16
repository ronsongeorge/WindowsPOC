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
    public partial class AccountReport : Form
    {
        public AccountReport()
        {
            InitializeComponent();
            LoadMonth();
            LoadYear();
            LoadBUAccountList();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvReportView.DataSource = null;
            cmbYear.SelectedIndex = -1;            
            lstMonth.SelectedItems.Clear();     
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Report_Load(object sender, EventArgs e)
        {

        }

        private void LoadMonth()
        {
            for (int dtime = 5; dtime > 0; dtime--)
            {
                var getMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-dtime).Month).Substring(0, 3).ToUpper();
                lstMonth.Items.Add(getMonthName);
            }
        }

        private void LoadYear()
        {
            int years = DateTime.Now.Year;
            cmbYear.Items.Add(years - 1);
            cmbYear.Items.Add(years);
        }

        private void rbBUList_CheckedChanged(object sender, EventArgs e)
        {
              
        }

        private void LoadBUAccountList()
        {

                BUModel bu = new BUModel();
                lstBUAcc.DataSource = bu.GetBUList();
                lstBUAcc.DisplayMember = "BUName";
                lstBUAcc.ValueMember = "BUID";
                lstBUAcc.SelectionMode = SelectionMode.One;        

        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedMonth = (List<string>)lstMonth.SelectedItems.Cast<String>().ToList();
                int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());
                int BU = 0;
                var AccountList = new List<Account>();

                BU = Convert.ToInt32(lstBUAcc.SelectedValue);
               
                AccountMonthRevenue accMonthRevenue = new AccountMonthRevenue();
                DataSet ds;
                if (cmbReportType.SelectedItem.ToString() == "Revenue Report")
                {
                    ds = accMonthRevenue.AccountRevenueReportBasedOnBU(BU,selectedYear,selectedMonth);
                }
                else
                {
                    ds = accMonthRevenue.AccountMarginReportBasedOnBU(BU, selectedYear,selectedMonth);
                }
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
