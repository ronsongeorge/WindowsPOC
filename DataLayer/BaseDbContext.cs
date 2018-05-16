using DataLayer.Repository;
using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BaseDbContext
    {
        public static string databasestring ="data source=databaseFile.db3";

        public static int counter = 1;

        public BaseDbContext()
        {
            if (counter == 1)
            {
                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                string assemblyDirectory = Path.GetDirectoryName(assemblyPath);
                string textPath = Path.Combine(assemblyDirectory, "databaseFile.db3");
                if (!File.Exists(textPath))
                {
                    System.Data.SQLite.SQLiteConnection.CreateFile("databaseFile.db3");
                    CreateAllTables();
                    InsertDefaultValues();
                }
                counter++;
            }
        }
                
        private static void CreateAllTables()
        {
            List<string> createTableQuery = DataBaseTableCreation();
            
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(databasestring))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    foreach (string query in createTableQuery)
                    {
                        com.CommandText = query;
                        com.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void InsertDefaultValues()
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;

            //Insert Accounts
            #region Insert Accounts

            AccountModel amodel = new AccountModel();
            amodel.CreateAccount("AQR", "AQR");
            amodel.CreateAccount("Marsh", "Marsh");  

            #endregion

            #region Insert Particulars and SubTypes

            CreateParticularAndSubType(dbManager, "Financial", "Avg Revenue");
            CreateParticularAndSubType(dbManager, "Financial", "YTD GM");
            CreateParticularAndSubType(dbManager, "Financial", "Onsite GM");
            CreateParticularAndSubType(dbManager, "Financial", "Offshore GM");
            CreateParticularAndSubType(dbManager, "Resource Counts", "Avg Total");
            CreateParticularAndSubType(dbManager, "Resource Counts", "Avg Offshore");
            CreateParticularAndSubType(dbManager, "Resource Counts", "Avg Onsite");
            CreateParticularAndSubType(dbManager, "Account MGMT #", "Account MGMT #");
            CreateParticularAndSubType(dbManager, "Account MGMT Cost", "% of revenue");
            CreateParticularAndSubType(dbManager, "NB #", "NB #");
            CreateParticularAndSubType(dbManager, "NB Cost", "% of revenue");

            #endregion
        }

        private static void CreateParticularAndSubType(IDBManager dbManager,string particularName,string particularSubType)
        {
            ParticularsModel pModel = new ParticularsModel();
            ParticularsSubTypeModel pSubModel = new ParticularsSubTypeModel();
            string sql;
            int lastID;
            try
            {
                pModel.CreateParticular(particularName);
                dbManager.Open();
                sql = "select seq from sqlite_sequence where name='Particulars'";
                lastID = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.Text, sql));
                dbManager.Close();
                pSubModel.CreateParticularsSubType(particularSubType, lastID);
            }
            catch
            {

            }
            finally
            {

            }
        }

        private static List<string> DataBaseTableCreation()
        {
            List<string> queryStrings = new List<string>();

            string createTableQuery = @"CREATE TABLE IF NOT EXISTS [Accounts] (
                          [AccountID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [AccountName] NVARCHAR(100)  NULL,
                          [AccountDescription] VARCHAR(2048)  NULL,
                          [BUID] INTEGER  
                          )";

            queryStrings.Add(createTableQuery);

            createTableQuery = @"CREATE TABLE IF NOT EXISTS [BU] (
                          [BUID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [BUName] NVARCHAR(100)  NULL,
                          [BUDescription] VARCHAR(2048) NULL
                          )";

            queryStrings.Add(createTableQuery);   
      

            createTableQuery = @"CREATE TABLE [BUAccountMapping] (
                          [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                        , [BUID] INTEGER NOT NULL
                        , [ACCOUNTID] INTEGER NOT NULL
                        )";
            queryStrings.Add(createTableQuery); 

            createTableQuery = @"CREATE TABLE IF NOT EXISTS [Particulars] (
                          [ParticularsID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [ParticularName] VARCHAR(100) NULL
                          )";

            queryStrings.Add(createTableQuery);


            createTableQuery = @"CREATE TABLE IF NOT EXISTS [ParticularsSubType] (
                          [ParticularsSubTypeID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [SubTypeName] VARCHAR(200) NULL,
                          [ParticularID] INTEGER
                          )";

            queryStrings.Add(createTableQuery);

            createTableQuery = @"CREATE TABLE IF NOT EXISTS [AccountCostRevenueData] (
                          [COSTID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [EmployeeID] INTEGER NOT NULL,
                          [EmployeeName] VARCHAR(500),
                          [ManagerName] VARCHAR(500),
                          [IsBillable] BIT,
                          [IsOnsite] BIT,
                          [SALARY] DECIMAL(10,2),
                          [GroupName] VARCHAR(500),
                          [Revenue] DECIMAL(10,2), 
                          [SeatCost] DECIMAL(10,2), 
                          [AccountID] INTEGER,
                          [MonthName] VARCHAR(5),
                          [Year] VARCHAR(4),
                          [EmployeeType] VARCHAR(25)                           
                          )";

            queryStrings.Add(createTableQuery);
            
            createTableQuery = @"CREATE TABLE IF NOT EXISTS [AccountMonthMasterData] (
                          [MasterID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [AccountID] INTEGER,
                          [MonthName] VARCHAR(5),
                          [Year] VARCHAR(4)                           
                          )";

            queryStrings.Add(createTableQuery);

            createTableQuery = @"CREATE TABLE IF NOT EXISTS [AccountMonthChildData] (
                          [ChildID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [MasterID] INTEGER,
                          [ParticularID] INTEGER NOT NULL,
                          [ParticularSubTypeID] INTEGER NOT NULL,
                          [Value] DECIMAL(10,2)
                          )";

            queryStrings.Add(createTableQuery);

            createTableQuery = @"CREATE TABLE IF NOT EXISTS [AccountMonthVerticalData] (
                          [AVerticalID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [MasterID] INTEGER,
                          [VerticalName] VARCHAR(500) NOT NULL,
                          [SubVerticalName] VARCHAR(500) NOT NULL,
                          [Value] DECIMAL(10,2)
                          )";

            queryStrings.Add(createTableQuery);

            return queryStrings;
        }
                               
        public void test()
        {
            // This is the query which will create a new table in our database file with three columns. An auto increment column called "ID", and two NVARCHAR type columns with the names "Key" and "Value"
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS [MyTable] (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [Key] NVARCHAR(2048)  NULL,
                          [Value] VARCHAR(2048)  NULL
                          )";

            System.Data.SQLite.SQLiteConnection.CreateFile("databaseFile.db3");        // Create the file which will be hosting our database
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(databasestring))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();                             // Open the connection to the database

                    com.CommandText = createTableQuery;     // Set CommandText to our query that will create the table
                    com.ExecuteNonQuery();                  // Execute the query

                    com.CommandText = "INSERT INTO MyTable (Key,Value) Values ('key one','value one')";     // Add the first entry into our database 
                    com.ExecuteNonQuery();      // Execute the query
                    com.CommandText = "INSERT INTO MyTable (Key,Value) Values ('key two','value value')";   // Add another entry into our database 
                    com.ExecuteNonQuery();      // Execute the query

                    com.CommandText = "Select * FROM MyTable";      // Select all rows from our database table

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Key"] + " : " + reader["Value"]);     // Display the value of the key and value column for every row
                        }
                    }
                    con.Close();        // Close the connection to the database
                }
            }
        }

        public void readDBFile()
        {
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection(databasestring))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();                             // Open the connection to the database

                    com.CommandText = "Select * FROM MyTable";      // Select all rows from our database table

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Key"] + " : " + reader["Value"]);     // Display the value of the key and value column for every row
                        }
                    }
                    con.Close();        // Close the connection to the database
                }
            }
        }
    }
}
