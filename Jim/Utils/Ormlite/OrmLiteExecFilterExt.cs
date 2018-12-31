using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using ServiceStack.OrmLite;

namespace Jim
{
    public class OrmLiteExecFilterExt : OrmLiteExecFilter
    {
        //private ILog log = LoggerHelper.GetLogger("OrmLiteExecFilterExt", "ADOExchange");
        /// <summary>
        /// 记录最后一次执行的完整SQL语句
        /// </summary>
        /// <param name="dbCmd"></param>
        /// <param name="dbConn"></param>
        /// <remarks>框架默认通过IDbConnection访问最后一次执行的SQL语句中不包含参数信息</remarks>
        public override void DisposeCommand(IDbCommand dbCmd, IDbConnection dbConn)
        {
            if (dbCmd == null) return;
            StringBuilder sql = new StringBuilder();
            sql.Append("SQL: ").Append(dbCmd.CommandText);
            if (dbCmd.Parameters.Count > 0)
            {
                sql.AppendLine()
                  .Append("PARAMS: ");
                for (int i = 0; i < dbCmd.Parameters.Count; i++)
                {
                    var p = (IDataParameter)dbCmd.Parameters[i];
                    if (i > 0)
                        sql.Append(", ");
                    sql.AppendFormat("{0}={1}", p.ParameterName, p.Value);
                }
            }
            dbConn.SetLastCommandText(sql.ToString());
            //System.Diagnostics.Debug.WriteLine(sql.ToString());
            //log.Info(sql.ToString());
            dbCmd.Dispose();
        }
    }
}