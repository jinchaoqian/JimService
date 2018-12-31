using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Oracle.ManagedDataAccess.Client;
using ServiceStack;

namespace Jim
{
    public class CallProcedure
    {
        //private static ILog logger = LoggerHelper.GetLogger(MethodBase.GetCurrentMethod().DeclaringType, "巨益会员通接口");

        //public static string AddMember(OracleConnection conn, OracleTransaction trans, string mobile, string name, int sex, string birthday, string cardNo, string gradeCode, string storeCode, string taobaoid)
        //{
        //    try
        //    {

        //        logger.Debug(string.Format("PRO_ADD_MEMBER  '{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}'", mobile, name, sex, birthday, cardNo, gradeCode, storeCode, taobaoid));
                
        //        DynamicParameters dp = new DynamicParameters();
        //        dp.Add("mobile", mobile);
        //        dp.Add("name", name);
        //        dp.Add("sex", sex);
        //        dp.Add("birthday", birthday);
        //        dp.Add("cardNo", string.Format("E{0}", cardNo));
        //        dp.Add("gradeCode", gradeCode);
        //        dp.Add("storeCode", storeCode);
        //        dp.Add("v_taobaoid", taobaoid);
        //        dp.Add("outresult", "1", DbType.String, ParameterDirection.Output);

        //        conn.Execute("PRO_ADD_MEMBER", dp, null, null, CommandType.StoredProcedure);
        //        string outresult = dp.Get<string>("outresult");
        //        return outresult;
        //    }
        //    catch (Exception e)
        //    {
                
        //        throw e;
        //    }
        //}

        //public static string PointChanged(OracleConnection conn, OracleTransaction trans, string mobile, long pointMod)
        //{
        //    try
        //    {

        //        logger.Debug(string.Format("PRO_Modify_integral  '{0}','{1}'", mobile, pointMod));
                
        //        DynamicParameters dp = new DynamicParameters();
        //        dp.Add("v_mobile", mobile);
        //        dp.Add("v_integral", pointMod);
        //        dp.Add("v_errorMsg", "", DbType.String, ParameterDirection.Output);

        //        conn.Execute("PRO_Modify_integral", dp, null, null, CommandType.StoredProcedure);
        //        string errorMsg = dp.Get<string>("v_errorMsg");
        //        return errorMsg;
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //}

    }
}
