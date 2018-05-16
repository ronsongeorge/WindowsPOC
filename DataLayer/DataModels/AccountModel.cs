using DataLayer.DataModels;
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
    public class AccountModel :BaseDbContext
    {
        public List<Account> GetAccountsList()
        {
            List<Account> accountList = new List<Account>();
            Account account;
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            try
            {
                dbManager.Open();
                dbManager.ExecuteReader(CommandType.Text, "Select AccountID,AccountName,AccountDescription FROM Accounts");
                while (dbManager.DataReader.Read())
                {
                    account = new Account();
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
            dbManager.ConnectionString = BaseDbContext.databasestring;
            try
            {
                dbManager.Open();
                var exists = dbManager.ExecuteScalar(CommandType.Text, string.Format("Select 1 from Accounts where AccountName='{0}'", AccountName));

                if (exists == null)
                {
                    int result = dbManager.ExecuteNonQuery(CommandType.Text,string.Format("INSERT INTO Accounts (AccountName,AccountDescription) Values ('{0}','{1}')", AccountName, AccountDescription));
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

        public List<string> GetExcludedVerticalNames(string AccountID)
        {
            List<string> excludedAccountNames = new List<string>();
            excludedAccountNames.Add("PMO");
            excludedAccountNames.Add("Account Management");
            return excludedAccountNames;
        }

        public List<Account> GetAccountsListwrtBU(int BUID)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            try
            {
                dbManager.Open();
                dbManager.ExecuteReader(CommandType.Text, string.Format(@"Select A.AccountID,AccountName,AccountDescription 
                            FROM Accounts A
                            JOIN BUAccountMapping B ON B.ACCOUNTID=A.ACCOUNTID WHERE B.BUID='{0}'",BUID));
                while (dbManager.DataReader.Read())
                {
                    account = new Account();
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

        public List<AccountBUViewModel> GetAccountsListWithBU()
        {
            List<AccountBUViewModel> accountList = new List<AccountBUViewModel>();
            AccountBUViewModel account;
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            try
            {
                dbManager.Open();
                dbManager.ExecuteReader(CommandType.Text, "Select AccountID,AccountName,AccountDescription,A.BUID,BUName FROM Accounts A JOIN BU B on B.BUID=A.BUID ");
                while (dbManager.DataReader.Read())
                {
                    account = new AccountBUViewModel();
                    account.BUID = Convert.ToInt32(dbManager.DataReader["BUID"]);
                    account.BUName = Convert.ToString(dbManager.DataReader["BUName"]);
                    account.AccountID = Convert.ToInt32(dbManager.DataReader["AccountID"]);
                    account.AccountName=Convert.ToString(dbManager.DataReader["AccountName"]);                    
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
    }
}
