using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using ServiceStack.Data;
using ServiceStack.OrmLite;


namespace Jim
{
    public class OrmClient : IOrmClient,IDisposable
    {
        private IDbConnectionFactory _dbFactory;
        private IOrmLiteDialectProvider _dialectProvider;
        private int _dbConnectionDepth;
        private IDbConnection _dbConnection;
        private ILog log = LoggerHelper.GetAdoNetLogger("OrmClient", "ADOExchange");


        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection = null;
            }
            if (_dbFactory != null)
            {
                _dbFactory = null;
            }
        }

        public OrmClient()
        {
            string sqlServerConnStr = ConfigurationManager.ConnectionStrings["DBContext"].ToString();
            this._dialectProvider = SqlServerDialect.Provider;
            _dbFactory = new OrmLiteConnectionFactory(sqlServerConnStr, _dialectProvider);
            OrmLiteConfig.ExecFilter = new OrmLiteExecFilterExt();
            OrmLiteConfig.InsertFilter = (dbCmd, row) =>
            {
                var auditRow = row as DBEntity;
                auditRow.ID = Guid.NewGuid().ToString().Replace("-","");
                if (auditRow != null)
                    auditRow.CreatedDate = auditRow.ModifyDate = DateTime.Now;
                if (auditRow != null && auditRow.ModifyBy == null)
                    throw new ArgumentNullException("没有修改人员");
            };
            OrmLiteConfig.UpdateFilter = (dbCmd, row) =>
            {
                var auditRow = row as DBEntity;
                if (auditRow != null)
                    auditRow.ModifyDate = DateTime.Now;
                if (auditRow != null && auditRow.ModifyBy == null)
                    throw new ArgumentNullException("没有修改人员");
            };


        }

        public OrmClient(string connectionString, IOrmLiteDialectProvider dialectProvider)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (dialectProvider == null)
                throw new ArgumentNullException("dialectProvider");
            _dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider);
            this._dialectProvider = dialectProvider;
            OrmLiteConfig.ExecFilter = new OrmLiteExecFilterExt();
            OrmLiteConfig.InsertFilter = (dbCmd, row) =>
            {
                var auditRow = row as DBEntity;
                if (auditRow != null)
                    auditRow.CreatedDate = auditRow.ModifyDate = DateTime.Now;
                if (auditRow != null && auditRow.ModifyBy == null)
                    throw new ArgumentNullException("没有修改人员");
            };
            OrmLiteConfig.UpdateFilter = (dbCmd, row) =>
            {
                var auditRow = row as DBEntity;
                if (auditRow != null)
                    auditRow.ModifyDate = DateTime.Now;
            };
        }

        public T Execute<T>(Func<IDbConnection, T> action)
        {
            try
            {
                OpenDBConnection();
                var result = action(_dbConnection);
                string debugMessage = string.Format("数据库操作，最后执行的SQL语句为：{0}", _dbConnection.GetLastSql());
                log.Debug(Util.InitLogMessage("OrmClient", "Execute", debugMessage, action.Target));
                System.Diagnostics.Debug.WriteLine(debugMessage);
                return result;
            }
            catch (Exception x)
            {
                string errMessage = string.Format("数据库操作异常，最后执行的SQL语句为：{0}，异常信息为：{1}", _dbConnection.GetLastSql(), x.ToString());
                log.Error(Util.InitLogMessage("OrmClient", "Execute", errMessage, action.Target),x);
                System.Diagnostics.Debug.WriteLine(errMessage);
                return default(T);
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public void Execute(Action<IDbConnection> action)
        {
            try
            {
                OpenDBConnection();
                action(_dbConnection);
                string debugMessage = string.Format("数据库操作，最后执行的SQL语句为：{0}", _dbConnection.GetLastSql());
                log.Debug(Util.InitLogMessage("OrmClient", "Execute", debugMessage, action.Target));
                System.Diagnostics.Debug.WriteLine(debugMessage);
            }
            catch (Exception x)
            {
                string errMessage = string.Format("数据库操作异常，最后执行的SQL语句为：{0}，异常信息为：{1}", _dbConnection.GetLastSql(), x.ToString());
                log.Error(Util.InitLogMessage("OrmClient", "Execute", errMessage, action.Target),x);
                System.Diagnostics.Debug.WriteLine(errMessage);
                //errMessage.PrintDump();
                //x.PrintDump();
            }
            finally
            {
                CloseDBConnection();
            }
        }

        public bool TableExists(string tableName, string schema = null)
        {
            return Execute(db => db.TableExists(tableName, schema));
        }

        public bool TableExists<T>()
        {
            return Execute(db => db.TableExists<T>());
        }

        public bool ColumnExists(string columnName, string tableName, string schema = null)
        {
            return Execute(db => db.ColumnExists(columnName, tableName, schema));
        }

        public void CreateTables(bool overwrite, params Type[] tableTypes)
        {
            Execute(db => db.CreateTables(overwrite,tableTypes));
        }

        public void CreateTable(bool overwrite, Type modelType)
        {
            Execute(db => db.CreateTables(overwrite, modelType));
        }

        public void CreateTableIfNotExists(params Type[] tableTypes)
        {
            Execute(db => db.CreateTableIfNotExists(tableTypes));
        }

        public void DropAndCreateTables(params Type[] tableTypes)
        {
            Execute(db => db.DropAndCreateTables(tableTypes));
        }

        public bool CreateTableIfNotExists<T>()
        {
            return Execute(db => db.CreateTableIfNotExists<T>());
        }

        public void CreateTable<T>(bool overwrite = false)
        {
            Execute(db => db.CreateTable<T>(overwrite));
        }

        public bool CreateTableIfNotExists(Type modelType)
        {
            return Execute(db => db.CreateTableIfNotExists(modelType));
        }

        public void DropAndCreateTable<T>()
        {
            Execute(db => db.DropAndCreateTable<T>());
        }

        public void DropAndCreateTable(Type modelType)
        {
            Execute(db => db.DropAndCreateTable(modelType));
        }

        public void DropTables(params Type[] tableTypes)
        {
            Execute(db => db.DropTables(tableTypes));
        }

        public void DropTable(Type modelType)
        {
            Execute(db => db.DropTable(modelType));
        }



        public void DropTable<T>()
        {
            Execute(db => db.DropTable<T>());
        }

        public SqlExpression<T> From<T>() where T : DBEntity, new()
        {
            return _dialectProvider.SqlExpression<T>();
        }

        public long Insert<T>(T entity, bool selectIdentity = false) where T : DBEntity, new()
        {
            if (entity == null)
                return 0L;
            return Execute<long>(db => db.Insert<T>(entity, selectIdentity));
        }

        public void InsertAll<T>(IEnumerable<T> entities) where T : DBEntity, new()
        {
            if (entities != null && entities.Any<T>())
            {
                Execute(db => db.InsertAll(entities));
            }
        }

        public long Update<T>(T entity) where T : DBEntity, new()
        {
            if (entity == null)
                return 0L;
            return Execute<long>(db => db.Update<T>(entity));
        }

        public long Update<T>(T entity, Expression<Func<T, bool>> where) where T : DBEntity, new()
        {
            if (entity == null)
                return 0L;
            return Execute<long>(db => db.Update<T>(entity, where));
        }

        public long Update<T>(T entity, Expression<Func<T, object>> onlyFields = null, Expression<Func<T, bool>> where = null) where T : DBEntity, new()
        {
            return Execute(db => db.UpdateOnly<T>(entity, onlyFields, where));
        }

        public void UpdateAll<T>(IEnumerable<T> entities) where T : DBEntity, new()
        {
            if (entities != null && entities.Any<T>())
            {
                Execute(db => db.UpdateAll<T>(entities));
            }
        }

        public int UpdateOnly<T>(Expression<Func<T>> updateFields, Expression<Func<T, bool>> where = null, Action<IDbCommand> commandFilter = null)
        {
           return Execute(db => db.UpdateOnly<T>(updateFields, where, commandFilter));
        }

        public int UpdateOnly<T>(T model, SqlExpression<T> onlyFields,Action<IDbCommand> commandFilter = null)
        {
            return Execute(db => db.UpdateOnly<T>(model,onlyFields, commandFilter));
        }

        public int UpdateOnly<T>(Expression<Func<T>> updateFields, SqlExpression<T> q,Action<IDbCommand> commandFilter = null)
        {
            return Execute(db => db.UpdateOnly<T>(updateFields, q, commandFilter));
        }

        public int UpdateOnly<T>(T obj, Expression<Func<T, object>> onlyFields = null,Expression<Func<T, bool>> where = null, Action<IDbCommand> commandFilter = null)
        {
            return Execute(db => db.UpdateOnly<T>(obj, onlyFields, where, commandFilter));
        }

        public int UpdateOnly<T>(T obj, string[] onlyFields, Expression<Func<T, bool>> where = null, Action<IDbCommand> commandFilter = null)
        {
            return Execute(db => db.UpdateOnly<T>(obj, onlyFields, where, commandFilter));
        }

        public int UpdateAdd<T>(Expression<Func<T>> updateFields,Expression<Func<T, bool>> where = null, Action<IDbCommand> commandFilter = null)
        {
            return Execute(db => db.UpdateAdd<T>(updateFields, where, commandFilter));
        }

        public int UpdateAdd<T>(Expression<Func<T>> updateFields, SqlExpression<T> q,Action<IDbCommand> commandFilter = null)
        {
            return Execute(db => db.UpdateAdd<T>(updateFields, q, commandFilter));
        }

        public int UpdateNonDefaults<T>(T item, Expression<Func<T, bool>> obj)
        {
            return Execute(db => db.UpdateNonDefaults<T>(item, obj));
        }

        public long Delete<T>(SqlExpression<T> where) where T : DBEntity, new()
        {
            return Execute(db => db.Delete<T>(where));
        }

        public long Delete<T>(Expression<Func<T, bool>> where) where T : DBEntity, new()
        {
            return Execute(db => db.Delete<T>(where));
        }

        public List<Tuple<T, T2>> SelectMulti<T, T2>(SqlExpression<T> expression)
        {
            return Execute(db => db.SelectMulti<T, T2>(expression));
        }

        public List<Tuple<T, T2,T3>> SelectMulti<T, T2,T3>(SqlExpression<T> expression)
        {
            return Execute(db => db.SelectMulti<T, T2,T3>(expression));
        }

        public List<Tuple<T, T2, T3, T4>> SelectMulti<T, T2, T3, T4>(SqlExpression<T> expression)
        {
            return Execute(db => db.SelectMulti<T, T2, T3, T4>(expression));
        }

        public List<Tuple<T, T2, T3, T4, T5>> SelectMulti<T, T2, T3, T4, T5>(SqlExpression<T> expression)
        {
            return Execute(db => db.SelectMulti<T, T2, T3, T4, T5>(expression));
        }

        public List<Tuple<T, T2, T3, T4, T5, T6>> SelectMulti<T, T2, T3, T4, T5, T6>(SqlExpression<T> expression)
        {
            return Execute(db => db.SelectMulti<T, T2, T3, T4, T5, T6>(expression));
        }

        public List<Tuple<T, T2, T3, T4, T5, T6, T7>> SelectMulti<T, T2, T3, T4, T5, T6, T7>(SqlExpression<T> expression)
        {
            return Execute(db => db.SelectMulti<T, T2, T3, T4, T5, T6, T7>(expression));
        }


        public List<T> Select<T>() where T : DBEntity, new()
        {
            return Execute(db => db.Select<T>());
        }

        public List<T> Select<T>(SqlExpression<T> expression) where T : DBEntity, new()
        {
            return Execute(db => db.Select<T>(expression));
        }

        public List<T> Select<T>(Expression<Func<T, bool>> predicate) where T : DBEntity, new()
        {
            return Execute(db => db.Select<T>(predicate));
        }

        public List<T> SelectAsync<T>(Expression<Func<T, bool>> predicate) where T : DBEntity, new()
        {
            return Execute(db => db.Select<T>(predicate));
        }

        public IEnumerable<T> SelectLazy<T>(SqlExpression<T> expression) where T : DBEntity, new()
        {
            return Execute(db => db.SelectLazy<T>(expression));
        }


        public T Single<T>(object anonType) where T : DBEntity, new()
        {
            return Execute(db => db.Single<T>(anonType));
        }

        public T Single<T>(Expression<Func<T, bool>> predicate) where T : DBEntity, new()
        {
            return Execute(db => db.Single<T>(predicate));
        }
        public T Single<T>(SqlExpression<T> expression) where T : DBEntity, new()
        {
            return Execute(db => db.Single<T>(expression));
        }
        public T Single<T>(ISqlExpression expression) where T : DBEntity, new()
        {
            return Execute(db => db.Single<T>(expression));
        }

        public T Single<T>(string sql, IEnumerable<IDbDataParameter> sqlParams) where T : DBEntity, new()
        {
            return Execute(db => db.Single<T>(sql,sqlParams));
        }

        public T Single<T>(string sql, object anonType = null) where T : DBEntity, new()
        {
            return Execute(db => db.Single<T>(sql, anonType));
        }

        public T SingleById<T>(object idValue) where T : DBEntity, new()
        {
            return Execute(db => db.SingleById<T>(idValue));
        }

        public T SingleWhere<T>(string name, object value) where T : DBEntity, new()
        {
            return Execute(db => db.SingleWhere<T>(name, value));
        }


        public long Count<T>(SqlExpression<T> expression) where T : DBEntity, new()
        {
            return Execute(db => db.Count<T>(expression));
        }

        public bool Exist<T>(SqlExpression<T> expression) where T : DBEntity, new()
        {
            return Execute(db => db.Exists<T>(expression));
        }

        public T Scalar<T>(SqlExpression<T> expression) where T : DBEntity, new()
        {
            return Execute(db => db.Scalar<T>(expression));
        }

        public int ExecuteSql(string sql, object dbParams = null)
        {
            if (dbParams == null)
                return Execute(db => db.ExecuteSql(sql));
            return Execute(db => db.ExecuteSql(sql, dbParams));
        }

        #region 事物

        public bool InvokeTransaction(IEnumerable<Func<long>> actions)
        {
            return InvokeTransaction(() =>
            {
                foreach (var action in actions)
                {
                    if (action() <= 0)
                        return false;
                }
                return true;
            });
        }

        public bool InvokeTransaction(Func<bool> action)
        {
            return Execute(db =>
            {
                bool isTranSuccess = false;
                using (var trans = db.BeginTransaction())
                {
                    try
                    {
                        isTranSuccess = action();
                    }
                    catch (Exception x)
                    {
                        string errorMessage = string.Format("事务操作异常，最后执行的SQL语句为：{0}，异常信息为：{1}", db.GetLastSql(), x.ToString());
                        log.Error(errorMessage);
                    }
                    if (isTranSuccess)
                        trans.Commit();
                    else
                        trans.Rollback();
                }
                return isTranSuccess;
            });
        }

        public bool InvokeTransaction(Func<bool> action, IsolationLevel isoLevel)
        {
            return Execute(db =>
            {
                bool isTranSuccess = false;
                using (var trans = db.BeginTransaction(isoLevel))
                {
                    try
                    {
                        isTranSuccess = action();
                    }
                    catch (Exception x)
                    {
                        string errorMessage = string.Format("事务操作异常，最后执行的SQL语句为：{0}，异常信息为：{1}", db.GetLastSql(), x.ToString());
                        log.Error(errorMessage);
                    }
                    if (isTranSuccess)
                        trans.Commit();
                    else
                        trans.Rollback();
                }
                return isTranSuccess;
            });
        }

        #endregion

        #region DBConnection Manager
        private void OpenDBConnection()
        {
            if (_dbConnectionDepth == 0)
            {
                _dbConnection = _dbFactory.CreateDbConnection();

                if (_dbConnection.State == ConnectionState.Broken)
                    _dbConnection.Close();
                else if (_dbConnection.State == ConnectionState.Closed)
                    _dbConnection.Open();
            }
            _dbConnectionDepth++;
        }

        private void CloseDBConnection()
        {
            if (_dbConnectionDepth > 0)
            {
                _dbConnectionDepth--;
                if (_dbConnectionDepth == 0)
                {
                    _dbConnection.Dispose();
                    _dbConnection = null;
                }
            }
        }
        #endregion

    }
}