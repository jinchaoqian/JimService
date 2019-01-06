using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim.Utils.Sql
{
    public enum DatabaseType
    {
        SqlServer,
        MySql,
        Oracle,
        Sqlite
    }

    public class SqlConnectionFactory
    {
        public static IDbConnection CreateSqlConnection(DatabaseType dbType, string strKey)
        {
            IDbConnection connection = null;
            string strConn = ConfigurationManager.ConnectionStrings[strKey].ConnectionString;

            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(strConn);
                    break;
                case DatabaseType.MySql:
                    throw new ArgumentNullException("未实现该方法");
                //   connection = new MySql.Data.MySqlClient.MySqlConnection(strConn);
                //   break;
                case DatabaseType.Oracle:
                    connection = new Oracle.ManagedDataAccess.Client.OracleConnection(strConn);
                    break;
                case DatabaseType.Sqlite:
                    throw new ArgumentNullException("未实现该方法");
                    //connection = new System.Data.OleDb.OleDbConnection(strConn);
            }
            return connection;
        }
    }
}
