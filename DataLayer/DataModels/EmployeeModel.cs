using DataLayer.Repository;
using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class EmployeeModel : Class1
    {
        public List<EmployeeDetails> GetEmployeeDetails()
        {
            List<EmployeeDetails> accountList = new List<EmployeeDetails>();
            EmployeeDetails account;
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = Class1.databasestring;
            try
            {
                dbManager.Open();
                dbManager.ExecuteReader(CommandType.Text, "Select AccountID,AccountName,AccountDescription FROM Accounts");
                while (dbManager.DataReader.Read())
                {
                    account = new EmployeeDetails();
                    account.AccountID = Convert.ToInt32(dbManager.DataReader["AccountID"]);
                    account.AccountName = Convert.ToString(dbManager.DataReader["AccountName"]);
                    account.AccountDescription = Convert.ToString(dbManager.DataReader["AccountDescription"]);
                    accountList.Add(account);
                }
                dbManager.DataReader.Close();
                return accountList;
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

        public bool CreateAccount(string AccountName, string AccountDescription)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = Class1.databasestring;
            try
            {
                dbManager.Open();
                var exists = dbManager.ExecuteScalar(CommandType.Text, string.Format("Select 1 from Accounts where AccountName='{0}'", AccountName));

                if (exists == null)
                {
                    int result = dbManager.ExecuteNonQuery(CommandType.Text,string.Format("INSERT INTO Accounts (AccountName,AccountDescription,CreatedOn) Values ('{0}','{1}','{2}')", AccountName, AccountDescription, DateTime.Now.ToString("MM/dd/yyyy")));
                    if(result > 0)
                        return true;
                    return false;
                }else
                    return false;
                
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
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
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
