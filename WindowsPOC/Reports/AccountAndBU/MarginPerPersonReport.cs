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
    public partial class MarginPerPersonReport : Form
    {
        public MarginPerPersonReport()
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
                ds = accMonthRevenue.MarginPerEmployeeYTD(BU, AccountList, selectedYear);

                dgvReportView.AutoGenerateColumns = true;
                dgvReportView.DataSource = ds.Tables[0];
                dgvReportView.AutoResizeColumns();
                dgvReportView.Show();

                //CallTestMethod();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while generating the report.\nPlease try again in sometime.");
            }
            
        }

        private void CallTestMethod()
        {
            //Parent table  
            DataTable dtstudent = new DataTable();
            // add column to datatable  
            dtstudent.Columns.Add("Student_ID", typeof(int));
            dtstudent.Columns.Add("Student_Name", typeof(string));
            dtstudent.Columns.Add("Student_RollNo", typeof(string));

            //Child table  
            DataTable dtstudentMarks = new DataTable();
            dtstudentMarks.Columns.Add("Student_ID", typeof(int));
            dtstudentMarks.Columns.Add("Subject_ID", typeof(int));
            dtstudentMarks.Columns.Add("Subject_Name", typeof(string));
            dtstudentMarks.Columns.Add("Marks", typeof(int));

            //Adding Rows  
            dtstudent.Rows.Add(111, "Devesh", "03021013014");
            dtstudent.Rows.Add(222, "ROLI", "0302101444");
            dtstudent.Rows.Add(333, "ROLI Ji", "030212222");
            dtstudent.Rows.Add(444, "NIKHIL", "KANPUR");

            // data for devesh ID=111  
            dtstudentMarks.Rows.Add(111, "01", "Physics", 99);
            dtstudentMarks.Rows.Add(111, "02", "Maths", 77);
            dtstudentMarks.Rows.Add(111, "03", "C#", 100);
            dtstudentMarks.Rows.Add(111, "01", "Physics", 99);


            //data for ROLI ID=222  
            dtstudentMarks.Rows.Add(222, "01", "Physics", 80);
            dtstudentMarks.Rows.Add(222, "02", "English", 95);
            dtstudentMarks.Rows.Add(222, "03", "Commerce", 95);
            dtstudentMarks.Rows.Add(222, "01", "BankPO", 99);

            DataSet dsDataset = new DataSet();
            //Add two DataTables in Dataset  
            dsDataset.Tables.Add(dtstudent);
            dsDataset.Tables.Add(dtstudentMarks);

            DataRelation Datatablerelation = new DataRelation("DetailsMarks", dsDataset.Tables[0].Columns[0], dsDataset.Tables[1].Columns[0], true);
            //DataRelation Datatablerelation = new DataRelation("CustOrd", dsDataset.Tables[0].Columns["Student_ID"], dsDataset.Tables[1].Columns["Student_ID"]);
            dsDataset.Relations.Add(Datatablerelation);
            dgvReportView.DataSource = dsDataset.Tables[0];  
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
