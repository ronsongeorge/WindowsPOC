using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReportModel : Class1
    {
        public List<AccountVerticalData> GetAccountMonthWiseData(string MonthName, string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetAccountYearWiseData(string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetAccountQuarterWiseData(string Month, string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetAccountHalfYearWiseData(string Month, string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetBUMonthWiseData(string MonthName, string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetBUYearWiseData(string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetBUQuarterWiseData(string Month, string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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

        public List<AccountVerticalData> GetBUHalfYearWiseData(string Month, string Year, string AccountName)
        {
            List<Account> accountList = new List<Account>();
            Account account;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(Class1.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select AccountID,AccountName,AccountDescription FROM Accounts";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                account = new Account();
                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.AccountName = Convert.ToString(reader["AccountName"]);
                                account.AccountDescription = Convert.ToString(reader["AccountDescription"]);
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
    }
}
