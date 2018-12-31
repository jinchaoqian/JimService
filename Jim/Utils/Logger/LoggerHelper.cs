// ***********************************************************************
// Assembly         : JimLog
// Author           : 金朝钱
// Created          : 12-02-2016
//
// Last Modified By : 金朝钱
// Last Modified On : 12-26-2016
// ***********************************************************************
// <copyright file="LoggerHelper.cs" company="金朝钱">
//     Copyright © 金朝钱
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository;
using log4net.Config;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using ServiceStack;

namespace Jim
{
    /// <summary>
    /// 日志处理类.
    /// 封装了Log4Net的方案，暂时支持写入日志文件，后续可以增加其他类型
    /// 默认根据日期分日志存储，如果超过默认大小，则新增日志文件存储。
    /// </summary>
    public static  class LoggerHelper
    {

       
        /// <summary>
        /// Initializes static members of the <see cref="LoggerHelper" /> class.
        /// </summary>
        static LoggerHelper()
        {
            //CreateLogger("Default");
        }

        #region AdoNetLogger

        /// <summary>
        /// 根据日志类型，创建
        /// </summary>
        /// <param name="methodType">Type of the method.</param>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>ILog.</returns>
        /// <exception cref="Exception">repositoryName实例名称不能为空</exception>
        public static ILog GetAdoNetLogger(Type methodType, string repositoryName = "")
        {
            return GetAdoNetLogger(methodType.GetType().FullName);
        }

        /// <summary>
        /// Gets the ADO net logger.
        /// </summary>
        /// <param name="methodType">Type of the method.</param>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>ILog.</returns>
        /// <exception cref="Exception">repositoryName实例名称不能为空</exception>
        public static ILog GetAdoNetLogger(string methodType, string repositoryName = "")
        {
            if (string.IsNullOrEmpty(repositoryName))
            {
                repositoryName = "DefaultAdoNetLogger";
            }
            try
            {
                LogManager.GetRepository(repositoryName);
            }
            catch (Exception e)
            {
                CreateAdoNetLogger(repositoryName);
            }

            return LogManager.GetLogger(repositoryName, methodType);
        }


        static void CreateAdoNetLogger(string repositoryName)
        {
            ILoggerRepository rep = LogManager.CreateRepository(repositoryName);
            string sqlServerConnStr = ConfigurationManager.ConnectionStrings["DbContext"].ToString();
            AdoNetAppender adoNetAppender = new AdoNetAppender();
            adoNetAppender.BufferSize = -1;
            adoNetAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            adoNetAppender.ConnectionString = sqlServerConnStr;
            adoNetAppender.CommandText = @"INSERT INTO SysLog ([Date],[Level],[Logger],[Message],[Exception],[MesID],[InterFaceNo],[Mac],[IP],[InterFaceName],[ObjectName],[RequestUrl],Data,Status,Response,RequestSystem) 
                                            VALUES (@log_date, @log_level, @logger, @Message, @exception,@MesID,@InterFaceNo,@Mac,@IP,@InterFaceName,@ObjectName,@RequestUrl,@Data,@Status,@Response,@RequestSystem)";
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });

