using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using EntitiesLib;
using DataLayer;

namespace BusinessLayer
{
    public class ProcessFiles
    {
        
        XDocument doc = new XDocument();
        List<EmployeeDetails> EmployeeObjtList = null;
        // List<ErrorMessage> errMsgList = new List<ErrorMessage>();
        string FileType = string.Empty;
        ErrorMessage errMsg = null;

        public List<EmployeeDetails> ProcessEmpFiles(string MonthName,string Year,int AccountID,string pathCost, string pathBilling, ref List<ErrorMessage> errMsgList)
        {
            FileType = "Cost File";
            Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.LastIndexOf('\\')) + "\\" + "Configuration.xml";
            doc = XDocument.Load(path);
            //doc = XDocument.Load(@"D:\WindowsPOC\EntitiesLib\Classes\Configuration.xml");
            XElement columns = doc.Descendants("Columns").FirstOrDefault();

            IWorkbook workbook;
            using (FileStream stream = new FileStream(pathCost, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(stream);
            }
            EmployeeObjtList = new List<EmployeeDetails>();
            ISheet sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet
            EmployeeDetails emp = null;
            int rowIndex = 0;
            decimal salary;

            #region ColumnIndex variables
            int CostStartIndex = int.Parse(columns.Attribute("CostStartIndex").Value);
            int EmployeeID = ColNmToNum(columns.Element("EmployeeID").Value);
            int EmloyeeFirstName = ColNmToNum(columns.Element("EmloyeeFirstName").Value);
            int EmployeeLastName = ColNmToNum(columns.Element("EmployeeLastName").Value);
            int Location = ColNmToNum(columns.Element("Location").Value);
            int GroupName = ColNmToNum(columns.Element("GroupName").Value);
            int ManagerName = ColNmToNum(columns.Element("ManagerName").Value);
            int Salary = ColNmToNum(columns.Element("Salary").Value);
            #endregion

            foreach (IRow row in sheet)
            {
                // skip header row
                if (rowIndex++ < CostStartIndex - 1) continue;
                try
                {
                    emp = new EmployeeDetails();

                    try
                    {
                        emp.EmployeeID = Convert.ToInt16(row.Cells[EmployeeID - 1].ToString());
                    }
                    catch (Exception ex)
                    {
                        if (row.Cells[0].ToString().ToUpper().Contains("SUBTOTAL"))
                            break;
                        else
                            throw ex;

                    }
                    if (!string.IsNullOrWhiteSpace(row.Cells[EmloyeeFirstName - 1].ToString()) &&
                                          !string.IsNullOrWhiteSpace(row.Cells[EmployeeLastName - 1].ToString()))
                    {
                        emp.EmployeeName = string.Format("{0} {1}", row.Cells[EmloyeeFirstName - 1].ToString(),
                       row.Cells[EmployeeLastName - 1].ToString());
                    }
                    else
                    {
                        errMsg = new ErrorMessage(FileType, "Either the First Name or Last Name is missing",
                            string.Format("{0} {1}", row.Cells[EmloyeeFirstName - 1].ToString(), row.Cells[EmployeeLastName - 1].ToString()), row.RowNum);
                        errMsgList.Add(errMsg);

                        continue;
                    }

                    emp.IsOnsite = row.Cells[Location - 1].ToString().ToLower().Equals("india") ? false : true;

                    if (!string.IsNullOrWhiteSpace(row.Cells[GroupName - 1].ToString()))
                        emp.VerticalName = row.Cells[GroupName - 1].ToString();
                    else
                    {

                        errMsg = new ErrorMessage(FileType, "Vertical Name not found", emp.EmployeeName, row.RowNum);
                        errMsgList.Add(errMsg);
                        continue;

                    }

                    if (!string.IsNullOrWhiteSpace(row.Cells[ManagerName - 1].ToString()))
                        emp.ManagerName = row.Cells[ManagerName - 1].ToString();
                    else
                    {
                        errMsg = new ErrorMessage(FileType, "ManagerName not found", emp.EmployeeName, row.RowNum);
                        errMsgList.Add(errMsg);
                        continue;
                    }

                    if (decimal.TryParse(row.Cells[Salary - 1].NumericCellValue.ToString(), out salary) && salary != 0)
                        emp.Salary = (decimal)row.Cells[Salary - 1].NumericCellValue / 12;
                    else
                    {
                        errMsg = new ErrorMessage(FileType, "Cannot read Salary", emp.EmployeeName, row.RowNum);
                        errMsgList.Add(errMsg);
                        continue;
                    }


                    emp.EmployeeTypeID = 0;

                    emp.AccountID = AccountID;
                    if (emp.VerticalName.ToLower().Equals("pmo") || emp.VerticalName.ToLower().Equals("account management"))
                        emp.AccountID = 1;

                    //add object to list
                    if (!EmployeeObjtList.Any(e => e.EmployeeID == emp.EmployeeID))
                    {
                        EmployeeObjtList.Add(emp);
                    }
                    else
                    {
                        errMsg = new ErrorMessage(FileType, "Duplicate Employee ID Found in a cost file", emp.EmployeeName, emp.EmployeeID);
                        errMsgList.Add(errMsg);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Exception while parsing cost file:{0} at row {1} - {2}", pathCost, row.RowNum, ex.Message));
                }

            }

            if (ValidateEmpDetails(pathBilling, columns, ref errMsgList))
            {
               AccountCostRevenueData db= new AccountCostRevenueData();
               db.InsertCostnRevenueData(EmployeeObjtList, MonthName, Year);
                return EmployeeObjtList;
            }               
            else
                return null;

        }

        private bool ValidateEmpDetails(string pathBilling, XElement columns, ref List<ErrorMessage> errMsgList)
        {
            FileType = "Billing File";
            bool isSucess = false;
            IWorkbook workbook;
            using (FileStream stream = new FileStream(pathBilling, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(stream);
            }
            ISheet sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet

            int rowIndex = 0;
            decimal value = 0;
            int id = 0;
            bool IsTotalCostCol = false;
            decimal tempRevenue;
            string identifier = string.Empty;
            List<int> empIdList = new List<int>();
            int emdId = 0;

            #region ColumnIndex variables
            int BillingStartIndex = int.Parse(columns.Attribute("BillingStartIndex").Value);
            int identifierColumn = ColNmToNum(columns.Element("ChangeInRevenue").Value);
            string identifierTxt = columns.Element("ChangeInRevenue").Attribute("identifier").Value;
            string parseTillSectionTxt = columns.Element("ChangeInRevenue").Attribute("ParseTillSection").Value;
            int empIdColumn = ColNmToNum(columns.Element("EmployeeID").Value);
            int amountColumn = ColNmToNum(columns.Element("Amount").Value);
            int TotalCost = ColNmToNum(columns.Element("TotalCost").Value);
            int SeatAllcationOffShore = int.Parse(columns.Attribute("SeatAllcationOffShore").Value);
            decimal SeatAllcationOnShorePercnt = decimal.Parse(columns.Attribute("SeatAllcationOnShorePercnt").Value);
            # endregion

            foreach (IRow row in sheet)
            {
                if (rowIndex++ < BillingStartIndex - 1) continue;
                identifier = row.Cells[identifierColumn - 1].ToString();
                try
                {
                    if (identifier.ToLower().Contains(identifierTxt.ToLower()))
                        break;
                    emdId = Convert.ToInt16(row.Cells[empIdColumn - 1].ToString());
                    empIdList.Add(emdId);
                }
                catch
                {
                    // junk data
                }

            }

            // validating emp id against cost file
            foreach (EmployeeDetails e in EmployeeObjtList)
            {
                if (!empIdList.Contains(e.EmployeeID))
                {
                    errMsg = new ErrorMessage(FileType, "Employee ID does not exist in billing file", e.EmployeeName, e.EmployeeID);
                    errMsgList.Add(errMsg);
                }

            }

            // adding revenue and is billable field to emp object
            foreach (IRow row in sheet)
            {
                value = 0;
                tempRevenue = 0;
                // skip header row
                if (rowIndex++ < BillingStartIndex - 1) continue;
                try
                {
                    identifier = row.Cells[identifierColumn - 1].ToString();

                    // for now do not calculate row below subtotal i.e. other expenses.
                    if (identifier.ToLower().Contains(parseTillSectionTxt.ToLower()))
                        break;

                    if (!IsTotalCostCol && identifier.ToLower().Contains(identifierTxt.ToLower()))
                        IsTotalCostCol = true;

                    id = Convert.ToInt16(row.Cells[empIdColumn - 1].ToString());

                    if (EmployeeObjtList.Any(e => e.EmployeeID == id))
                    {

                        if (!IsTotalCostCol)
                        {
                            if (decimal.TryParse(row.Cells[amountColumn - 1].NumericCellValue.ToString(), out value) && value != 0)
                            {
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue = value;
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().IsBillable = true;
                            }
                            else
                            {
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().IsBillable = false;
                            }

                            if (EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().IsOnsite == false)
                            {
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().SeatCost = SeatAllcationOffShore;
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Salary += SeatAllcationOffShore;
                            }
                            else
                            {
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().SeatCost = EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Salary * (SeatAllcationOnShorePercnt);
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Salary += EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Salary * (SeatAllcationOnShorePercnt);
                            }

                        }
                        else
                        {
                            if (decimal.TryParse(row.Cells[TotalCost - 1].NumericCellValue.ToString(), out value) && value != 0)
                            {
                                tempRevenue = EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue;
                                EmployeeObjtList.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue = tempRevenue + value;


                            }

                        }

                    }
                    else// CLIENT OR NEW MEMBER?
                    {

                        errMsg = new ErrorMessage(FileType, "Employee ID does not exist in cost file as mentioned in Billing file", id.ToString(), row.RowNum);
                        errMsgList.Add(errMsg);
                    }
                }
                catch
                {
                    // junk row continue processing.
                }
            }

            isSucess = true;

            return isSucess;
        }

        public static int ColNmToNum(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("columnName");

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += (columnName[i] - 'A' + 1);
            }

            return sum;
        }

    }

}
