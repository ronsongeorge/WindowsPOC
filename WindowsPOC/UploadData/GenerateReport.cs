using BusinessLayer;
using DataLayer;
using EntitiesLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsPOC
{
    public partial class GenerateReport : Form
    {
        #region Properties

        ProcessFiles vemp;
        string costPath;
        string billingPath;
        public string selectedAccountName;
        List<ErrorMessage> errorMsgs;
        AccountModel amodel;

        #endregion

        public GenerateReport()
        {
            InitializeComponent();            
              
            FillAccountDropDown();  
            FillYearDropDown();
            FillBillingCycleDropDown();            
            DisableControls();
            InitializeDataBaseObjects();
            
        }

        private void InitializeDataBaseObjects()
        {
            //AccountMonthRevenue a = new AccountMonthRevenue();
            ////List<string> month = new List<string> { "JAN","DEC" };
            ////a.GetMonthwiseSingleAccountData(month,2018,"1");
            //Dictionary<string,int> mlist= new Dictionary<string,int>();
            //mlist.Add("FEB", 2018);
            ////mlist.Add("MAR", 2018);
            //a.GetMultipleMonthBUWiseData(mlist,1);

        }

        #region Events

        private void btnUploadEmp_Click(object sender, EventArgs e)
        {
            ValidateUploadedFileForExtension(txtEmpDetailsExcellName, ref costPath);
        }

        private void btnUploadBilling_Click(object sender, EventArgs e)
        {
            bool checkFileExists = true;
            if (txtEmpDetailsExcellName.Text == string.Empty)
            {
                checkFileExists = false;
                MessageBox.Show("Upload the employee cost sheet before proceeding");
            }
            if (checkFileExists)
                ValidateUploadedFileForExtension(txtUploadBillingName, ref billingPath);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
            cmbAccountName.SelectedIndex = -1;
            cmbBillingCycle.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
        }

        private void ClearData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();            
            txtEmpDetailsExcellName.Text = string.Empty;
            txtUploadBillingName.Text = string.Empty;
        }

        #endregion

        #region Generate Report Button Click Methods

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (CanGenerateReport())
            {
                vemp = new ProcessFiles();
                errorMsgs = new List<ErrorMessage>();
                var empData = vemp.ProcessEmpFiles(cmbBillingCycle.SelectedItem.ToString(),cmbYear.SelectedItem.ToString(),Convert.ToInt32(cmbAccountName.SelectedValue),costPath, billingPath, ref errorMsgs);

                if (errorMsgs.Count() > 0)
                {
                    var msgResult = MessageBox.Show("There are few discripencies in the file.\nDo you like to continue","Discripency Alert",MessageBoxButtons.YesNo);
                    if (msgResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        AccountMonthRevenue aMonthData = new AccountMonthRevenue();
                        bool isSuccessful = aMonthData.CreateAccountMonthWiseData(cmbBillingCycle.Text, Convert.ToInt32(cmbYear.Text), ((EntitiesLib.Account)(cmbAccountName.SelectedItem)).AccountID.ToString(), empData);
                        //FillReportData(empData);
                        dataGridView1.Visible = true;
                        dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                        DisableControls();
                        MessageBox.Show("Date generated successfully");
                    }else
                    {
                        ClearData();
                        dataGridView1.DefaultCellStyle.ForeColor = Color.Red;
                        dataGridView1.DataSource = errorMsgs;                    
                        dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        //DisableControls();
                        MessageBox.Show("Cannot proceed as discripencies found in data!");
                    }
                }else
                {
                    AccountMonthRevenue aMonthData = new AccountMonthRevenue();
                    bool isSuccessful = aMonthData.CreateAccountMonthWiseData(cmbBillingCycle.Text, Convert.ToInt32(cmbYear.Text), ((EntitiesLib.Account)(cmbAccountName.SelectedItem)).AccountID.ToString(), empData);
                    //FillReportData(empData);
                    dataGridView1.Visible = true;
                    dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                    DisableControls();
                    MessageBox.Show("Date generated successfully");
                }                
            }
        }

        private bool CanGenerateReport()
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (
                string.IsNullOrEmpty(txtEmpDetailsExcellName.Text) ||
                string.IsNullOrEmpty(txtUploadBillingName.Text) ||
                cmbBillingCycle.SelectedIndex == -1 ||
                cmbAccountName.SelectedIndex == -1
                )
            {
                if (cmbAccountName.SelectedIndex == -1)
                    ErrorMessage.Append("\nPlease select the account name to continue..");
                if (cmbBillingCycle.SelectedIndex == -1)
                    ErrorMessage.Append("\nPlease select the billing cycle to continue..");
                if (string.IsNullOrEmpty(txtEmpDetailsExcellName.Text))
                    ErrorMessage.Append("Please upload the employee details excel to continue..");
                if (string.IsNullOrEmpty(txtUploadBillingName.Text))
                    ErrorMessage.Append("\nPlease upload the billing details excel to continue..");

                if (!string.IsNullOrEmpty(ErrorMessage.ToString()))
                {
                    MessageBox.Show(ErrorMessage.ToString());
                    return false;
                }
            }

            return true;
        }

        private void FillReportData(List<EmployeeDetails> empData)
        {
            var excludedGroupNames = Properties.Settings.Default.GroupNameExclude.Split(',');
            var verticals = empData.Where(a => !excludedGroupNames.Contains(a.VerticalName)).Select(a => a.VerticalName).Distinct();
            CreateColumnsForGrid(verticals);
            UploadDataForSingleMonth(cmbBillingCycle.SelectedText, verticals, empData);
        }

        private void CreateColumnsForGrid(IEnumerable<string> VerticalNames)
        {
            if (dataGridView1.ColumnCount > 0)
            {
                dataGridView1.ColumnCount = dataGridView1.ColumnCount + 1;
                if (cmbBillingCycle.SelectedIndex != -1)
                    dataGridView1.Columns[dataGridView1.Columns.Count - 1].Name = cmbBillingCycle.SelectedItem.ToString();
            }
            else
            {
                dataGridView1.ColumnCount = 4;

                dataGridView1.Columns[0].Name = DateTime.Now.Year.ToString();
                dataGridView1.Columns[1].Name = " ";
                dataGridView1.Columns[2].Name = "AVERAGE";
                if (cmbBillingCycle.SelectedIndex != -1)
                    dataGridView1.Columns[dataGridView1.Columns.Count - 1].Name = cmbBillingCycle.SelectedItem.ToString();

                Particulars P = new Particulars();
                var s = P.GetAllParticulars();
                foreach (var l in s)
                {
                    int rowIndex = this.dataGridView1.Rows.Add();
                    var row = this.dataGridView1.Rows[rowIndex];
                    row.Cells[0].Value = l.ParticularsName;
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    foreach (var b in l.ParticularsSubTypes)
                    {
                        int rowIndex1 = this.dataGridView1.Rows.Add();
                        //Obtain a reference to the newly created DataGridViewRow 
                        var row1 = this.dataGridView1.Rows[rowIndex1];
                        row1.Cells[1].Value = b.SubTypeName;
                    }
                }

                var verticals = VerticalNames;
                if (verticals != null)
                {
                    foreach (var vert in verticals)
                    {
                        int rowIndex = this.dataGridView1.Rows.Add();
                        var row = this.dataGridView1.Rows[rowIndex];
                        row.Cells[1].Value = vert;
                        row.DefaultCellStyle.BackColor = Color.LightBlue;

                        string[] testarray = Properties.Settings.Default.VerticalsSubSection.Split(',');//{ "% of Rev", "Onsite % of Rev", "Offshore % of Rev", "Onsite GM", "Offshore GM" };
                        foreach (string b in testarray)
                        {
                            int rowIndex1 = this.dataGridView1.Rows.Add();
                            //Obtain a reference to the newly created DataGridViewRow 
                            var row1 = this.dataGridView1.Rows[rowIndex1];
                            row1.Cells[1].Value = b;
                        }
                    }
                }
            }
        }

        private void UploadDataForSingleMonth(string selectedMonth, IEnumerable<string> verticals, List<EmployeeDetails> empData)
        {
            SetDefaultData(verticals);
            CalculateDataUsingFormula(empData, verticals);
        }

        private void SetDefaultData(IEnumerable<string> verticals)
        {
            string setcellName = "AVERAGE";
            int defaultVal = 0;
            string setCmbVal = cmbBillingCycle.SelectedItem.ToString();
            Particulars p = new Particulars();
            var particulars = p.GetAllParticulars();
            foreach (var part in particulars)
            {
                foreach (var subtype in part.ParticularsSubTypes)
                {
                    dataGridView1.Rows[subtype.SubTypeID].Cells[setcellName].Value = defaultVal;
                    dataGridView1.Rows[subtype.SubTypeID].Cells[setCmbVal].Value = defaultVal;
                }
            }

            //----------------Calculations According to verticals
            int rowno = 17;
            int verticalsubSecCount = Properties.Settings.Default.VerticalsSubSection.Split(',').Length;
            foreach (var vert in verticals)
            {                
                for (int subver = 0; subver < verticalsubSecCount; subver++)
                {
                    dataGridView1.Rows[rowno].Cells[setcellName].Value = defaultVal;
                    dataGridView1.Rows[rowno].Cells[setCmbVal].Value = defaultVal;
                    rowno++;
                }
                rowno ++;
            }
        }

        private void CalculateDataUsingFormula(List<EmployeeDetails> empData, IEnumerable<string> verticals)
        {
            


            var averageColumnName = "Average";
            var updateColumnName = cmbBillingCycle.SelectedItem.ToString();

            //------------Financial Data
            var getFullRevenue = Calculate.CalculateSum(empData, CalculationType.Revenue, CalculationType.None);
            var getFullSalary = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.None);
            var getOnsiteRevenue = Calculate.CalculateSum(empData, CalculationType.Revenue, CalculationType.IsOnsite);
            var getOnsiteSalary = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsOnsite);
            var getOnShoreRevenue = Calculate.CalculateSum(empData, CalculationType.Revenue, CalculationType.IsOffShore);
            var getOnShoreSalary = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsOffShore);

            dataGridView1.Rows[1].Cells[updateColumnName].Value = String.Format("{0:F2}", getFullRevenue);
            dataGridView1.Rows[2].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculateGM(getFullRevenue, getFullSalary, true));
            dataGridView1.Rows[3].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculateGM(getOnsiteRevenue, getOnsiteSalary, true));
            dataGridView1.Rows[4].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculateGM(getOnShoreRevenue, getOnShoreSalary, true));

            dataGridView1.Rows[1].Cells[averageColumnName].Value = CalculateAverage(1);
            dataGridView1.Rows[2].Cells[averageColumnName].Value = CalculateAverage(2);
            dataGridView1.Rows[3].Cells[averageColumnName].Value = CalculateAverage(3);
            dataGridView1.Rows[4].Cells[averageColumnName].Value = CalculateAverage(4);

            //------------Resource Counts Data

            dataGridView1.Rows[6].Cells[updateColumnName].Value = empData.Count();
            dataGridView1.Rows[7].Cells[updateColumnName].Value = empData.Where(a => a.IsOnsite).Count();
            dataGridView1.Rows[8].Cells[updateColumnName].Value = empData.Where(a => !a.IsOnsite).Count();

            dataGridView1.Rows[6].Cells[averageColumnName].Value = CalculateAverage(6);
            dataGridView1.Rows[7].Cells[averageColumnName].Value = CalculateAverage(7);
            dataGridView1.Rows[8].Cells[averageColumnName].Value = CalculateAverage(8);

            //------------Account Management Count
            dataGridView1.Rows[9].Cells[updateColumnName].Value = empData.Where(a => a.AccountID == 1).Count();
            dataGridView1.Rows[9].Cells[averageColumnName].Value = CalculateAverage(9);

            //------------Account MGMT Cost
            var mgmCost = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsAccMgmt);
            dataGridView1.Rows[11].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculatePercent(mgmCost, getFullRevenue));
            dataGridView1.Rows[11].Cells[averageColumnName].Value = CalculateAverage(11);
            //------------NB Count
            dataGridView1.Rows[12].Cells[updateColumnName].Value = empData.Where(a => !a.IsBillable).Count();
            dataGridView1.Rows[12].Cells[averageColumnName].Value = CalculateAverage(12);
            //------------NB Cost
            var nbCost = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsNonBillable);
            dataGridView1.Rows[14].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculatePercent(nbCost, getFullRevenue));
            dataGridView1.Rows[14].Cells[averageColumnName].Value = CalculateAverage(14);

            //----------------Calculations According to verticals
            int colno = 17;
            foreach (var vert in verticals)
            {
                //% of Rev
                var verticalrev = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Revenue, CalculationType.None, vert);
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculatePercent(verticalrev, getFullRevenue));
                dataGridView1.Rows[colno].Cells[averageColumnName].Value = CalculateAverage(colno); 
                colno++;

                //Onsite % of Rev
                var verticalonsiterev = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Revenue, CalculationType.IsOnsite, vert); //empData.Where(a => a.VerticalName == vert && a.IsOnsite).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculatePercent(verticalonsiterev, verticalrev));
                dataGridView1.Rows[colno].Cells[averageColumnName].Value = CalculateAverage(colno); 
                colno++;

                //Offshore % of Rev
                var verticaloffshorerev = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Revenue, CalculationType.IsOffShore, vert);//empData.Where(a => a.VerticalName == vert && !a.IsOnsite).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculatePercent(verticaloffshorerev, verticalrev));//(offshorerev == 0 ? 0 : (verticaloffshorerev / offshorerev) * 100));
                dataGridView1.Rows[colno].Cells[averageColumnName].Value = CalculateAverage(colno); 
                colno++;

                //Onsite GM
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculateGM(verticalonsiterev, verticalrev, true));//(onsitegm == 0 ? 0 : (onsitegm - verticalonsitegm) / onsitegm));
                dataGridView1.Rows[colno].Cells[averageColumnName].Value = CalculateAverage(colno); 
                colno++;

                //Offshore GM
                var verticaloffshoregm = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Salary, CalculationType.None, vert);
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}", Calculate.CalculateGM(verticaloffshorerev, verticaloffshoregm, true));//(offshoregm == 0 ? 0 : (offshoregm - verticaloffshoregm) / offshoregm));
                dataGridView1.Rows[colno].Cells[averageColumnName].Value = CalculateAverage(colno); 
                colno += 2;
            }
            //-----------------------------------------------------------------------

            int totalCellCount = dataGridView1.Columns.Count - 3;
        }
                        
        private void DisableControls()
        {
            btnExportExcel.Enabled = false;
            btnExportPdf.Enabled = false;
            if (dataGridView1.Rows.Count > 0)
            {
                btnExportExcel.Enabled = true;
                btnExportPdf.Enabled = true;
            }
        }

        #endregion

        #region Event Validation

        private void FillBillingCycleDropDown()
        {
            for (int dtime = 5; dtime > 0; dtime--)
            {
                var getMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-dtime).Month).Substring(0, 3).ToUpper();
                cmbBillingCycle.Items.Add(getMonthName);
            }
        }

        private void FillAccountDropDown()
        {
            amodel = new AccountModel();
            var amodelList = amodel.GetAccountsList();
            cmbAccountName.ValueMember = "AccountID";
            cmbAccountName.DisplayMember = "AccountName";
            cmbAccountName.DataSource = amodelList;
        }

        private void ValidateUploadedFileForExtension(TextBox txtBoxToValidate, ref string cPath)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension 
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        txtBoxToValidate.Text = filePath;
                        cPath = filePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                }
            }
        }

        #endregion

        #region Export Data

        private void btnExportExcel_Click(object sender, EventArgs e)
        {            
            if (ExportDataToExcel())
                MessageBox.Show("Report exported successfully to excel");
            else
                MessageBox.Show("Error while downloading the report to excel");
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 30;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.DefaultCell.BorderWidth = 1;

                //Adding Header row
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }

                //Adding DataRow
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        pdfTable.AddCell(cell.Value == null ? string.Empty : cell.Value.ToString());
                    }
                }

                //Exporting to PDF
                string folderPath = Properties.Settings.Default.PDFExportPath;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (FileStream stream = new FileStream(folderPath + "DataGridViewExport.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
                MessageBox.Show("Report exported successfully to pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while export the file to pdf " + ex.Message);
            }

        }

        private bool ExportDataToExcel()
        {
            // creating Excel Application  
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            try
            {                
                // creating new Excelsheet in workbook  
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                // see the excel sheet behind the program  
                app.Visible = true;
                // get the reference of first sheet. By default its name is Sheet1.  
                // store its reference to worksheet  
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                // changing the name of active sheet  
                worksheet.Name = cmbBillingCycle.Text;
                // storing header part in Excel  
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet  
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                var outputPath = Properties.Settings.Default.OutputFolderPath;
                outputPath += cmbAccountName.Text + "\\" + cmbYear.Text + "\\" + cmbBillingCycle.Text+"\\";
                var path=string.Empty;
                if (errorMsgs.Count() > 0)
                {
                    outputPath = outputPath + "ErrorReport\\";
                    CreateFolderStructure(outputPath);
                    path = outputPath + "Error.xls";
                }
                else
                {
                    CreateFolderStructure(outputPath);
                    path = outputPath + cmbBillingCycle.Text + ".xls";
                }
                // save the application  
                workbook.SaveAs(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //workbook.Close();

                workbook.Close(0);
                //app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                // Exit from the application  
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                workbook.Close(Type.Missing, Type.Missing, Type.Missing);
                app.Quit();

                if (workbook != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                //app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            }
        }

        private static void CreateFolderStructure(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
        }

        private void CreatePathForExport(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void FillYearDropDown()
        {
            int years=DateTime.Now.Year;
            cmbYear.Items.Add(years-1);
            cmbYear.Items.Add(years);
        }

        #endregion

        #region Display uploaded Employee Data
        
        private void btnDisplayEmployeeModel_Click(object sender, EventArgs e)
        {
            if (txtEmpDetailsExcellName.Text == string.Empty || txtUploadBillingName.Text == string.Empty)
            {
                MessageBox.Show("Please upload the cost sheet and revenue sheet to view the details");
            }else
            {
                vemp = new ProcessFiles();
                List<ErrorMessage> errorMsgs = new List<ErrorMessage>();

                var empData = vemp.ProcessEmpFiles(cmbBillingCycle.SelectedItem.ToString(), cmbYear.SelectedItem.ToString(), Convert.ToInt32(cmbAccountName.SelectedValue),costPath, billingPath, ref errorMsgs);
                if (errorMsgs.Count() == 0)
                    dataGridView1.DataSource = empData;
                else
                    dataGridView1.DataSource = errorMsgs;
            }

        }

        #endregion

        #region Read Excel Data from Previous Sheets
                
        private void ReadDataForAccount(string SelectedAccount)
        {
            try
            {
                List<int> rownumbers = new List<int>();
                //specify the file name where its actually exist  
                var readOutputfile = Properties.Settings.Default.OutputFolderPath;
                string filepath = string.Empty;
                    int rows_count = 0;
                    Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                    try
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(readOutputfile + SelectedAccount + "//");
                        var files = dirInfo.GetFiles();
                        int counter = 1;
                        foreach (FileInfo fileInfo in files)
                        {
                            filepath = readOutputfile + SelectedAccount + "\\" + fileInfo.Name;
                            Microsoft.Office.Interop.Excel.Workbook WB = oExcel.Workbooks.Open(filepath);
                            try
                            {
                                // statement get the workbookname  
                                string ExcelWorkbookname = WB.Name;
                                // statement get the worksheet count  
                                int worksheetcount = WB.Worksheets.Count;

                                Microsoft.Office.Interop.Excel.Worksheet wks = (Microsoft.Office.Interop.Excel.Worksheet)WB.Worksheets[1];

                                // statement get the firstworksheetname  
                                Microsoft.Office.Interop.Excel.Range range = wks.UsedRange;

                                if (counter == 1)
                                {
                                    rows_count = range.Rows.Count;
                                }

                                string firstworksheetname = wks.Name;
                                if (counter == 1)
                                {
                                    CreateColumnsForGrid(null);
                                    dataGridView1.Rows.Add(rows_count - 17);
                                    //statement get the first cell value  
                                    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                                    {
                                        dataGridView1.Columns[i - 1].HeaderText = wks.Cells[1, i].Text;
                                    }
                                }
                                if (counter > 1)
                                {
                                    dataGridView1.Columns.Add(fileInfo.Name.Replace(fileInfo.Extension, ""), fileInfo.Name.Replace(fileInfo.Extension, ""));
                                }
                                // storing Each row and column value to excel sheet  
                                if (counter == 1)
                                {
                                    for (int i = 0; i < rows_count - 1; i++)
                                    {
                                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                                        {
                                            dataGridView1.Rows[i].Cells[j].Value = wks.Cells[i + 2, j + 1].Text;
                                            decimal result;
                                            if (decimal.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString(), out result))
                                            {
                                                if(!rownumbers.Exists(a=>a ==i))
                                                    rownumbers.Add(i);
                                            }                                          
                                                
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < rows_count - 1; i++)
                                    {
                                        for (int j = dataGridView1.Columns.Count - 1; j < dataGridView1.Columns.Count; j++)
                                        {
                                            dataGridView1.Rows[i].Cells[j].Value = wks.Cells[i + 2, 4].Text;                                           
                                        }
                                    }
                                }
                                counter++;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Issue occured.." + e.Message);
                            }
                            finally
                            {
                                if (WB != null)
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(WB);
                            }
                        }
                        DisplayAverageData(rownumbers);
                        MessageBox.Show("Data read successfully");
                    }
                    catch (Exception ex)
                    {
                    //    MessageBox.Show("Something went wrong.." + ex.Message);
                    }
                    finally
                    {
                        //oExcel.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
                    }               
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem reading the excel sheet, please try again in sometime! " + ex.Message);
            }
        }

        #endregion

        private void cmbBillingCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                var selectedBillingCycle = comboBox.SelectedItem.ToString();

                bool gridviewcolumns = CheckMonthExists(selectedBillingCycle);// dataGridView1.Columns.Contains(selectedBillingCycle);
                if (gridviewcolumns)
                {
                    //MessageBox.Show("Data already exists for the current month.\nPlease choose another month.");
                    var dataexistsmsg =MessageBox.Show("Data already exists for the current month.\nDo you want to reload the data for the selected month.","Data Already Exists", MessageBoxButtons.YesNo);
                    if (dataexistsmsg == System.Windows.Forms.DialogResult.No)
                    {
                        MessageBox.Show("Please select a different month");
                        comboBox.SelectedIndex = -1;
                    }else
                    {
                        MessageBox.Show("Functionality not implemented yet.\nWe regret any inconvenience caused by the functionality");
                    }                    
                }
            }
        }

        private bool CheckMonthExists(string selectedMonth)
        {
            var path = Properties.Settings.Default.OutputFolderPath + cmbAccountName.Text+"\\"+cmbYear.Text+"\\";
            if (Directory.Exists(path))
            {
                var file = Directory.GetFiles(path, cmbBillingCycle.Text + ".xls", SearchOption.AllDirectories)
                        .Count();
                if (file > 0)
                    return true;
            }
            return false;
        }

        private void DisplayAverageData(List<int> rowNumbers)
        {
            foreach (int row in rowNumbers)
                dataGridView1.Rows[row].Cells["Average"].Value = CalculateAverage(row);
        }

        private decimal CalculateAverage(int rowNumber)
        {
            int columnStartIndex = 3;
            int totalColumns = dataGridView1.Columns.Count - columnStartIndex;

            decimal totalVal = 0;
            for(int loop=columnStartIndex;loop <dataGridView1.Columns.Count;loop++)
            {
                totalVal += Convert.ToDecimal(dataGridView1.Rows[rowNumber].Cells[loop].Value);
            }
            return totalVal / totalColumns;            
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedIndex != -1)
            {
                ReadDataForAccountAndYear(cmbAccountName.SelectedItem.ToString(),comboBox.SelectedItem.ToString());           
            }
                
        }

        private void ReadDataForAccountAndYear(string SelectedAccount, string selectedYear)
        {
            try
            {
                var readOutputfile = Properties.Settings.Default.OutputFolderPath;
                DirectoryInfo dirInfo = new DirectoryInfo(readOutputfile + SelectedAccount + "//" + selectedYear + "//");
                if (dirInfo.Exists)
                {
                    var directs = dirInfo.GetDirectories();
                    if (directs.Length > 0)
                    {
                        var messageResult = MessageBox.Show("Do you like to load all months data.", "Month Data Alert", MessageBoxButtons.YesNo);
                        if (messageResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            int counter = 1;
                            foreach (var directory in directs)
                            {
                                List<int> rownumbers = new List<int>();
                                //specify the file name where its actually exist  

                                string filepath = string.Empty;
                                int rows_count = 0;
                                
                                try
                                {

                                    var files = directory.GetFiles();
                                    
                                    foreach (FileInfo fileInfo in files)
                                    {
                                        if (!fileInfo.Name.StartsWith("~"))
                                        {
                                            filepath = directory.FullName + "\\" + fileInfo.Name;
                                            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                                            Microsoft.Office.Interop.Excel.Workbook WB = oExcel.Workbooks.Open(filepath);
                                            // statement get the workbookname  
                                            string ExcelWorkbookname = WB.Name;
                                            // statement get the worksheet count  
                                            int worksheetcount = WB.Worksheets.Count;
                                            Microsoft.Office.Interop.Excel.Worksheet wks = (Microsoft.Office.Interop.Excel.Worksheet)WB.Worksheets[1];
                                            try
                                            {
                                                // statement get the firstworksheetname  
                                                Microsoft.Office.Interop.Excel.Range range = wks.UsedRange;
                                                rows_count = range.Rows.Count;                                                

                                                string firstworksheetname = wks.Name;
                                                if (counter == 1)
                                                {
                                                    CreateColumnsForGrid(null);
                                                    dataGridView1.Rows.Add(rows_count - 17);
                                                    //statement get the first cell value  
                                                    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                                                    {
                                                        dataGridView1.Columns[i - 1].HeaderText = wks.Cells[1, i].Text;
                                                    }
                                                }
                                                if (counter > 1)
                                                {
                                                    dataGridView1.Columns.Add(fileInfo.Name.Replace(fileInfo.Extension, ""), fileInfo.Name.Replace(fileInfo.Extension, ""));
                                                }
                                                // storing Each row and column value to excel sheet  
                                                if (counter == 1)
                                                {
                                                    for (int i = 0; i < rows_count - 1; i++)
                                                    {
                                                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                                                        {
                                                            dataGridView1.Rows[i].Cells[j].Value = wks.Cells[i + 2, j + 1].Text;
                                                            decimal result;
                                                            if (decimal.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString(), out result))
                                                            {
                                                                if (!rownumbers.Exists(a => a == i))
                                                                    rownumbers.Add(i);
                                                            }

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    for (int i = 0; i < rows_count - 1; i++)
                                                    {
                                                        for (int j = dataGridView1.Columns.Count - 1; j < dataGridView1.Columns.Count; j++)
                                                        {
                                                            dataGridView1.Rows[i].Cells[j].Value = wks.Cells[i + 2, 4].Text;
                                                        }
                                                    }
                                                }
                                                counter++;

                                            }
                                            catch (Exception e)
                                            {
                                                MessageBox.Show("Issue occured.." + e.Message);
                                            }
                                            finally
                                            {
                                                if (wks != null)
                                                {
                                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wks);
                                                    wks = null;
                                                }
                                                if (WB != null)
                                                {
                                                    WB.Close(false, null, null);
                                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(WB);
                                                    WB = null;
                                                }

                                                if (oExcel != null)
                                                {
                                                    oExcel.Quit();
                                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
                                                    oExcel = null;
                                                }
                                                GC.Collect();
                                                GC.WaitForPendingFinalizers();
                                                GC.Collect();

                                            }
                                        }
                                    }
                                    DisplayAverageData(rownumbers);
                                    
                                }
                                catch (Exception ex)
                                {
                                    //    MessageBox.Show("Something went wrong.." + ex.Message);
                                }                               
                            }
                            MessageBox.Show("Data read successfully");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem reading the excel sheet, please try again in sometime! " + ex.Message);
            } 
        }

        private void GenerateReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Dashboard")
                    frm.Show();
            } 
        }       
    }
}
