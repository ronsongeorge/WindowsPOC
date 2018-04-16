using EntitiesLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsPOC
{
    public partial class GenerateReport : Form
    {
        #region Properties
        ProcessFiles vemp;
        string costPath;
        string billingPath;
        #endregion

        public GenerateReport()
        {
            InitializeComponent();
            FillBillingCycleDropDown();
            FillAccountDropDown();
            DisableControls();
        }

        #region Events

        private void btnUploadEmp_Click(object sender, EventArgs e)
        {
            ValidateUploadedFileForExtension(txtEmpDetailsExcellName,ref costPath); 
        }

        private void btnUploadBilling_Click(object sender, EventArgs e)
        {
            bool checkFileExists = true;
            if (txtEmpDetailsExcellName.Text == string.Empty)
            {
                checkFileExists = false;
                MessageBox.Show("Upload the employee cost sheet before proceeding");
            }
            if(checkFileExists)
                ValidateUploadedFileForExtension(txtUploadBillingName,ref billingPath);           
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if(CanGenerateReport())
            {
                vemp = new ProcessFiles();
                var empData = vemp.ProcessEmpFiles(costPath, billingPath);
                FillReportData(empData);
                dataGridView1.Visible = true;
                DisableControls();
                MessageBox.Show("Date generated successfully");
            }
                
        }
         
        private void btnExportExcel_Click(object sender, EventArgs e)
        {            
            if (ExportDataToExcel())
            {
                MessageBox.Show("Report exported successfully to excel");
            }
            else
            {
                MessageBox.Show("Error while downloading the report to excel");
            }
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
                string folderPath = Properties.Settings.Default.ExportPath;
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

        private void btnReadData_Click(object sender, EventArgs e)
        {
            ReadAndPopulateDataFromExcelSheet();
        }

        #endregion

        #region Methods

        private void DisableControls()
        {
            btnExportExcel.Enabled = false;
            btnExportPdf.Enabled = false;
            if(dataGridView1.Rows.Count > 0)
            {
                btnExportExcel.Enabled = true;
                btnExportPdf.Enabled = true;
            }
        }

        private void FillBillingCycleDropDown()
        {
            for (int dtime = 5; dtime > 0; dtime--)
            {
                var getMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-dtime).Month).Substring(0,3).ToUpper() + "-";
                var getYearName = DateTime.Now.AddMonths(-dtime).Year;
                cmbBillingCycle.Items.Add(getMonthName + getYearName);
            }
        }

        private void FillAccountDropDown()
        {
            for (int accint = 1; accint < 5; accint++)
            {
                cmbAccountName.Items.Add("Account-" +accint);
            }
        }

        private void ValidateUploadedFileForExtension(TextBox txtBoxToValidate,ref string cPath)
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
                if(cmbBillingCycle.SelectedIndex ==-1)
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

        private decimal CalculateMargin(decimal parameter1, decimal parameter2)
        {
            return ((parameter1 -parameter2) /parameter1);
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
                        string[] testarray = { "% of Rev", "Onsite % of Rev", "Offshore % of Rev", "Onsite GM", "Offshore GM" };
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
            CalculateDataUsingFormula(empData,verticals);
        }

        private void SetDefaultData(IEnumerable<string> verticals)
        {
            string setcellName = "AVERAGE";
            int defaultVal = 0;
            string setCmbVal = cmbBillingCycle.SelectedItem.ToString();
            Particulars p = new Particulars();
            var particulars = p.GetAllParticulars();
            foreach(var part in particulars)
            {
                foreach(var subtype in part.ParticularsSubTypes)
                {
                    dataGridView1.Rows[subtype.SubTypeID].Cells[setcellName].Value = defaultVal;
                    dataGridView1.Rows[subtype.SubTypeID].Cells[setCmbVal].Value = defaultVal;
                }
            }
           
            //----------------Calculations According to verticals
            int rowno = 17;
            foreach (var vert in verticals)
            {
                dataGridView1.Rows[rowno].Cells[setcellName].Value = defaultVal;
                dataGridView1.Rows[rowno].Cells[setCmbVal].Value = defaultVal;
                rowno++;
                dataGridView1.Rows[rowno].Cells[setcellName].Value = defaultVal;
                dataGridView1.Rows[rowno].Cells[setCmbVal].Value = defaultVal;
                rowno++;
                dataGridView1.Rows[rowno].Cells[setcellName].Value = defaultVal;
                dataGridView1.Rows[rowno].Cells[setCmbVal].Value = defaultVal;
                rowno++;
                dataGridView1.Rows[rowno].Cells[setcellName].Value = defaultVal;
                dataGridView1.Rows[rowno].Cells[setCmbVal].Value = defaultVal;
                rowno++;
                dataGridView1.Rows[rowno].Cells[setcellName].Value = defaultVal;
                dataGridView1.Rows[rowno].Cells[setCmbVal].Value = defaultVal;
                rowno += 2;
            }
        }

        private void CalculateDataUsingFormula(List<EmployeeDetails> empData,IEnumerable<string> verticals)
        {
            var updateColumnName = cmbBillingCycle.SelectedItem.ToString();
            //------------Financial Data
            dataGridView1.Rows[1].Cells[updateColumnName].Value = String.Format("{0:F2}",empData.Select(a => a.Revenue).Sum());
            dataGridView1.Rows[2].Cells[updateColumnName].Value = String.Format("{0:F2}",(((empData.Select(a => a.Revenue).Sum() - empData.Select(a => a.Salary).Sum() )/ empData.Select(a => a.Revenue).Sum()) * 100));
            dataGridView1.Rows[3].Cells[updateColumnName].Value = String.Format("{0:F2}",(((empData.Where(a => a.IsOnsite).Select(a => a.Revenue).Sum() - empData.Where(a => a.IsOnsite).Select(a => a.Salary).Sum()) / empData.Where(a => a.IsOnsite).Select(a => a.Revenue).Sum()) * 100));
            dataGridView1.Rows[4].Cells[updateColumnName].Value = String.Format("{0:F2}", (((empData.Where(a => !a.IsOnsite).Select(a => a.Revenue).Sum() - empData.Where(a => !a.IsOnsite).Select(a => a.Salary).Sum()) / empData.Where(a => !a.IsOnsite).Select(a => a.Revenue).Sum()) * 100));
            //------------Resource Counts Data
            dataGridView1.Rows[6].Cells[updateColumnName].Value = empData.Count();
            dataGridView1.Rows[7].Cells[updateColumnName].Value = empData.Where(a => a.IsOnsite).Count();
            dataGridView1.Rows[8].Cells[updateColumnName].Value = empData.Where(a => !a.IsOnsite).Count();
            //------------Account Management Count
            dataGridView1.Rows[9].Cells[updateColumnName].Value = empData.Where(a => a.AccountID == 1).Count();
            //------------Account MGMT Cost
            dataGridView1.Rows[11].Cells[updateColumnName].Value = String.Format("{0:F2}",((empData.Where(a => a.AccountID == 1).Select(a => a.Salary).Sum()) / empData.Select(a => a.Revenue).Sum() * 100));
            //------------NB Count
            dataGridView1.Rows[12].Cells[updateColumnName].Value = empData.Where(a => !a.IsBillable).Count();
            //------------NB Cost
            dataGridView1.Rows[14].Cells[updateColumnName].Value = String.Format("{0:F2}",((empData.Where(a => !a.IsBillable).Select(a => a.Salary).Sum()) / empData.Select(a => a.Revenue).Sum() * 100));
            
            //----------------Calculations According to verticals
            int colno = 17;
            foreach (var vert in verticals)
            {
                var totalrev = empData.Select(a => a.Revenue).Sum();
                var verticalrev = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}",((totalrev == 0 ? 0 : verticalrev / totalrev) * 100));
                colno++;
                var onsiterev = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                var verticalonsiterev = empData.Where(a => a.VerticalName == vert && a.IsOnsite).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}",(onsiterev == 0 ? 0 : (verticalonsiterev / onsiterev) * 100));
                colno++;
                var offshorerev = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                var verticaloffshorerev = empData.Where(a => a.VerticalName == vert && !a.IsOnsite).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}",(offshorerev == 0 ? 0 : (verticaloffshorerev / offshorerev) * 100));
                colno++;
                var onsitegm = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                var verticalonsitegm = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}",(onsitegm == 0 ? 0 : (onsitegm - verticalonsitegm) / onsitegm));
                colno++;
                var offshoregm = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                var verticaloffshoregm = empData.Where(a => a.VerticalName == vert).Select(a => a.Revenue).Sum();
                dataGridView1.Rows[colno].Cells[updateColumnName].Value = String.Format("{0:F2}",(offshoregm == 0 ? 0 : (offshoregm - verticaloffshoregm) / offshoregm));
                colno += 2;
            }
            //-----------------------------------------------------------------------

            int totalCellCount = dataGridView1.Columns.Count - 3;
            CalculateAverageDataUsingFormula(empData, totalCellCount,verticals);

        }

        private void CalculateAverageDataUsingFormula(List<EmployeeDetails> empData, int totalCellCount,IEnumerable<string> allverticals)
        {
            string setcellName = "AVERAGE";

            Particulars p = new Particulars();
            var particulars = p.GetAllParticulars();
            for (int colcnt = 3; colcnt <= dataGridView1.Columns.Count - 1; colcnt++)
            {
                foreach (var part in particulars)
                {
                    foreach (var subtype in part.ParticularsSubTypes)
                    {
                        dataGridView1.Rows[subtype.SubTypeID].Cells[setcellName].Value = (Convert.ToDecimal(dataGridView1.Rows[subtype.SubTypeID].Cells[setcellName].Value) + Convert.ToDecimal(dataGridView1.Rows[subtype.SubTypeID].Cells[colcnt].Value)); 
                    }
                }
                //dataGridView1.Rows[3].Cells[setcellName].Value = empData.Where(a => a.IsOnsite).Select(a => (a.Revenue - a.CTC / a.Revenue)) == null ? 0 : (Convert.ToDecimal(dataGridView1.Rows[3].Cells[setcellName].Value) + Convert.ToDecimal(dataGridView1.Rows[3].Cells[colcnt].Value));
            }

            foreach (var part in particulars)
            {
                foreach (var subtype in part.ParticularsSubTypes)
                {
                    dataGridView1.Rows[subtype.SubTypeID].Cells[setcellName].Value = String.Format("{0:F2}",(Convert.ToDecimal(dataGridView1.Rows[subtype.SubTypeID].Cells[setcellName].Value)) / totalCellCount);
                }
            }


            int colno = 17;
            foreach (var vert in allverticals)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int colcnt = 3; colcnt <= dataGridView1.Columns.Count - 1; colcnt++)
                    {
                        dataGridView1.Rows[colno].Cells[setcellName].Value = String.Format("{0:F2}",(Convert.ToDecimal(dataGridView1.Rows[colno].Cells[setcellName].Value) + Convert.ToDecimal(dataGridView1.Rows[colno].Cells[colcnt].Value))); 
                        colno++;
                    }
                    
                }colno++;
            }
            colno = 17;
            foreach (var vert in allverticals)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int colcnt = 3; colcnt <= dataGridView1.Columns.Count - 1; colcnt++)
                    {
                        colno++;
                        dataGridView1.Rows[colno].Cells[setcellName].Value = String.Format("{0:F2}",(Convert.ToDecimal(dataGridView1.Rows[colno].Cells[setcellName].Value)) / totalCellCount);
                    }
                    
                }colno++;
            }

        }

        private void FillReportData(List<EmployeeDetails> empData)
        {
            var verticals = empData.Select(a => a.VerticalName).Distinct();
            CreateColumnsForGrid(verticals);
            UploadDataForSingleMonth(cmbBillingCycle.SelectedText, verticals,empData);
        }
        
        private bool ExportDataToExcel()
        {
            // creating Excel Application  
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                // creating new WorkBook within Excel application  
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                // creating new Excelsheet in workbook  
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                // see the excel sheet behind the program  
                app.Visible = true;
                // get the reference of first sheet. By default its name is Sheet1.  
                // store its reference to worksheet  
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                // changing the name of active sheet  
                worksheet.Name = "Exported from gridview";
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
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value ==null ? string.Empty : dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // save the application  
                workbook.SaveAs("d:\\output.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                // Exit from the application  
                return true;
            }
            catch 
            {                
                return false;
            }
        }
        
        //Populate Previous Data
        private void ReadAndPopulateDataFromExcelSheet()
        {

            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();

            //specify the file name where its actually exist  
            string filepath = @"D:\\output.xls";

            //pass that to workbook object  
            Microsoft.Office.Interop.Excel.Workbook WB = oExcel.Workbooks.Open(filepath);


            // statement get the workbookname  
            string ExcelWorkbookname = WB.Name;

            // statement get the worksheet count  
            int worksheetcount = WB.Worksheets.Count;

            Microsoft.Office.Interop.Excel.Worksheet wks = (Microsoft.Office.Interop.Excel.Worksheet)WB.Worksheets[1];

            // statement get the firstworksheetname  

            Microsoft.Office.Interop.Excel.Range range = wks.UsedRange;
            int rows_count = range.Rows.Count;

            string firstworksheetname = wks.Name;
            

            //var verticals = empData.Select(a => a.VerticalName).Distinct();
            CreateColumnsForGrid(null);
            dataGridView1.Rows.Add(rows_count-17);
            //statement get the first cell value  
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                dataGridView1.Columns[i - 1].HeaderText = wks.Cells[1, i].Text;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < rows_count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = wks.Cells[i + 2, j + 1].Text;
                }
            }
        }

        #endregion
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            cmbAccountName.SelectedIndex =- 1;
            cmbBillingCycle.SelectedIndex = -1;
            txtEmpDetailsExcellName.Text=string.Empty;
            txtUploadBillingName.Text=string.Empty;

        }
    }
}
