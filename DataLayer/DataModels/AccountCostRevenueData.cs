using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLib;

namespace DataLayer
{
    public class AccountCostRevenueData : BaseDbContext
    {
        public List<EmployeeDetails> GetCostnRevenueData(int AccountId,string Month, string Year)
        {
            List<EmployeeDetails> EmployeeDetailsList = new List<EmployeeDetails>();
            EmployeeDetails emp;
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            try
            {
                dbManager.Open();
                dbManager.ExecuteReader(CommandType.Text, "Select EmployeeID,EmployeeName,ManagerName,isBillable,isOnsite,"
                +"SALARY,GroupName, Revenue,SeatCost,AccountID,MonthName,Year,"
                +"EmployeeTypeID FROM AccountCostRevenueData "
                + string.Format("Where AccountID={0} AND MonthName={1} AND Year={2}", AccountId, Month, Year));

                while (dbManager.DataReader.Read())
                {
                    emp = new EmployeeDetails();
                    emp.AccountID = Convert.ToInt32(dbManager.DataReader["AccountID"]);
                    emp.EmployeeID = Convert.ToInt32(dbManager.DataReader["EmployeeID"]);
                    emp.EmployeeName = Convert.ToString(dbManager.DataReader["EmployeeName"]);
                    emp.EmployeeTypeID = Convert.ToInt32(dbManager.DataReader["EmployeeTypeID"]);
                    emp.IsBillable = Convert.ToBoolean(dbManager.DataReader["IsBillable"]);
                    emp.IsOnsite = Convert.ToBoolean(dbManager.DataReader["IsOnsite"]);
                    emp.ManagerName = Convert.ToString(dbManager.DataReader["ManagerName"]);
                    emp.Revenue = Convert.ToDecimal(dbManager.DataReader["Revenue"]);
                    emp.Salary = Convert.ToDecimal(dbManager.DataReader["Salary"]);
                    emp.SeatCost = Convert.ToDecimal(dbManager.DataReader["SeatCost"]);
                    emp.VerticalName = Convert.ToString(dbManager.DataReader["VerticalName"]);
                   
                    EmployeeDetailsList.Add(emp);
                }
                dbManager.DataReader.Close();
                return EmployeeDetailsList;
            }

            catch (Exception ex)
            {
                return null;
            }

            finally
            {
                dbManager.Close();
                dbManager.Dispose();
            }
        }

        public bool InsertCostnRevenueData(List<EmployeeDetails> empObjList, string month, string year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            try
            {

                DataTable dt = new DataTable();

               
                dbManager.Open();
              //  dbManager.ExecuteReader(CommandType.Text, "Select * FROM AccountCostRevenueData");
                //var exists = dbManager.ExecuteScalar(CommandType.Text, string.Format("Select 1 from Accounts where AccountName='{0}'", AccountName));

                StringBuilder insertQuery = null;
                foreach (EmployeeDetails empobj in empObjList)
                {
                    try
                    {
                        insertQuery = new StringBuilder();
                        insertQuery.Append("INSERT INTO AccountCostRevenueData");
                        insertQuery.Append(" (EmployeeID,EmployeeName,ManagerName,isBillable,isOnsite,SALARY,GroupName, Revenue,SeatCost,AccountID,MonthName,Year,EmployeeType)");
                        insertQuery.AppendFormat(" Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                               + empobj.EmployeeID, empobj.EmployeeName, empobj.ManagerName, empobj.IsBillable, empobj.IsOnsite, empobj.Salary, empobj.VerticalName, empobj.Revenue, empobj.SeatCost, empobj.AccountID, month, year, empobj.EmployeeTypeID);

                        int result = dbManager.ExecuteNonQuery(CommandType.Text, insertQuery.ToString());
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                                                   
                }

                return true;

            }

            catch (Exception ex)
            {
                return false;
            }

            finally
            {
                dbManager.Close();
                dbManager.Dispose();
            }
        }

        public bool UpdateAccount(string AccountID, string AccountName, string AccountDescription)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Update Accounts SET AccountName='{0}',AccountDescription='{1}' Where AccountID='{2}'", AccountName, AccountDescription, AccountID);
                        com.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
