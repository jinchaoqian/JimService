using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Jim;
using log4net;
using log4net.Core;
using ServiceStack.Logging;

namespace ServiceStack.Logging.Log4Net
{

    /// <summary>
    /// ServiceStack的日志工厂
    /// </summary>
    public class MyLoggerFactory : ILogFactory
    {
        public ILog GetLogger(Type type)
        {
            return new MyLog4NetLogger(type);
        }

        public ILog GetLogger(string typeName)
        {
            return new MyLog4NetLogger(typeName);
        }
    }

    public class MyLog4NetLogger : ILogWithContext, ILogWithException, ILog
    {
        private readonly log4net.ILog _log= LoggerHelper.GetLogger(typeof(MyLog4NetLogger), "MyLog", Level.All);

        public MyLog4NetLogger()
        {
            LoggerHelper.GetLogger(typeof(MyLog4NetLogger), "MyLog", Level.All);
        }

        public MyLog4NetLogger(string typeName)
        {
            LoggerHelper.GetLogger(typeName,"MyLog",Level.All);
        }

        public MyLog4NetLogger(Type type)
        {
            LoggerHelper.GetLogger(type, "MyLog", Level.All);
        }

        public bool IsDebugEnabled { get; set; }

        public void Debug(Exception exception, string format, params object[] args)
        {
            if (!this._log.IsDebugEnabled)
                return;
            this._log.DebugFormat(format, args);
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format,args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            if (!this._log.IsErrorEnabled)
                return;
            this._log.ErrorFormat(format, args);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message,exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format,args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            if (!this._log.IsFatalEnabled)
                return;
            this._log.FatalFormat(format, args);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message,exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format,args);
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            if (!this._log.IsInfoEnabled)
                return;
            this._log.InfoFormat(format, args);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format,args);
        }

        public IDisposable PushProperty(string key, object value)
        {
            LogicalThreadContext.Properties[key] = value;
            return (IDisposable)new RemovePropertyOnDispose(key);
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            if (!this._log.IsWarnEnabled)
                return;
            this._log.WarnFormat(format, args);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }

        private class RemovePropertyOnDispose : IDisposable
        {
            private readonly string _removeKey;

            public RemovePropertyOnDispose(string removeKey)
            {
                this._removeKey = removeKey;
            }

            public void Dispose()
            {
                LogicalThreadContext.Properties.Remove(this._removeKey);
            }
        }
    }
}