using BusinessLayer;
using DataLayer.Repository;
using EntitiesLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AccountMonthRevenue : BaseDbContext
    {
        public bool CreateAccountMonthWiseData(string MonthName, int Year, string AccountID,List<EmployeeDetails> empData)
        {
            AccountModel accModel = new AccountModel();            
            List<ErrorMessage> errorMsg = new List<ErrorMessage>();            
            ParticularsSubTypeModel pSubType = new ParticularsSubTypeModel();
            List<ParticularsSubType> pSubTypeList = pSubType.GetParticularsSubTypeList();
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            bool isInsertSuccessful = true;
            try
            {
                dbManager.Open();

                #region Insert Data into Primary Table

                string insertQuery = string.Format("INSERT INTO AccountMonthMasterData(AccountID,MonthName,Year) Values ('{0}','{1}','{2}')", AccountID, MonthName, Year);
                int result = dbManager.ExecuteNonQuery(CommandType.Text, insertQuery);
                if (result <= 0)
                    isInsertSuccessful = false;

                #endregion

                #region Get Vertyical Names

                string sql = "select seq from sqlite_sequence where name='AccountMonthMasterData'";
                int lastID = Convert.ToInt32(dbManager.ExecuteScalar(CommandType.Text, sql));

                var excludedGroupNames = accModel.GetExcludedVerticalNames(AccountID);

                var verticals = empData.Where(a => !excludedGroupNames.Contains(a.VerticalName)).Select(a => a.VerticalName).Distinct();

                #endregion

                #region Get Data for Calculation

                var getFullRevenue = Calculate.CalculateSum(empData, CalculationType.Revenue, CalculationType.None);
                var getFullSalary = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.None);
                var getOnsiteRevenue = Calculate.CalculateSum(empData, CalculationType.Revenue, CalculationType.IsOnsite);
                var getOnsiteSalary = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsOnsite);
                var getOnShoreRevenue = Calculate.CalculateSum(empData, CalculationType.Revenue, CalculationType.IsOffShore);
                var getOnShoreSalary = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsOffShore);
                decimal calculatedValue = 0;

                #endregion

                #region Financial Data

                calculatedValue = getFullRevenue;
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Financial", "Avg Revenue");
                
                calculatedValue = Calculate.CalculateGM(getFullRevenue, getFullSalary, true);
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Financial", "YTD GM");

                calculatedValue = Calculate.CalculateGM(getOnsiteRevenue, getOnsiteSalary, true);
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Financial", "Onsite GM");
                
                calculatedValue = Calculate.CalculateGM(getOnShoreRevenue, getOnShoreSalary, true);
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Financial", "Offshore GM");

                #endregion
                
                #region Resource Counts Data

                calculatedValue = empData.Count();
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Resource Counts", "Avg Total");
                                
                calculatedValue = empData.Where(a => a.IsOnsite).Count();
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Resource Counts", "Avg Offshore");
               
                calculatedValue = empData.Where(a => !a.IsOnsite).Count();
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Resource Counts", "Avg Onsite");
               
                #endregion
                
                #region Account Management Count

                calculatedValue = empData.Where(a => a.AccountID == 1).Count();
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Account MGMT #", "Account MGMT #");
                               
                #endregion
                
                #region Account Management Cost

                var mgmCost = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsAccMgmt);
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "Account MGMT Cost", "% of revenue");
                
                #endregion

                #region NB Count

                calculatedValue = empData.Where(a => !a.IsBillable).Count();
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "NB #", "NB #");
               
                #endregion

                #region NB Cost

                var nbCost = Calculate.CalculateSum(empData, CalculationType.Salary, CalculationType.IsNonBillable);
                calculatedValue = Calculate.CalculatePercent(nbCost, getFullRevenue);
                isInsertSuccessful = InsertDataInChildTable(dbManager, lastID, calculatedValue, "NB Cost", "% of revenue");

                #endregion

                #region Vertical Specific Data
                                
                foreach (var verticalName in verticals)
                {
                    #region Vertical Wise Data

                    //1
                    var verticalrev = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Revenue, CalculationType.None, verticalName);
                    var calculateValue = Calculate.CalculatePercent(verticalrev, getFullRevenue);
                    isInsertSuccessful = InsertDataInVerticalTable(dbManager, lastID, calculateValue, verticalName, "% of Rev");
                    
                    //2
                    var verticalonsiterev = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Revenue, CalculationType.IsOnsite, verticalName);
                    calculateValue = Calculate.CalculatePercent(verticalonsiterev, verticalrev);
                    isInsertSuccessful = InsertDataInVerticalTable(dbManager, lastID, calculateValue, verticalName, "Onsite % of Rev");
                    
                    //3
                    var verticaloffshorerev = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Revenue, CalculationType.IsOffShore, verticalName);
                    calculateValue = Calculate.CalculatePercent(verticaloffshorerev, verticalrev);
                    isInsertSuccessful = InsertDataInVerticalTable(dbManager, lastID, calculateValue, verticalName, "Offshore % of Rev");
                    
                    //4
                    calculateValue = Calculate.CalculateGM(verticalonsiterev, verticalrev, true);
                    isInsertSuccessful = InsertDataInVerticalTable(dbManager, lastID, calculateValue, verticalName, "Onsite GM");
                   
                    //5
                    var verticaloffshoregm = Calculate.CalculateSumWithVerticalName(empData, CalculationType.Salary, CalculationType.None, verticalName);
                    calculateValue = Calculate.CalculateGM(verticaloffshorerev, verticaloffshoregm, true);
                    isInsertSuccessful = InsertDataInVerticalTable(dbManager, lastID, calculateValue, verticalName, "Offshore GM");
                   
                    #endregion
                }

                #endregion

                return isInsertSuccessful;
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

        private static void GetParticularIDAndTypeID(IDBManager dbManager, string particularName, string particularSubType, out int particularID, out int particularSubTypeID)
        {
            particularID = 0;
            particularSubTypeID = 0;
            string selectQuery = string.Format(@"SELECT ParticularsID,ParticularsSubTypeID from Particulars p
                                                JOIN ParticularsSubType pst on pst.ParticularID =p.ParticularsID 
                                                WHERE p.ParticularName='{0}' and pst.SubTypeName='{1}'", particularName, particularSubType);
            var dataread= dbManager.ExecuteReader(CommandType.Text, selectQuery);
            while (dataread.Read())
            {
                particularID = Convert.ToInt32(dataread["ParticularsID"]);
                particularSubTypeID = Convert.ToInt32(dataread["ParticularsSubTypeID"]);
            }
            dataread.Close();
        }
        
        private static bool InsertDataInChildTable(IDBManager dbManager, int lastID, decimal calculatedValue,string particularName,string particularSubType)
        {
            int particularID = 0;
            int particularSubTypeID = 0;
            GetParticularIDAndTypeID(dbManager,particularName, particularSubType, out particularID, out particularSubTypeID);
            string insertQuery = string.Format("INSERT INTO AccountMonthChildData(MasterID,ParticularID,ParticularSubTypeID,Value) Values ('{0}','{1}','{2}','{3}')", lastID, particularID, particularSubTypeID, Math.Round(calculatedValue, 2));
            int result = dbManager.ExecuteNonQuery(CommandType.Text, insertQuery);
            if (result <= 0)
                return false;
            return true;
        }
        
        private static bool InsertDataInVerticalTable(IDBManager dbManager, int lastID, decimal calculatedValue,string verticalName,string verticalSubType)
        {
            string insertQuery = string.Format("INSERT INTO AccountMonthVerticalData(MasterID,VerticalName,SubVerticalName,Value) Values ('{0}','{1}','{2}','{3}')", lastID, verticalName, verticalSubType, Math.Round(calculatedValue,2));
            int result = dbManager.ExecuteNonQuery(CommandType.Text, insertQuery);
            if (result <= 0)
                return false;
            return true;
        }

        public bool IsDataPresent(string MonthName, int Year, string AccountID)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            string insertQuery = string.Format("SELECT 1 FROM AccountMonthMasterData WHERE AccountID='{0}' AND MonthName ='{1}',Year='{2}'", AccountID, MonthName, Year);
            int result = (Int32)dbManager.ExecuteScalar(CommandType.Text, insertQuery);
            return (result > 0);
        }

        public bool DeleteExistingAccountData(string MonthName, int Year, string AccountID)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            string insertQuery = string.Format("DELETE FROM AccountMonthMasterData WHERE AccountID='{0}' AND MonthName ='{1}',Year='{2}'", AccountID, MonthName, Year);
            int result = (Int32)dbManager.ExecuteNonQuery(CommandType.Text, insertQuery);
            return (result > 0);
        }
                
        public void GetMonthwiseSingleAccountData(List<string> MonthName, int Year, string AccountID)
        {
            
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            DataTable dt = new DataTable();
            dt.Columns.Add("Particulars");
            dt.Columns.Add("ParticularsSubType");
            dt.Columns.Add("Average");
            Dictionary<int, string> MasterList = new Dictionary<int, string>();
            try
            {
                if(MonthName.Count > 0)
                dbManager.Open();

                foreach (string Month in MonthName)
                {
                    dt.Columns.Add(Month);

                    int MasterID = 0;
                    string selectQuery = string.Format(@"SELECT MasterID from AccountMonthMasterData aMasterData 
                                                WHERE aMasterData.AccountID='{0}' and aMasterData.MonthName='{1}'
                                                and aMasterData.Year='{2}'", AccountID, Month, Year);
                    var dataread = dbManager.ExecuteReader(CommandType.Text, selectQuery);
                    while (dataread.Read())
                    {
                        MasterID = Convert.ToInt32(dataread["MasterID"]);
                        MasterList.Add(MasterID,Month);
                    }
                    dataread.Close();
                }

                int counter = 0;               
                
                var selectQuery1 = string.Empty;
                var selectQuery2 = string.Empty;
                var queryFirst = string.Empty;
                var querySecond = string.Empty;
                var finalquery = string.Empty;

                var queryFirstUpper = @"SELECT P.ParticularName as PName,PS.SubTypeName as SUB,Value as {1}";
                var queryFirstLower = @" FROM AccountMonthChildData C
                                            JOIN Particulars P ON P.ParticularsID=C.ParticularID
                                            JOIN ParticularsSubType PS ON PS.ParticularsSubTypeID=C.ParticularSubTypeID
                                            WHERE C.MasterID='{0}'";

                var querySecUpper = @"SELECT V.VerticalName AS PName,V.SubVerticalName as SUB,Value as {1}";

                var querySecLower = @" FROM AccountMonthVerticalData V
                                     WHERE V.MasterID='{0}'";

                if (MasterList.Count == 1)
                {
                    queryFirst = string.Format(queryFirstUpper + queryFirstLower, MasterList.First().Key, MasterList.First().Value);
                    querySecond = string.Format(querySecUpper + querySecLower, MasterList.First().Key, MasterList.First().Value);
                    finalquery = queryFirst +" UNION ALL "+ querySecond;
                }
                else
                {

                    foreach (var item in MasterList)
                    {
                        if (counter != 0)
                        {
                            string upperinsertStatement1 = string.Format(@",(SELECT Value 
                    FROM AccountMonthChildData C1 
                    WHERE C1.MasterID ='{0}' and C1.ParticularID =P.ParticularsID
                    AND C1.ParticularSubTypeID = PS.ParticularsSubTypeID
                    ) AS {1}", item.Key, item.Value);

                            string lowerinsertStatement1 = string.Format(@",(SELECT Value 
	                    FROM AccountMonthVerticalData V1 
	                    WHERE V1.MasterID ='{0}' AND V1.VerticalName=V.VerticalName 
	                    AND V1.SubVerticalName=V.SubVerticalName
                    ) AS {1}", item.Key, item.Value);


                            queryFirstUpper += upperinsertStatement1;
                            querySecUpper += lowerinsertStatement1;
                        }
                        else
                        {
                            queryFirstUpper = string.Format(queryFirstUpper, item.Key, item.Value);
                            queryFirstLower = string.Format(queryFirstLower, item.Key, item.Value);
                            querySecUpper = string.Format(querySecUpper, item.Key, item.Value);
                            querySecLower = string.Format(querySecLower, item.Key, item.Value);
                        }
                        counter++;
                 
                    }
                    finalquery = queryFirstUpper + queryFirstLower + " UNION ALL " + querySecUpper + querySecLower;
                }
                
                
                var DS = dbManager.ExecuteDataSet(CommandType.Text, finalquery);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                dbManager.Close();
            }
        }
        
        public void GetMultipleMonthBUWiseData(Dictionary<string,int> MonthYear, int BUID)
        {
            //Get All Accounts according to BU ID
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            AccountModel accModel = new AccountModel();
            List<Account> accList = accModel.GetAccountsListwrtBU(BUID);
            StringBuilder ParticularstopQuery = new StringBuilder();
            StringBuilder ParticularsUpperQuery = new StringBuilder();
            StringBuilder ParticularsLowerQuery = new StringBuilder();
            StringBuilder VerticalUpperQuery = new StringBuilder();
            StringBuilder VerticalLowerQuery = new StringBuilder();

            StringBuilder YearString = new StringBuilder(" AND M.Year in ('");
            StringBuilder MonthString = new StringBuilder(" AND M.MonthName in ('");

            foreach (var Month in MonthYear)
            {
                YearString.Append(Month.Value + "','");
                MonthString.Append(Month.Key + "','");
            }
            YearString = YearString.Remove(YearString.Length - 2, 2).Append(") ");
            MonthString = MonthString.Remove(MonthString.Length - 2, 2).Append(") ");
           
            string dynamicQuery = string.Empty;
            try
            {
                ParticularstopQuery.Append("SELECT P.ParticularName as PName,PS.SubTypeName as SUB");

                //Fetch Data for all the data listed for the month
                int counter = 0;
                string counter0 = string.Empty;
                foreach (Account acc in accList)
                {
                    #region Upper Part Particular Query

                    ParticularstopQuery.AppendFormat(",AVG({0}.Value) as {0}", acc.AccountName);

                    if (counter == 0)
                    {
                        ParticularsUpperQuery.AppendFormat(@" FROM
                        AccountMonthMasterData M
                        JOIN ACCOUNTS A ON A.ACCOUNTID=M.ACCOUNTID and M.accountid={0}
                        {2}{3}
                        JOIN AccountMonthChildData {1} ON {1}.MasterID=  M.MasterID
                        JOIN Particulars P ON P.ParticularsID={1}.ParticularID
                        JOIN ParticularsSubType PS ON PS.ParticularsSubTypeID={1}.ParticularSubTypeID", 
                        acc.AccountID, acc.AccountName,YearString,MonthString);
                        counter0 = acc.AccountName;
                    }
                    else
                    {
                        ParticularsUpperQuery.AppendFormat(@"
                        LEFT JOIN
                        (SELECT ParticularsID,ParticularSubTypeID,P.ParticularName as PName,
                        PS.SubTypeName as SUB,AVG(Value) as Value,a.ACCOUNTID
                         FROM
                        AccountMonthMasterData M
                        JOIN ACCOUNTS A ON A.ACCOUNTID=M.ACCOUNTID AND M.ACCOUNTID={0}
                        {3}{4}
                        JOIN AccountMonthChildData C ON C.MasterID=  M.MasterID
                        JOIN Particulars P ON P.ParticularsID=C.ParticularID
                        JOIN ParticularsSubType PS ON PS.ParticularsSubTypeID=C.ParticularSubTypeID 
                        WHERE M.ACCOUNTID={0}
                        GROUP BY M.ACCOUNTID,PS.ParticularsSubTypeID 
                        ) {1} ON {1}.ACCOUNTID={0}
                        and 
                        {1}.ParticularsID={2}.ParticularID
                        and {1}.ParticularSubTypeID={2}.ParticularSubTypeID", 
                        acc.AccountID,acc.AccountName,counter0,YearString,MonthString);
                    }
                    

                    counter++;
                    #endregion
                }
                //Display the data for BU
                ParticularsUpperQuery.Append(" GROUP BY A.ACCOUNTID,PS.ParticularsSubTypeID");
                var finalQuery = ParticularstopQuery.Append(ParticularsUpperQuery).ToString();
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, finalQuery);
            }
            catch (Exception e)
            {

            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet MonthDataExists(int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
           
            AccountModel acc = new AccountModel();
            var accountList=acc.GetAccountsList();
            StringBuilder stringQuery = new StringBuilder();
            stringQuery.AppendFormat("SELECT m.month");
            
            foreach (Account a in accountList)
            {
                stringQuery.AppendFormat(@",CASE WHEN EXISTS(SELECT 1 FROM 
                    AccountMonthMasterData 
				    WHERE MonthName=m.Month and Accountid={0} AND Year={2}) 
                    then 1 else 0 end {1} ",a.AccountID,a.AccountName,Year);
            }
            stringQuery.AppendFormat(@" FROM 
                (
                SELECT 'JAN' AS
                MONTH
                UNION SELECT 'FEB' AS
                MONTH
                UNION SELECT 'MAR' AS
                MONTH
                UNION SELECT 'APR' AS
                MONTH
                UNION SELECT 'MAY' AS
                MONTH
                UNION SELECT 'JUN' AS
                MONTH
                UNION SELECT 'JUL' AS
                MONTH
                UNION SELECT 'AUG' AS
                MONTH
                UNION SELECT 'SEP' AS
                MONTH
                UNION SELECT 'OCT' AS
                MONTH
                UNION SELECT 'NOV' AS
                MONTH
                UNION SELECT 'DEC' AS
                MONTH
                ) AS m");            

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        #region Reports

        public DataSet GetHighestSalary(int FromNoofPerson, int ToNoofPerson, int? BU, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU!=null)
            {
               AccountModel acc = new AccountModel();
               AccountList= acc.GetAccountsListwrtBU((int)BU);
            }

            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT A.AccountName,s.EmployeeID,s.EmployeeName,Salary,
                (SELECT COUNT()+1 FROM (
                    SELECT DISTINCT Salary AS Sal 
                    FROM ACCOUNTCOSTREVENUEDATA AS t WHERE Salary > S.Salary)
                ) AS Rank
                FROM ACCOUNTCOSTREVENUEDATA S
                JOIN Accounts A ON A.AccountID=S.AccountID
                where 
                S.AccountID IN ('{2}') 
                AND (Rank between {0} and {1}) and Year={4}
                GROUP BY A.AccountName,s.EmployeeID,s.EmployeeName,Salary
                ORDER BY A.AccountName,Rank", FromNoofPerson,ToNoofPerson, string.Join("','", stringAccList)
                                            , Year);
            }else
            {
                stringQuery.AppendFormat(@"SELECT A.AccountName,s.EmployeeID,s.EmployeeName,Salary,
                (SELECT COUNT()+1 FROM (
                    SELECT DISTINCT Salary AS Sal 
                    FROM ACCOUNTCOSTREVENUEDATA AS t WHERE Salary > S.Salary)
                ) AS Rank
                FROM ACCOUNTCOSTREVENUEDATA S
                JOIN BUAccountMapping B ON B.ACCOUNTID=S.ACCOUNTID
                JOIN Accounts A ON A.AccountID=B.AccountID
                where (Rank between {0} and {1}) AND B.BUID={2} 
                    and Year={4}
                GROUP BY A.AccountName,s.EmployeeID,s.EmployeeName,Salary
                ORDER BY A.AccountName,Rank", FromNoofPerson,ToNoofPerson, BU,Year);
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet GetLowestSalary(int FromNoofPerson, int ToNoofPerson, int? BU, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }

            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT A.AccountName,s.EmployeeID,s.EmployeeName,Salary,
                (SELECT COUNT()+1 FROM (
                    SELECT DISTINCT Salary AS Sal 
                    FROM ACCOUNTCOSTREVENUEDATA AS t WHERE Salary < S.Salary)
                ) AS Rank
                FROM ACCOUNTCOSTREVENUEDATA S
                JOIN Accounts A ON A.AccountID=S.AccountID
                where 
                S.AccountID IN ('{2}') 
                AND (Rank between {0} and {1}) and Year={3}
                GROUP BY A.AccountName,s.EmployeeID,s.EmployeeName,Salary
                ORDER BY A.AccountName,Rank", FromNoofPerson, ToNoofPerson, string.Join("','", stringAccList)
                                            , Year);
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT A.AccountName,s.EmployeeID,s.EmployeeName,Salary,
                (SELECT COUNT()+1 FROM (
                    SELECT DISTINCT Salary AS Sal 
                    FROM ACCOUNTCOSTREVENUEDATA AS t WHERE Salary < S.Salary)
                ) AS Rank
                FROM ACCOUNTCOSTREVENUEDATA S
                JOIN BUAccountMapping B ON B.ACCOUNTID=S.ACCOUNTID
                JOIN Accounts A ON A.AccountID=B.AccountID
                where (Rank between {0} and {1}) AND B.BUID={2} 
                    and Year={3}
                GROUP BY A.AccountName,s.EmployeeID,s.EmployeeName,Salary
                ORDER BY A.AccountName,Rank", FromNoofPerson, ToNoofPerson, BU, Year);
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet GetPeopleWithHighestMargin(int NoofPerson, int? BU, List<Account> AccountList, int Year, List<string> MonthList)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }
            
            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT A.AccountName ,EmployeeID,EmployeeName,ManagerName,
                IsBillable,IsOnsite,Salary,Revenue,SeatCost,EmployeeType,Year,
                ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) as MarginGM
                from AccountCostRevenueData e1
                JOIN Accounts A ON A.Accountid=e1.Accountid
                WHERE {0} > (SELECT COUNT(DISTINCT Salary)
                FROM AccountCostRevenueData e2
                JOIN Accounts A ON A.Accountid=e1.Accountid
                WHERE e1.AccountID = e2.AccountID AND e1.Salary > e2.Salary)
                AND e1.AccountID IN ('{1}') AND YEAR='{2}'
                AND MONTHNAME IN ('{3}')
                GROUP BY E1.ACCOUNTID,EmployeeID
                ORDER BY ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) ASC"
                , NoofPerson, string.Join("','", stringAccList), Year, string.Join("','", MonthList));
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT b.BUName,A.AccountName ,EmployeeID,EmployeeName,ManagerName,
                IsBillable,IsOnsite,Salary,Revenue,SeatCost,EmployeeType,Year,
                ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) as MarginGM
                from AccountCostRevenueData e1
                JOIN Accounts A ON A.Accountid=e1.Accountid
                JOIN BU b on b.BUID=a.BUID
                WHERE {0} > (SELECT COUNT(DISTINCT Salary)
                FROM AccountCostRevenueData e2
                JOIN Accounts A ON A.Accountid=e1.Accountid
                JOIN BU b on b.BUID=a.BUID
                WHERE e1.AccountID = e2.AccountID AND e1.Salary > e2.Salary)
                AND YEAR='{2}'
                AND b.BUID IN ({1})
                AND MONTHNAME IN ('{3}')
                GROUP BY E1.ACCOUNTID,EmployeeID
                ORDER BY ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) ASC"
                , NoofPerson, BU, Year, string.Join("','", MonthList));
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet GetPeopleWithLowestMargin(int NoofPerson, int? BU, List<Account> AccountList, int Year, List<string> MonthList)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }

            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT A.AccountName ,EmployeeID,EmployeeName,ManagerName,
                IsBillable,IsOnsite,Salary,Revenue,SeatCost,EmployeeType,Year,
                ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) as MarginGM
                from AccountCostRevenueData e1
                JOIN Accounts A ON A.Accountid=e1.Accountid
                WHERE {0} > (SELECT COUNT(DISTINCT Salary)
                FROM AccountCostRevenueData e2
                JOIN Accounts A ON A.Accountid=e1.Accountid
                WHERE e1.AccountID = e2.AccountID AND e1.Salary < e2.Salary)
                AND e1.AccountID IN ('{1}') AND YEAR='{2}'
                AND MONTHNAME IN ('{3}')
                GROUP BY E1.ACCOUNTID,EmployeeID
                ORDER BY ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) DESC"
                , NoofPerson, string.Join("','", stringAccList), Year, string.Join("','", MonthList));
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT b.BUName,A.AccountName ,EmployeeID,EmployeeName,ManagerName,
                IsBillable,IsOnsite,Salary,Revenue,SeatCost,EmployeeType,Year,
                ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) as MarginGM
                from AccountCostRevenueData e1
                JOIN Accounts A ON A.Accountid=e1.Accountid
                JOIN BU b on b.BUID=a.BUID
                WHERE {0} > (SELECT COUNT(DISTINCT Salary)
                FROM AccountCostRevenueData e2
                JOIN Accounts A ON A.Accountid=e1.Accountid
                JOIN BU b on b.BUID=a.BUID
                WHERE e1.AccountID = e2.AccountID AND e1.Salary < e2.Salary)
                AND YEAR='{2}'
                AND b.BUID IN ({1})
                AND MONTHNAME IN ('{3}')
                GROUP BY E1.ACCOUNTID,EmployeeID
                ORDER BY ROUND((AVG(Revenue)-AVG(Salary))/cast(AVG(Revenue) as real)*100,2) DESC"
                , NoofPerson, BU, Year, string.Join("','", MonthList));
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }
                
        public DataSet AccountMarginReportBasedOnBU(int BU, int Year, List<string> MonthList)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;           
            StringBuilder stringQuery = new StringBuilder();

            stringQuery.AppendFormat(@"SELECT AccountName,VerticalName,Avg(Value) As Revenue
                from AccountMonthMasterData ammd
                JOIN AccountMonthVerticalData amcd on amcd.MasterID=ammd.MasterID
                JOIN Accounts A ON A.AccountID=ammd.AccountID
                WHERE a.BUID='{0}'
                AND MONTHName IN ('{2}')
                AND Year='{1}' and SubVerticalName='% of Rev'
                group BY AVerticalID"
                , BU, Year, string.Join("','", MonthList));
            
            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet AccountRevenueReportBasedOnBU(int BU, int Year, List<string> MonthList)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            StringBuilder stringQuery = new StringBuilder();

            AccountModel B = new AccountModel();
            var accList = B.GetAccountsListwrtBU(BU);

            StringBuilder sb1 = new StringBuilder("");
            StringBuilder sb2 = new StringBuilder("");
            sb1.Append("SELECT ParticularName,SubTypeName");
            
            foreach (Account acc in accList)
            {
                sb1.AppendFormat(@",{0}.v as {0}", acc.AccountName);
                sb2.AppendFormat(@" LEFT JOIN (SELECT amcd.ParticularID, amcd.PARTICULARSUBTYPEID, AVG(Value) v 
                    FROM AccountMonthMasterData ammd
	                JOIN AccountMonthChildData amcd on amcd.MasterID=ammd.MasterID
	                JOIN Accounts A ON A.AccountID=ammd.AccountID
					where monthname in ('{0}') AND AMMD.ACCOUNTID={1} AND YEAR={3}
					group by amcd.PARTICULARSUBTYPEID )	{2} ON {2}.ParticularID=P.ParticularSID AND
	                {2}.PARTICULARSUBTYPEID=PST.PARTICULARSSUBTYPEID",
                        string.Join("','",MonthList),acc.AccountID,acc.AccountName,Year);
            }
            sb1.Append(@" FROM PARTICULARS P
	                JOIN PARTICULARSSubType PST ON PST.ParticularID=P.ParticularSID");
            sb1.Append(sb2);

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, sb1.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }
        
        public DataSet MarginPerEmployeeYTD(int? BU, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }

            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT AccountName,EmployeeID,
                EmployeeName,AVG(Salary) AS Salary ,AVG(Revenue) AS Revenue, 
                ROUND((AVG(Revenue) - AVG(Salary))/CAST(AVG(Revenue) AS real) *100,2) AS GrossMargin 
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                WHERE ACRD.ACCOUNTID in ('{0}')
                AND Year='{1}'
                GROUP BY EMPLOYEEID,AccountName", 
               string.Join("','", stringAccList), Year);              

            }
            else
            {
                stringQuery.AppendFormat(@"SELECT EmployeeID,EmployeeName,AVG(SALARY) S,AVG(Revenue) R,a.aCCOUNTNAME,
                COALESCE(ROUND((AVG(Revenue)-AVG(SALARY))/AVG(Revenue),2),0) GM
                FROM AccountCostRevenueData ammd	            
                JOIN Accounts A ON A.AccountID=ammd.AccountID	
                JOIN BUACCOUNTMapping BAM ON BAM.AccountID=A.AccountID
                where BAM.BUID='{0}'
                AND YEAR='{1}'	
                GROUP BY EmployeeID,EmployeeName,aCCOUNTNAME
                Order by AccountName,EmployeeName",BU,Year);

            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet GroupWiseMarginReport(int? BU, List<string> MonthList, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }
            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT AccountName,
                GroupName,AVG(Salary) AS Salary ,AVG(Revenue) AS Revenue, 
                ROUND((AVG(Revenue) - AVG(Salary))/CAST(AVG(Revenue) AS real),2) AS GrossMargin 
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                WHERE ACRD.ACCOUNTID IN ('{0}')
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY GroupName,AccountName",
               string.Join("','", stringAccList), Year,string.Join("','", MonthList));
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT B.BUName,AccountName,GroupName,
                AVG(Salary) AS Salary ,AVG(Revenue) AS Revenue, 
                ROUND((AVG(Revenue) - AVG(Salary))/CAST(AVG(Revenue) AS real),2) AS GrossMargin 
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                JOIN BU B ON B.BUID=A.BUID
                WHERE B.BUID ='{0}'
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY GroupName,AccountName", BU, Year, string.Join("','", MonthList));
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet GroupWiseRevenueReport(int? BU, List<string> MonthList, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }

            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT AccountName,
                GroupName,AVG(Revenue) AS Revenue
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                WHERE ACRD.ACCOUNTID IN ('{0}')
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY GroupName,AccountName",
               string.Join("','", stringAccList), Year, string.Join("','", MonthList));
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT B.BUName,AccountName,GroupName,
                AVG(Revenue) AS Revenue                
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                JOIN BU B ON B.BUID=A.BUID
                WHERE B.BUID ='{0}'
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY GroupName,AccountName", BU, Year, string.Join("','", MonthList));
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet ManagerWiseMarginReport(int? BU, List<string> MonthList, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }
            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT AccountName,
                ManagerName,AVG(Salary) AS Salary ,AVG(Revenue) AS Revenue, 
                ROUND((AVG(Revenue) - AVG(Salary))/CAST(AVG(Revenue) AS real),2) AS GrossMargin 
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                WHERE ACRD.ACCOUNTID IN ('{0}')
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY ManagerName,AccountName",
               string.Join("','", stringAccList), Year, string.Join("','", MonthList));
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT B.BUName,AccountName,ManagerName,
                AVG(Salary) AS Salary ,AVG(Revenue) AS Revenue, 
                ROUND((AVG(Revenue) - AVG(Salary))/CAST(AVG(Revenue) AS real),2) AS GrossMargin 
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                JOIN BU B ON B.BUID=A.BUID
                WHERE B.BUID ='{0}'
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY ManagerName,AccountName", BU, Year, string.Join("','", MonthList));
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        public DataSet ManagerWiseRevenueReport(int? BU, List<string> MonthList, List<Account> AccountList, int Year)
        {
            IDBManager dbManager = new DBManager(DataProvider.SQLite);
            dbManager.ConnectionString = BaseDbContext.databasestring;
            if (BU != null)
            {
                AccountModel acc = new AccountModel();
                AccountList = acc.GetAccountsListwrtBU((int)BU);
            }
            List<string> stringAccList = new List<string>();
            foreach (Account a in AccountList)
                stringAccList.Add(a.AccountID.ToString());

            StringBuilder stringQuery = new StringBuilder();
            if (BU == null)
            {
                stringQuery.AppendFormat(@"SELECT AccountName,
                ManagerName,AVG(Revenue) AS Revenue
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                WHERE ACRD.ACCOUNTID IN ('{0}')
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY ManagerName,AccountName",
               string.Join("','", stringAccList), Year, string.Join("','", MonthList));
            }
            else
            {
                stringQuery.AppendFormat(@"SELECT B.BUName,AccountName,ManagerName,
                AVG(Revenue) AS Revenue                
                from AccountCostRevenueData ACRD
                JOIN Accounts A ON A.AccountID=ACRD.AccountID
                JOIN BU B ON B.BUID=A.BUID
                WHERE B.BUID ='{0}'
                AND MonthName in ('{2}')
                AND Year='{1}'
                GROUP BY ManagerName,AccountName", BU, Year, string.Join("','", MonthList));
            }

            try
            {
                dbManager.Open();
                var DS = dbManager.ExecuteDataSet(CommandType.Text, stringQuery.ToString());
                return DS;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                dbManager.Close();
            }
        }

        #endregion
    }
}
