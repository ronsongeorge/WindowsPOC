using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CurrencyModel : BaseDbContext
    {
        public List<Currency> GetCurrencyList()
        {
            List<Currency> currencyList = new List<Currency>();
            Currency currency;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select CurrencyID,CurrencyName,CurrencyFactor FROM Currencies";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                currency = new Currency();
                                currency.CurrencyID = Convert.ToInt32(reader["CurrencyID"]);
                                currency.CurrencyName = Convert.ToString(reader["CurrencyName"]);
                                currency.CurrencyConversionFactor = Convert.ToDecimal(reader["CurrencyFactor"]);
                                currencyList.Add(currency);
                            }
                        }
                        con.Close();        // Close the connection to the database
                    }
                }
                return currencyList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CreateCurrency(string CurrencyName, decimal CurrencyFactor)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Select 1 from Currencies where CurrencyName='{0}'", CurrencyName);     // Add the first entry into our database 
                        var exists = com.ExecuteScalar();
                        if (exists == null)
                        {
                            com.CommandText = string.Format("INSERT INTO Currencies(CurrencyName,CurrencyFactor) Values ('{0}','{1}')", CurrencyName, CurrencyFactor);     // Add the first entry into our database 
                            com.ExecuteNonQuery();
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

        public bool UpdateCurrency(string CurrencyID, string CurrencyName, decimal CurrencyFactor)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Update Currencies SET CurrencyName='{0}',CurrencyFactor='{1}' Where CurrencyID='{2}'", CurrencyName, CurrencyFactor, CurrencyID);
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
