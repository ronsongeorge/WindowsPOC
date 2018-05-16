using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BUModel :BaseDbContext
    {
        public List<BU> GetBUList()
        {
            List<BU> accountList = new List<BU>();
            BU account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select BUID,BUName,BUDescription FROM BU";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new BU();
                                account.BUID = Convert.ToInt32(reader["BUID"]);
                                account.BUName = Convert.ToString(reader["BUName"]);
                                account.BUDescription = Convert.ToString(reader["BUDescription"]);
                                accountList.Add(account);
                            }
                        }
                        con.Close();        // Close the connection to the database
                    }
                }
                return accountList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CreateBU(string BUName, string BUDescription,List<Account> accounts)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Select 1 from BU where BUName='{0}'", BUName);     // Add the first entry into our database 
                        var exists = com.ExecuteScalar();
                        if (exists == null)
                        {
                            com.CommandText = string.Format("INSERT INTO BU (BUName,BUDescription) Values ('{0}','{1}')", BUName, BUDescription);     // Add the first entry into our database 
                            com.ExecuteNonQuery();

                            com.CommandText = string.Format("SELECT BUID FROM BU WHERE BUNAME='{0}'", BUName);     // Add the first entry into our database 
                            string buid =Convert.ToString(com.ExecuteScalar());

                            foreach (Account acc in accounts)
                            {
                                com.CommandText = string.Format("INSERT INTO BUAccountMapping (BUID,AccountID) Values ('{0}','{1}')", buid, acc.AccountID);     // Add the first entry into our database 
                                com.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateBU(string BUID, string BUName, string BUDescription,List<Account> AccList)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Update BU SET BUName='{0}',BUDescription='{1}' Where BUID='{2}'", BUName, BUDescription, BUID);
                        com.ExecuteNonQuery();
                        com.CommandText = string.Format("delete from BUAccountMapping where BUID='{0}'", BUID);
                        com.ExecuteNonQuery();

                        foreach (Account acc in AccList)
                        {
                            com.CommandText = string.Format("INSERT INTO BUAccountMapping (BUID,AccountID) Values ('{0}','{1}')", string.Format("SELECT BUID FROM BU WHERE BUNAME='{0}'", BUName), acc.AccountID);     // Add the first entry into our database 
                            com.ExecuteNonQuery();
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AssignMultipleAccountsToBU(int BUID, List<int> Accounts)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();

                        UnAssignAllAccountsInBU(BUID);
                        foreach (int accID in Accounts)
                        {
                            com.CommandText = string.Format("UPDATE Accounts set BUID ='{0}' WHERE AccountID='{1}'", BUID, accID);     // Add the first entry into our database 
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool UnAssignAllAccountsInBU(int BUID)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("UPDATE Accounts set BUID =NULL WHERE BUID='{0}'", BUID);     // Add the first entry into our database 
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }                
    }
}
