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
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsPOC
{
    public partial class ManagerReport : Form
    {
        public ManagerReport()
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
                    ds = accMonthRevenue.ManagerWiseRevenueReport(BU, selectedMonth, AccountList, selectedYear);
                else
                    ds = accMonthRevenue.ManagerWiseMarginReport(BU, selectedMonth, AccountList, selectedYear);

                if (cmbChartType.SelectedItem.ToString() == "Grid")
                {
                    dgvReportView.AutoGenerateColumns = true;
                    dgvReportView.DataSource = ds.Tables[0];
                    dgvReportView.AutoResizeColumns();
                    dgvReportView.Show();
                }
                else 
                {
                    dgvReportView.Hide();
                    bool chartexists = splitContainer1.Panel2.Controls.ContainsKey("chart1");
                    Chart ch;
                    if (!chartexists)
                        ch = new Chart();
                    else
                        ch = (Chart)splitContainer1.Panel2.Controls["chart1"];

                    ch.Series.Clear();
                    System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                    System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
                    System.Windows.Forms.DataVisualization.Charting.Series series1;

                    if (cmbChartType.SelectedItem.ToString() == "Bar Chart")
                    {
                        series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                         {
                             Name = "Series1",
                             Color = System.Drawing.Color.Green,
                             IsVisibleInLegend = true,
                             IsXValueIndexed = true,
                             ChartType = SeriesChartType.Bar
                         };
                    }
                    else
                    {
                        series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "Series1",
                            Color = System.Drawing.Color.Green,
                            IsVisibleInLegend = true,
                            IsXValueIndexed = true,
                            ChartType = SeriesChartType.Pie
                        };
                    }

                    ch.Series.Add(series1);

                    ch.Legends.Add(legend1);
                    ch.Location = new System.Drawing.Point(0, 50);
                    ch.Name = "chart1";

                    ch.DataSource = ds.Tables[0];
                    ch.Series["Series1"].XValueMember = "ManagerName";

                    if (cmbReportType.SelectedItem.ToString() == "Revenue Report")
                        ch.Series["Series1"].YValueMembers = "Revenue";
                    else
                        ch.Series["Series1"].YValueMembers = "GrossMargin";
                    chartArea1.Name = "ChartArea1";
                    if (!chartexists)
                        ch.ChartAreas.Add(chartArea1);
                    ch.Dock = System.Windows.Forms.DockStyle.Fill;
                    ch.Series["Series1"].IsValueShownAsLabel = true;
                    if (!chartexists)
                        splitContainer1.Panel2.Controls.Add(ch);
                }
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
