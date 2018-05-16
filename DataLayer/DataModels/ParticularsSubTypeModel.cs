using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ParticularsSubTypeModel : BaseDbContext
    {
        public List<ParticularsSubType> GetParticularsSubTypeList()
        {
            List<ParticularsSubType> subtypeList = new List<ParticularsSubType>();
            ParticularsSubType subType;
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = "Select SubTypeID,SubTypeName FROM ParticularsSubType";      // Select all rows from our database table
                        using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subType = new ParticularsSubType();
                                subType.SubTypeID = Convert.ToInt32(reader["SubTypeID"]);
                                subType.SubTypeName = Convert.ToString(reader["SubTypeName"]);
                                subtypeList.Add(subType);
                            }
                        }
                        con.Close();        // Close the connection to the database
                    }
                }
                return subtypeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CreateParticularsSubType(string SubTypeName,int ParticularID)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Select 1 from ParticularsSubType where SubTypeName='{0}' and ParticularID='{1}'", SubTypeName,ParticularID);     // Add the first entry into our database 
                        var exists = com.ExecuteScalar();
                        if (exists == null)
                        {
                            com.CommandText = string.Format("INSERT INTO ParticularsSubType (SubTypeName,ParticularID) Values ('{0}','{1}')", SubTypeName, ParticularID);     // Add the first entry into our database 
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

        public bool UpdateParticularsSubTypeName(string ParticularSubTypeID, string ParticularSubTypeName)
        {
            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        com.CommandText = string.Format("Update ParticularSubType SET ParticularSubTypeName='{0}' Where ParticularSubTypeID='{1}'", ParticularSubTypeName, ParticularSubTypeID);
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
