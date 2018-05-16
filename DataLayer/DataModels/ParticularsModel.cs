using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ParticularsModel : BaseDbContext
    {
        public List<Particulars> GetParticularList()
        {
            List<Particulars> verticalList = new List<Particulars>();
            Particulars vertical;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select ParticularID,ParticularName FROM Particulars";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vertical = new Particulars();
                                vertical.ParticularsID = Convert.ToInt32(reader["ParticularID"]);
                                vertical.ParticularsName = Convert.ToString(reader["ParticularName"]);
                                verticalList.Add(vertical);
                            }
                        }
                        con.Close();        // Close the connection to the database
                    }
                }
                return verticalList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CreateParticular(string ParticularName)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Select 1 from Particulars where ParticularName='{0}'", ParticularName);     // Add the first entry into our database 
                        var exists = com.ExecuteScalar();
                        if (exists == null)
                        {
                            com.CommandText = string.Format("INSERT INTO Particulars (ParticularName) Values ('{0}')", ParticularName);     // Add the first entry into our database 
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

        public bool UpdateParticular(string ParticularID, string ParticularName)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Update Particulars SET ParticularName='{0}' Where ParticularID='{1}'", ParticularName, ParticularID);
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
