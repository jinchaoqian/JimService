using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Jim
{
    public class DBHelper
    {
        static string conStr = ConfigurationManager.ConnectionStrings["RegentContext"].ToString();
        static string sqlServerConnStr = ConfigurationManager.ConnectionStrings["DbContext"].ToString();

        public static OracleConnection GetOpenConnection()
        {
            var con = new OracleConnection(conStr);
            con.Open();
            return con;
        }

        public static SqlConnection GetSqlServerConnection()
        {
            var con = new SqlConnection(sqlServerConnStr);
            con.Open();
            return con;
        }
        
    }
}
