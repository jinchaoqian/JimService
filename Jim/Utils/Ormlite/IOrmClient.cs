using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Jim
{
    public interface IOrmClient
    {

        /// <summary>
        ///     执行一个数据库操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        T Execute<T>(Func<IDbConnection, T> action);
        /// <summary>
        ///     执行一个数据库操作
        /// </summary>
        /// <param name="action"></param>
        void Execute(Action<IDbConnection> action);

        bool TableExists(string tableName, string schema = null);

        bool TableExists<T>();

        bool ColumnExists( string columnName, string tableName, string schema = null);
        
        void CreateTables(bool overwrite, params Type[] tableTypes);

        void CreateTable(bool overwrite, Type modelType);

        void CreateTableIfNotExists(params Type[] tableTypes);

        void DropAndCreateTables(params Type[] tableTypes);

        void CreateTable<T>(bool overwrite = false);

        bool CreateTableIfNotExists<T>();
        bool CreateTableIfNotExists( Type modelType);

        void DropAndCreateTable<T>();

        void DropAndCreateTable(Type modelType);

        void DropTables(params Type[] tableTypes);

        void DropTable(Type modelType);

        void DropTable<T>();

        /// <summary>
        /// 　　获取一个表的SqlExpression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        SqlExpression<T> From<T>() where T : DBEntity, new();
        /// <summary>
        ///     新增方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="selectIdentity">主键为自增长列时，返回新增数据的主键值</param>
        /// <returns></returns>
        long Insert<T>(T entity, bool selectIdentity = false) where T : DBEntity, new();
        /// <summary>
        ///     批量新增方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void InsertAll<T>(IEnumerable<T> entities) where T : DBEntity, new();
        /// <summary>
        ///     更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        long Update<T>(T entity) where T : DBEntity, new();
        /// <summary>
        ///     更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        long Update<T>(T entity, Expression<Func<T, bool>> where) where T : DBEntity, new();
        /// <summary>
        ///     更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyFields"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <remarks>更新一行数据的部分字段</remarks>
        long Update<T>(T entity, Expression<Func<T, object>> onlyFields = null, Expression<Func<T, bool>> where = null) where T : DBEntity, new();
        /// <summary>
        ///     批量更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void UpdateAll<T>(IEnumerable<T> entities) where T : DBEntity, new();

        int UpdateOnly<T>(Expression<Func<T>> updateFields, Expression<Func<T, bool>> where = null,Action<IDbCommand> commandFilter = null);

        int UpdateOnly<T>(T model, SqlExpression<T> onlyFields, Action<IDbCommand> commandFilter = null);

        int UpdateOnly<T>(Expression<Func<T>> updateFields, SqlExpression<T> q, Action<IDbCommand> commandFilter = null);

        int UpdateOnly<T>(T obj, Expression<Func<T, object>> onlyFields = null, Expression<Func<T, bool>> where = null,
            Action<IDbCommand> commandFilter = null);

        int UpdateOnly<T>(T obj, string[] onlyFields, Expression<Func<T, bool>> where = null,
            Action<IDbCommand> commandFilter = null);

        int UpdateAdd<T>(Expression<Func<T>> updateFields, Expression<Func<T, bool>> where = null,
            Action<IDbCommand> commandFilter = null);

        int UpdateAdd<T>(Expression<Func<T>> updateFields, SqlExpression<T> q, Action<IDbCommand> commandFilter = null);
        int UpdateNonDefaults<T>(T item, Expression<Func<T, bool>> obj);


        /// <summary>
        ///     删除方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        long Delete<T>(SqlExpression<T> where) where T : DBEntity, new();
        /// <summary>
        ///     删除方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        long Delete<T>(Expression<Func<T, bool>> where) where T : DBEntity, new();

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2">The type of the t2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>List&lt;Tuple&lt;T, T2&gt;&gt;.</returns>
        List<Tuple<T, T2>> SelectMulti<T, T2>(SqlExpression<T> expression);
        List<Tuple<T, T2, T3>> SelectMulti<T, T2, T3>(SqlExpression<T> expression);
        List<Tuple<T, T2, T3, T4>> SelectMulti<T, T2, T3, T4>(SqlExpression<T> expression);
        List<Tuple<T, T2, T3, T4, T5>> SelectMulti<T, T2, T3, T4, T5>(SqlExpression<T> expression);
        List<Tuple<T, T2, T3, T4, T5, T6>> SelectMulti<T, T2, T3, T4, T5, T6>(SqlExpression<T> expression);
        List<Tuple<T, T2, T3, T4, T5, T6, T7>> SelectMulti<T, T2, T3, T4, T5, T6, T7>(SqlExpression<T> expression);
        /// <summary>
        ///     查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<T> Select<T>() where T : DBEntity, new();
        /// <summary>
        ///     查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        List<T> Select<T>(SqlExpression<T> expression) where T : DBEntity, new();
        /// <summary>
        ///     查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<T> Select<T>(Expression<Func<T, bool>> predicate) where T : DBEntity, new();
        /// <summary>
        ///     查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> SelectLazy<T>(SqlExpression<T> expression) where T : DBEntity, new();


        T Single<T>(Expression<Func<T, bool>> predicate) where T : DBEntity, new();
        T Single<T>(SqlExpression<T> expression) where T : DBEntity, new();
        T Single<T>(ISqlExpression expression) where T : DBEntity, new();
        T Single<T>(object anonType) where T : DBEntity, new();
        T Single<T>(string sql, IEnumerable<IDbDataParameter> sqlParams) where T : DBEntity, new();
        T Single<T>(string sql, object anonType = null) where T : DBEntity, new();
        T SingleById<T>(object idValue) where T : DBEntity, new();

        /// <summary>
        ///     统计方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        long Count<T>(SqlExpression<T> expression) where T : DBEntity, new();
        /// <summary>
        ///     查询指定条件的数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exist<T>(SqlExpression<T> expression) where T : DBEntity, new();
        /// <summary>
        ///     读取满足条件的第一行第一列的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T Scalar<T>(SqlExpression<T> expression) where T : DBEntity, new();
        /// <summary>
        ///     执行一个语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSql(string sql, object dbParams = null);
        /// <summary>
        ///     在事物中执行一个批操作
        /// </summary>
        /// <param name="actions"></param>
        /// <returns>事物是否执行成功</returns>
        bool InvokeTransaction(IEnumerable<Func<long>> actions);
        /// <summary>
        ///     在事物中执行一个批操作
        /// </summary>
        /// <param name="action"></param>
        /// <returns>事物是否成功</returns>
        bool InvokeTransaction(Func<bool> action);
        /// <summary>
        ///     在事物中执行一个批操作
        /// </summary>
        /// <param name="action"></param>
        /// <returns>事物是否成功</returns>
        bool InvokeTransaction(Func<bool> action, IsolationLevel isoLevel);
    }
}
