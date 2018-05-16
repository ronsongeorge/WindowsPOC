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
    public partial class PersonReport : Form
    {
        public PersonReport()
        {
            InitializeComponent();
            LoadMonth();
            LoadYear();
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
            RadioButton rb = (RadioButton)sender;
            rbAccounts.Checked = !rb.Checked;
            if (rb.Checked)
                LoadBUAccountList(rbBUList);
            else
                LoadBUAccountList(rbAccounts);
        }

        private void LoadBUAccountList(RadioButton selectedRadio)
        {
            if (selectedRadio.Text == "BU")
            {
                BUModel bu = new BUModel();

                lstBUAcc.DataSource = bu.GetBUList();
                lstBUAcc.DisplayMember = "BUName";
                lstBUAcc.ValueMember = "BUID";
                lstBUAcc.SelectionMode = SelectionMode.One;
            }else
            {
                AccountModel accModel = new AccountModel();
                lstBUAcc.DataSource = accModel.GetAccountsList();
                lstBUAcc.DisplayMember = "AccountName";
                lstBUAcc.ValueMember = "AccountID";
                lstBUAcc.SelectionMode = SelectionMode.MultiSimple;
            }

        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedMonth = (List<string>)lstMonth.SelectedItems.Cast<String>().ToList();
                int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());
                int? BU = null;
                var AccountList = new List<Account>();
                if (rbBUList.Checked)
                {
                    BU = Convert.ToInt32(lstBUAcc.SelectedValue);
                }
                else
                {
                    foreach (Account item in lstBUAcc.SelectedItems)
                    {
                        Account a = new Account();
                        a.AccountID = Convert.ToInt32(item.AccountID);
                        AccountList.Add(a);
                    }
                }
                AccountMonthRevenue accMonthRevenue = new AccountMonthRevenue();
                DataSet ds;
                if (cmbReportType.SelectedItem.ToString() == "Highest Margin")
                {
                    ds = accMonthRevenue.GetPeopleWithHighestMargin(Convert.ToInt32(txtNoOfPeople.Text),BU, AccountList, selectedYear, selectedMonth);
                }
                else
                {
                    ds = accMonthRevenue.GetPeopleWithLowestMargin(Convert.ToInt32(txtNoOfPeople.Text),BU, AccountList, selectedYear, selectedMonth);
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
