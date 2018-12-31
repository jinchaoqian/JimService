using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim.Utils.Sql
{
    public class Data<T> where T : class
    {
        #region 构造函数

        private static Data<T> _instance;
        public static Data<T> GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Data<T>();
            }
            return _instance;
        }

        #endregion

        ///// <summary>
        ///// 分页列表
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="model"></param>
        ///// <param name="count"></param>
        ///// <param name="context"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public List<T> GetQueryManyForPage(SelectBuilder data, T model, out int count, SqlConnection context = null, IDbTransaction transaction = null)
        //{
        //    if (context == null) context = Db.GetInstance().Context();
        //    string sqlStr = Db.GetInstance().GetSqlForSelectBuilder(data);
        //    string sqlStrCount = Db.GetInstance().GetSqlForTotalBuilder(data);
        //    List<T> list = context.Query<T>(sqlStr, model, transaction).ToList();

        //    //TODO:这里待完善，把查询结果转为数字
        //    count = 1;  //context.ExecuteScalar(sqlStrCount, model, transaction);
        //    return list;
        //}
        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="model"></param>
        ///// <param name="context"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public int Add(string query, T model, SqlConnection context = null, IDbTransaction transaction = null)
        //{
        //    if (context == null) context = Db.GetInstance().Context();
        //    //  query += " ; select last_insert_rowid() newid";
        //    //  string id = context.ExecuteScalar(query, model, transaction).ToString();
        //    //  return ZConvert.StrToInt(id, 0);
        //    int id = context.Execute(query, model, transaction);
        //    return id;
        //}






        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="model"></param>
        ///// <param name="context"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public int Update(string query, T model, SqlConnection context = null, IDbTransaction transaction = null)
        //{
        //    if (context == null) context = Db.GetInstance().Context();
        //    int id = context.Execute(query, model, transaction);
        //    return id;
        //}
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="tablename"></param>
        ///// <param name="wheresql"></param>
        ///// <param name="model"></param>
        ///// <param name="context"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public int Delete(string tablename, string wheresql, T model, SqlConnection context = null, IDbTransaction transaction = null)
        //{
        //    if (context == null) context = Db.GetInstance().Context();
        //    string query = "DELETE FROM  " + tablename + "  WHERE " + wheresql;
        //    int id = context.Execute(query, model, transaction);
        //    return id;
        //}
        ///// <summary>
        ///// 获取单个实体
        ///// </summary>
        ///// <param name="tablename"></param>
        ///// <param name="wheresql"></param>
        ///// <param name="model"></param>
        ///// <param name="context"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public T GetModel(string tablename, string wheresql, T model, SqlConnection context = null, IDbTransaction transaction = null)
        //{
        //    if (context == null) context = Db.GetInstance().Context();
        //    string query = "SELECT  * FROM " + tablename + " WHERE " + wheresql + "  LIMIT 0,1";
        //    T obj = context.Query<T>(query, model, transaction).SingleOrDefault();
        //    return obj;
        //}

        ///// <summary>
        ///// 实体列表
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="model"></param>
        ///// <param name="context"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //public List<T> GetModelList(string query, T model, SqlConnection context = null, IDbTransaction transaction = null)
        //{
        //    if (context == null) context = Db.GetInstance().Context();
        //    List<T> list = context.Query<T>(query, model, transaction).ToList();
        //    return list;
        //}
    }
}