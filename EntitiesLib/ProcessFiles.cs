﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace EntitiesLib
{
    public class ProcessFiles
    {

        XDocument doc = new XDocument();
        List<EmployeeDetails> EmployeeObject = null;

        public List<EmployeeDetails> ProcessEmpFiles(string pathCost, string pathBilling)
        {
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
            EmployeeObject = new List<EmployeeDetails>();
            ISheet sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet
            EmployeeDetails emp = null;
            int rowIndex = 0;
            foreach (IRow row in sheet)
            {
                // skip header row
                if (rowIndex++ == 0) continue;
                try
                {
                    emp = new EmployeeDetails();

                    try
                    {
                        emp.EmployeeID = Convert.ToInt16(row.Cells[int.Parse(columns.Element("EmployeeID").Value) - 1].ToString());
                    }
                    catch
                    {
                        // junk data ; continue
                        continue;
                    }

                    emp.EmployeeName = string.Format("{0} {1}", row.Cells[int.Parse(columns.Element("EmloyeeFirstName").Value) - 1].ToString(),
                        row.Cells[int.Parse(columns.Element("EmployeeLastName").Value) - 1].ToString());
                    emp.IsOnsite = row.Cells[int.Parse(columns.Element("Location").Value) - 1].ToString().ToLower().Equals("india") ? false : true;
                    emp.VerticalName = row.Cells[int.Parse(columns.Element("GroupName").Value) - 1].ToString();
                    emp.ManagerName = row.Cells[int.Parse(columns.Element("ManagerName").Value) - 1].ToString();
                    emp.Salary = Decimal.Parse(row.Cells[int.Parse(columns.Element("CTC").Value) - 1].ToString().Replace("$", "").Replace(",", "")) / 12;

                    emp.AccountID = 0;
                    if (emp.VerticalName.ToLower().Equals("pmo") || emp.VerticalName.ToLower().Equals("account management"))
                        emp.AccountID = 1;

                    //add object to list
                    if (!EmployeeObject.Any(e => e.EmployeeID == emp.EmployeeID))
                    {
                        EmployeeObject.Add(emp);
                    }
                    else
                    {
                        throw new Exception(string.Format("Duplicate Employee ID Found:{0}", emp.EmployeeID));

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Exception while parsing cost file:{0} at row {1}- {2}", pathCost, row.RowNum, ex.Message));
                }

            }

            if (ValidateEmpDetails(pathBilling, columns))
                return EmployeeObject;
            else
                return null;
        }

        private bool ValidateEmpDetails(string pathBilling, XElement columns)
        {
            bool isSucess = false;
            IWorkbook workbook;
            using (FileStream stream = new FileStream(pathBilling, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(stream);
            }
            ISheet sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet

            #region Initialize private variable
            int rowIndex = 0;
            decimal value = 0;
            int id = 0;
            bool IsTotalCostCol = false;
            decimal tempRevenue;
            string identifier = string.Empty;
            int identifierColumn = int.Parse(columns.Element("ChangeInRevenue").Value) - 1;
            string identifierTxt = columns.Element("ChangeInRevenue").Attribute("identifier").Value;
            string parseTillSectionTxt = columns.Element("ChangeInRevenue").Attribute("ParseTillSection").Value;
            int empIdColumn = int.Parse(columns.Element("EmployeeID").Value) - 1;
            int amountColumn = int.Parse(columns.Element("Amount").Value) - 1;
            # endregion

            foreach (IRow row in sheet)
            {
                value = 0;
                tempRevenue = 0;
                // skip header row
                if (rowIndex++ < 2) continue;
                try
                {
                    identifier = row.Cells[identifierColumn].ToString();

                    // for now do not calculate row below subtotal i.e. other expenses.
                    if (identifier.ToLower().Contains(parseTillSectionTxt.ToLower()))
                        break;

                    if (!IsTotalCostCol && identifier.ToLower().Contains(identifierTxt.ToLower()))
                        IsTotalCostCol = true;

                    id = Convert.ToInt16(row.Cells[empIdColumn].ToString());

                    if (EmployeeObject.Any(e => e.EmployeeID == id))
                    {

                        if (!IsTotalCostCol)
                        {
                            if (decimal.TryParse(row.Cells[amountColumn].NumericCellValue.ToString(), out value) && value != 0)
                            {
                                EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue = value;
                                EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().IsBillable = true;
                            }
                            else
                            {
                                EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().IsBillable = false;
                            }

                            if (EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().IsOnsite == false)
                            {
                                EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue += int.Parse(columns.Attribute("SeatAllcationOffShore").Value);
                            }
                            else
                            {
                                EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue += EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().Salary * decimal.Parse(columns.Attribute("SeatAllcationOnShorePercnt").Value);
                            }

                        }
                        else
                        {
                            if (decimal.TryParse(row.Cells[int.Parse(columns.Element("TotalCost").Value) - 1].NumericCellValue.ToString(), out value) && value != 0)
                            {
                                tempRevenue = EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue;
                                EmployeeObject.Where(e => e.EmployeeID == id).FirstOrDefault().Revenue = tempRevenue + value;


                            }

                        }

                    }
                    else// CLIENT OR NEW MEMEBER?
                    {


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

    }
}
