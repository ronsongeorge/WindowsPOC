using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public sealed class DBManagerFactory
  {
    private DBManagerFactory(){}
    public static IDbConnection GetConnection(DataProvider
     providerType)
    {
      IDbConnection iDbConnection = null;
      switch (providerType)
      {
        case DataProvider.SqlServer:
          iDbConnection = new SqlConnection();
          break;
        case DataProvider.SQLite:
          iDbConnection = new System.Data.SQLite.SQLiteConnection(BaseDbContext.databasestring);
          break; 
        default:
          return null;
      }
      return iDbConnection;
    }
 
    public static IDbCommand GetCommand(DataProvider providerType)
    {
      switch (providerType)
      {
        case DataProvider.SqlServer:
          return new SqlCommand();
        case DataProvider.SQLite:
          return new SQLiteCommand(); 
        default:
          return null;
      }
    }
 
    public static IDbDataAdapter GetDataAdapter(DataProvider
    providerType)
    {
      switch (providerType)
      {
        case DataProvider.SqlServer:
          return new SqlDataAdapter();
        case DataProvider.SQLite:
          return new SQLiteDataAdapter();
        default:
          return null;
      }
    }
 
    public static IDbTransaction GetTransaction(DataProvider
     providerType)
    {
      IDbConnection iDbConnection =GetConnection(providerType);
      IDbTransaction iDbTransaction =iDbConnection.BeginTransaction();
      return iDbTransaction;
    }
 
    public static IDataParameter GetParameter(DataProvider
     providerType)
    {
      IDataParameter iDataParameter = null;
      switch (providerType)
      {
        case DataProvider.SqlServer:
          iDataParameter = new SqlParameter();
          break;
        case DataProvider.SQLite:
          iDataParameter = new SQLiteParameter();
          break; 
      }
      return iDataParameter;
    }
 
    public static IDbDataParameter[] GetParameters(DataProvider
     providerType,
      int paramsCount)
    {
      IDbDataParameter[]idbParams = new IDbDataParameter[paramsCount];
 
      switch (providerType)
      {
        case DataProvider.SqlServer:
          for (int i = 0; i < paramsCount;++i)
          {
            idbParams[i] = new SqlParameter();
          }
          break;
        case DataProvider.SQLite:
          for (int i = 0; i < paramsCount; ++i)
          {
              idbParams[i] = new SQLiteParameter();
          }
          break; 
        default:
          idbParams = null;
          break;
      }
      return idbParams;
    }
  }
}