            PatternLayout layout = new MyLayout() { ConversionPattern = "%property{MesID}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MesID", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{InterFaceNo}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@InterFaceNo", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Mac}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Mac", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{IP}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@IP", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{InterFaceName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@InterFaceName", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{ObjectName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ObjectName", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{RequestUrl}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@RequestUrl", DbType = System.Data.DbType.String, Size = 500, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Message}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Data}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Data", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Status}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Status", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });
            
            layout = new MyLayout() { ConversionPattern = "%property{Response}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Response", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });


            layout = new MyLayout() { ConversionPattern = "%property{RequestSystem}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@RequestSystem", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            
            adoNetAppender.ActivateOptions();

            BasicConfigurator.Configure(rep, adoNetAppender);

        }


        public static void NewStringPara(this AdoNetAppender ado, string paraName, string split,int length =80)
        {
            PatternLayout layout = new MyLayout() { ConversionPattern = "%property{".Fmt(paraName).Fmt("}") };
            layout.ActivateOptions();
            ado.AddParameter(new AdoNetAppenderParameter { ParameterName = "".Fmt(split).Fmt(paraName), DbType = System.Data.DbType.String, Size = length, Layout = new Layout2RawLayoutAdapter(layout) });
        }

        public static void NewDatePara(this AdoNetAppender ado, string paraName, string split)
        {
            PatternLayout layout = new MyLayout() { ConversionPattern = "%property{".Fmt(paraName).Fmt("}") };
            layout.ActivateOptions();
            ado.AddParameter(new AdoNetAppenderParameter { ParameterName = "".Fmt(split).Fmt(paraName), DbType = System.Data.DbType.DateTime, Layout = new Layout2RawLayoutAdapter(layout) });
        }



        /// <summary>
        /// Creates the ADO net logger.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        static void CreateAdoNetLogger1(string repositoryName)
        {
            ILoggerRepository rep = LogManager.CreateRepository(repositoryName);

            AdoNetAppender adoNetAppender = new AdoNetAppender();
            adoNetAppender.BufferSize = -1;  
            //adoNetAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            //adoNetAppender.ConnectionString = "data source=10.8.250.162;initial catalog=jst;persist security info=True;user id=sa;password=Eifini@2o17;";
            //adoNetAppender.CommandText = "INSERT INTO Logs ([Date],[Level],[Logger],[Message],[Exception],[UserName],[ModName],[Mac],[IP],[Url],[ActionName],[Name]) VALUES (@log_date, @log_level, @logger, @Message, @exception,@UserName,@ModName,@Mac,@IP,@Url,@ActionName,@Name)";

            adoNetAppender.ConnectionString = "DATA SOURCE=10.8.250.49:1521/regent;PASSWORD=regentdtest;PERSIST SECURITY INFO=True;USER ID=regentd;";
            adoNetAppender.ConnectionType ="System.Data.OracleClient.OracleConnection, System.Data.OracleClient, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            adoNetAppender.CommandText = "INSERT INTO INTERFACELOGS (LOGDATE,LOGLEVEL,LOGGER,LOGEXCEPTION,IP,MAC,MESID,INTERFACENO,REQUESTSYSTEM,INTERFACENAME,OBJECTNAME,REQUESTURL,POSTDATA,STATUS,RESPONSE,MESSAGE) VALUES (:log_date, :log_level, :logger, :exception,:IP,:MAC,:MesID,:InterFaceNo,:RequestSystem,:InterfaceName,:ObjectName,:RequestUrl,:PostData,:Status,:Response,:Message)";
            
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":log_date,", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });

            PatternLayout layout = new MyLayout() { ConversionPattern = "%property{IP}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":IP", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{MAC}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":MAC", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{MesID}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":MesID", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{InterFaceNo}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":InterFaceNo", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{RequestSystem}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":RequestSystem", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{InterfaceName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":InterfaceName", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{ObjectName}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":ObjectName", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{RequestUrl}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":RequestUrl", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{PostData}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":PostData", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Status}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":Status", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            layout = new MyLayout() { ConversionPattern = "%property{Response}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":Response", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });
            
            layout = new MyLayout() { ConversionPattern = "%property{Message}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = ":Message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            adoNetAppender.ActivateOptions();

            BasicConfigurator.Configure(rep, adoNetAppender);

        }

        /// <summary>
        /// Creates the ADO net logger.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        static void CreateAdoNetLogger2(string repositoryName)
        {
            ILoggerRepository rep = LogManager.CreateRepository(repositoryName);

            AdoNetAppender adoNetAppender = new AdoNetAppender();
            adoNetAppender.BufferSize = -1;
            adoNetAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            //adoNetAppender.ConnectionType =
            //    "System.Data.OracleClient.OracleConnection, System.Data.OracleClient, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            adoNetAppender.ConnectionString = "data source=10.8.250.162;initial catalog=jst;persist security info=True;user id=sa;password=Eifini@2o17;";

            //adoNetAppender.CommandText = "INSERT INTO InterFaceLogs (Date,Level,Logger,Exception,IP,MAC,MESID,InterFaceNo,RequestSystem,InterFaceName,ObjectName,RequestUrl,Data,Status,Response,Message) " +
            //                             "VALUES (@log_date, @log_level, @logger, @exception,@IP,@MAC,@MesID,@InterFaceNo,@RequestSystem,@InterfaceName,@ObjectName,@RequestUrl,@PostData,@Status,@Response,@Message)";
            adoNetAppender.CommandText = "INSERT INTO InterFaceLogs (Date,Level,Logger,Exception,Message) " +
                                         "VALUES (@log_date, @log_level, @logger, @exception,@Message)";

            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_date,", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });

            //PatternLayout layout = new MyLayout() { ConversionPattern = "%property{IP}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@IP", DbType = System.Data.DbType.String, Size = 20, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{MAC}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MAC", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{MesID}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MesID", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{InterFaceNo}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@InterFaceNo", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{RequestSystem}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@RequestSystem", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{InterfaceName}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@InterfaceName", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{ObjectName}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ObjectName", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{RequestUrl}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@RequestUrl", DbType = System.Data.DbType.String, Size = 200, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{PostData}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@PostData", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{Status}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Status", DbType = System.Data.DbType.String, Size = 80, Layout = new Layout2RawLayoutAdapter(layout) });

            //layout = new MyLayout() { ConversionPattern = "%property{Response}" };
            //layout.ActivateOptions();
            //adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Response", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            PatternLayout layout = new MyLayout() { ConversionPattern = "%property{Message}" };
            layout.ActivateOptions();
            adoNetAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(layout) });

            adoNetAppender.ActivateOptions();

            BasicConfigurator.Configure(rep, adoNetAppender);

        }
        #endregion

        #region RollingFileAppender
        /// <summary>
        /// 根据版本库获取日志对象
        /// </summary>
        /// <param name="methodType">当前调用日志接口的类名</param>
        /// <param name="repositoryName">Repository（版本库）名称</param>
        /// <returns>ILog.</returns>
        public static ILog GetLogger(Type methodType, string repositoryName = "")
        {

            return GetLogger(methodType.GetType().FullName);
        }

        /// <summary>
        /// 根据版本库获取日志对象
        /// </summary>
        /// <param name="methodType">初始化日志调用的对象</param>
        /// <param name="repositoryName">根据版本库获取日志对象，默认是Default版本库</param>
        /// <returns>ILog.</returns>
        public static ILog GetLogger(string methodType, string repositoryName = "")
        {
            if (string.IsNullOrEmpty(repositoryName))
            {
                repositoryName = "Default";
            }
            try
            {
                LogManager.GetRepository(repositoryName);
            }
            catch (Exception e)
            {
                CreateLogger(repositoryName);
            }

            return LogManager.GetLogger(repositoryName, methodType);
        }


        /// <summary>
        /// 新增日志版本库
        /// Default版本库，支持全部的日志等级
        /// 其他版本库，支持Info和Error日志
        /// </summary>
        /// <param name="repositoryName">需要创建的版本库名称</param>
        private static void CreateLogger(string repositoryName = "")
        {
            try
            {
                LogManager.CreateRepository(repositoryName);
                if (repositoryName == "Default")
                {
                    CreateLogger(repositoryName, Level.All);
                }
                else
                {
                    CreateLogger(repositoryName, Level.Info);
                    CreateLogger(repositoryName, Level.Error);
                    CreateLogger(repositoryName, Level.Debug);
                    CreateLogger(repositoryName, Level.Fatal);
                    CreateLogger(repositoryName, Level.Warn);
                }
            }
            catch (Exception e)
            {

            }
        }


        /// <summary>
        /// 设置日志格式
        /// </summary>
        /// <param name="repositoryName">Repository（版本库）名称</param>
        /// <param name="level">日志等级</param>
        static void CreateLogger(string repositoryName, Level level)
        {
            ILoggerRepository curRepository = LogManager.GetRepository(repositoryName);

            //设置日志属性
            RollingFileAppender curAppender = new RollingFileAppender()
            {
                //日志的名称:版本库+日志登记命名
                Name = repositoryName + level.ToString(),
                //Name = "InfoFileAppender",
                //日志的保存目录
                // File = string.Format("D:\\log\\EDBLog\\EDB{0}\\", level.ToString()),
                //File = "log\\EDBLog\\EDBError\\",                               
                File = string.Format("log\\{0}\\{1}\\", repositoryName, level),
                //是否静态日志名，静态通过fileAppender.File设置，动态通过日期或者文件大小进行设置
                StaticLogFileName = false,
                //根据日期设置日志文件名
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                //日志文件名的命名规则
                DatePattern = "yyyyMMdd.lo\\g",
                //是否把日志加入到原有日志中。
                AppendToFile = true,
                //每个单独日志文件的最大大小。
                MaxFileSize = 50 * 1024 * 1024,
                //日志可以重写的数量，log，log.1,log.2....
                MaxSizeRollBackups = 1000,
                //选择UTF8编码，确保中文不乱码
                Encoding = Encoding.UTF8,
                LockingModel = new log4net.Appender.FileAppender.MinimalLock()
            };

            //过滤日志等级
            LevelRangeFilter filter = new LevelRangeFilter() { LevelMax = level == Level.All ? Level.Fatal : level, LevelMin = level };
            curAppender.AddFilter(filter);
            filter.ActivateOptions();

            //设置日志记录的内容
            PatternLayout pattenLayout = new PatternLayout() { ConversionPattern = "%date [%thread] %-5level  %logger %message%newline" };
            curAppender.Layout = pattenLayout;
            pattenLayout.ActivateOptions();

            curAppender.ActivateOptions();
            BasicConfigurator.Configure(curRepository, curAppender);
        }
        #endregion
        
    }
}