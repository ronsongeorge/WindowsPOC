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
    public partial class SalaryReport : Form
    {
        public SalaryReport()
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
                if (cmbReportType.SelectedItem.ToString() == "Highest Salary")
                {
                    ds = accMonthRevenue.GetHighestSalary(Convert.ToInt32(txtFromNoofPeople.Text), Convert.ToInt32(txtToNoofPeople.Text)
                        , BU, AccountList, selectedYear);
                }
                else
                {
                    ds = accMonthRevenue.GetLowestSalary(Convert.ToInt32(txtFromNoofPeople.Text), Convert.ToInt32(txtToNoofPeople.Text)
                        , BU, AccountList, selectedYear);
                }
                dgvReportView.AutoGenerateColumns = true;
                dgvReportView.DataSource = ds.Tables[0];
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
