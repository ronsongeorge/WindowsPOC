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
    public partial class GroupReport : Form
    {
        public GroupReport()
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

        private void LoadMonth()
        {
            for (int dtime = 5; dtime >= 0; dtime--)
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

        private void LoadBUAccountList()
        {
            AccountModel accModel = new AccountModel();
            lstBUAcc.DataSource = accModel.GetAccountsList();
            lstBUAcc.DisplayMember = "AccountName";
            lstBUAcc.ValueMember = "AccountID";
            lstBUAcc.SelectionMode = SelectionMode.MultiSimple;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedMonth = (List<string>)lstMonth.SelectedItems.Cast<String>().ToList();
                int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());
                int? BU = null;
                var AccountList = new List<Account>();
                foreach (Account item in lstBUAcc.SelectedItems)
                {
                    Account a = new Account();
                    a.AccountID = Convert.ToInt32(item.AccountID);
                    AccountList.Add(a);
                }
               
                AccountMonthRevenue accMonthRevenue = new AccountMonthRevenue();
                DataSet ds;
                if (cmbReportType.SelectedItem.ToString() == "Revenue Report")
                    ds = accMonthRevenue.GroupWiseRevenueReport(BU, selectedMonth, AccountList, selectedYear);
                else
                    ds = accMonthRevenue.GroupWiseMarginReport(BU, selectedMonth, AccountList, selectedYear);
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
