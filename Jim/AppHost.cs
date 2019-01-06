
using Funq;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.ApplicationServices;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.FluentValidation;
using ServiceStack.Configuration;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.Logging.Log4Net;
using ServiceStack.IO;
using ServiceStack.VirtualPath;
using System.Configuration;
using ServiceStack.Caching;
using System.Data;
using ServiceStack.Admin;

namespace Jim
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// 注册一个服务类，用于定位其他服务的解析目录
        /// </summary>
        public AppHost()
            : base("Jim", typeof(UserService).Assembly) { }

        //通过依赖注入来注册所有的服务
        protected override ServiceController CreateServiceController(
            params Assembly[] assembliesWithServices)
        {
            return new ServiceController(this, () => assembliesWithServices.ToList().SelectMany(x => x.GetTypes()));
        }
        

        /// <summary>
        /// 个性化配置
        /// 用户初始化服务器需要的IOC资源
        /// </summary>
        public override void Configure(Container container)
        {
            //注册模板页面
            SetConfig(new HostConfig
            {
                //调试模式
                DebugMode = AppSettings.Get("DebugMode", false),
                ////隐藏metadata page页面
                //EnableFeatures = Feature.All.Remove(Feature.Metadata),
                //没有登录可以使用?authsecret=xxx方式，和登录效果一致
                AdminAuthSecret = AppSettings.Get("AdminAuthSecret", "secretz"),
                //注册网页根目录
                //WebHostPhysicalPath = MapProjectPath("~/my-project/dist"),
                //WebHostPhysicalPath = MapProjectPath("~/wwwroot"),
                AddRedirectParamsToQueryString = true,
                //UseCamelCase = true,
                //注册接口服务地址
                HandlerFactoryPath = "api",
                DefaultContentType= MimeTypes.Json,
            });
            //注册测试页
            Plugins.Add(new TemplatePagesFeature());

            Plugins.Add(new AdminFeature());

            Plugins.Add(new AutoQueryFeature { MaxLimit = 100 });

            //Plugins.Add(new BasicAuthFeature());

            //添加一个request filter 确认用户的登录session
            //this.GlobalRequestFilters.Add((req, res, requestDto) =>
            //{
            //    if (!req.GetSession().IsAuthenticated)
            //    {
            //        res.ReturnAuthRequired();
            //    }
            //});

            //container.Register<IAuthProvider>(r => new myAuthProvider());


            //获取连接字符串
            var connectionString = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;
            //注册数据库连接工厂
            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider));
            //注册数据库连接
            container.Register<IAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()){ UseDistinctRoleTables = true });
            //获取用户表的数据库连接,如果没有生成表则自动生成
            container.Resolve<IAuthRepository>().InitSchema();


            //Also store User Sessions in SQL Server
            container.RegisterAs<OrmLiteCacheClient, ICacheClient>();
            container.Resolve<ICacheClient>().InitSchema();


            //获取数据库的配置连接
            //container.Register<IAppSettings>(c => new OrmLiteAppSettings(c.Resolve<IDbConnectionFactory>()));

            //container.Register<IAppSettings>(c => new AppSettings());
            //获取配置
            //var appsetting = (AppSettings)container.Resolve<IAppSettings>();
            //初始化数据表
            //appsetting.InitSchema();

            //appsetting.Get("test", "test");

            var privateKey = RsaUtils.CreatePrivateKeyParams(RsaKeyLengths.Bit2048);
            var publicKeyXml = privateKey.ToPublicKeyXml();


            //所有需要用账号访问的方法，都会调用认证界面CustomUserSession,myAuthProvider为自定义的方法
            //Add Support for 
            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                    new JwtAuthProvider(AppSettings) {
                        AuthKey = AesUtils.CreateKey(),
                        RequireSecureConnection = false,
                        AllowInQueryString = true,
                        SessionExpiry = TimeSpan.FromDays(1),
                        //RedirectUrl = "www.baidu.com",
                        ExpireRefreshTokensIn = TimeSpan.FromDays(1),
                        ExpireTokensIn = TimeSpan.FromDays(100),
                        CreatePayloadFilter = (payload,session) =>
                        {
                            if (session.IsAuthenticated)
                            {
                                payload["CreatedAt"] = session.CreatedAt.ToUnixTime().ToString();
                                payload["exp"] = DateTime.UtcNow.AddYears(1).ToUnixTime().ToString();
                            }
                        },
                        
                        //AuthKeyBase64 = "Base64AuthKey",//AppSettings.GetString("AuthKeyBase64") //"Base64AuthKey",
                        //HashAlgorithm = "RS256",
                        //PrivateKeyXml ="PrivateKey2016Xml", //AppSettings.GetString("PrivateKeyXml")
                    },
                    new ApiKeyAuthProvider(AppSettings)
                    {
                        RequireSecureConnection = false,
                        ExpireKeysAfter = TimeSpan.FromDays(100),
                        SessionCacheDuration = TimeSpan.FromMinutes(10),
                        InitSchema =true,
                        AllowInHttpParams =true,
                        //KeyTypes = new[] { "secret", "publishable" },
                    },        //Sign-in with API Key
                    new CredentialsAuthProvider(),              //Sign-in with UserName/Password credentials
                    new BasicAuthProvider(),                    //Sign-in with HTTP Basic Auth 
                })
            {
                IncludeRegistrationService = true,
                IncludeAssignRoleServices = true,
                IncludeAuthMetadataProvider = true,
                ValidateUniqueEmails = true,
                ValidateUniqueUserNames = true,
                DeleteSessionCookiesOnLogout = true,
                SaveUserNamesInLowerCase = true,
                GenerateNewSessionCookiesOnAuthentication=true,

            });

            //Default route: /register
            //Plugins.Add(new RegistrationFeature() { AllowUpdates = true });
            ////The IAuthRepository is used to store the user credentials etc.
            ////Implement this interface to adjust it to your app's data storage



            //CreateUser(10, "jinchaoqian1", "test1@qq.com", "j4012693", new List<string> { "TheRole" }, new List<string> { "ThePermission" });

            //记录日志
            LogManager.LogFactory = new MyLoggerFactory();
            //启用日志
            Plugins.Add(new RequestLogsFeature()
            {
                EnableRequestBodyTracking = true,
                EnableResponseTracking = true,
                EnableSessionTracking = true,
                EnableErrorTracking = true,
            });
            //启用跨域访问
            this.Plugins.Add(new CorsFeature(allowedMethods: "GET, POST"));
            //注册Swagger插件进行API的Web调试
            Plugins.Add(new SwaggerFeature());
            //启用验证插件
            Plugins.Add(new ValidationFeature());
            //注册验证插件
            container.RegisterValidators(typeof(LoginValidator).Assembly);
            ////自动扫描验证插件
            //Plugins.Add(new ValidationFeature
            //{
            //    ScanAppHostAssemblies = true
            //});


            //JsConfig.EmitLowercaseUnderscoreNames = true;
            //JsConfig.ExcludeDefaultValues = true;
            //序列化时的日期格式
            JsConfig<DateTime>.SerializeFn = dateTime => dateTime.ToString("yyyy-MM-dd");
            JsConfig<TimeSpan>.SerializeFn = time =>
                (time.Ticks < 0 ? "-" : "") + time.ToString("hh':'mm':'ss'.'fffffff");

            //container.Register<ICacheClient>(c => new OrmLiteCacheClient(){DbFactory = c.Resolve<IDbConnectionFactory>()});

            //var cache = container.Resolve<OrmLiteCacheClient>();
            //cache.DbFactory = container.Resolve<IDbConnectionFactory>();

            //container.RegisterAs<OrmLiteCacheClient, ICacheClient>();

            ////Create 'CacheEntry' RDBMS table if it doesn't exist already
            //container.Resolve<ICacheClient>().InitSchema();

            container.Register<ICacheClient>(new MemoryCacheClient());



            //虚拟目录，使用插件方式或者是重载GetVirtualFileSources
            //Plugins.Add(new Disk1Plugin());

            //捕捉到被处理过的错误
            this.ServiceExceptionHandlers.Add((httpReq, request, exception) => {
                //记录错误信息
                //TODO:
                System.Diagnostics.Debug.WriteLine(exception.Message);
                return null; //返回默认错误

                //或者自定义错误
                //return DtoUtils.CreateErrorResponse(request, exception);
            });

            //处理未被系统处理的错误
            //E.g. Exceptions during Request binding or in filters:
            this.UncaughtExceptionHandlers.Add((req, res, operationName, ex) =>
            {
                res.WriteAsync($"Error: {ex.GetType().Name}: {ex.Message}");
                res.EndRequest(skipHeaders: true);


            });

            AfterInitCallbacks.Add(host =>
            {
                var authProvider = (ApiKeyAuthProvider)
                    AuthenticateService.GetAuthProvider(ApiKeyAuthProvider.Name);
                using (var db = host.TryResolve<IDbConnectionFactory>().Open())
                {
                    var userWithKeysIds = db.Column<string>(db.From<ApiKey>()
                        .SelectDistinct(x => x.UserAuthId)).Map(int.Parse);

                    var userIdsMissingKeys = db.Column<string>(db.From<UserAuth>()
                        .Where(x => userWithKeysIds.Count == 0 || !userWithKeysIds.Contains(x.Id))
                        .Select(x => x.Id));

                    var authRepo = (IManageApiKeys)host.TryResolve<IAuthRepository>();
                    foreach (var userId in userIdsMissingKeys)
                    {
                        var apiKeys = authProvider.GenerateNewApiKeys(userId.ToString());
                        authRepo.StoreAll(apiKeys);
                    }
                }
            });
        }

        public override IDbConnection GetDbConnection(IRequest req = null)
        {
            //If an API Test Key was used return DB connection to TestDb instead: 
            return req.GetApiKey()?.Environment == "test"
                ? TryResolve<IDbConnectionFactory>().OpenDbConnection("TestContext")
                : base.GetDbConnection(req);
        }

        private void CreateUser(int id, string username, string email, string password, List<string> roles = null, List<string> permissions = null)
        {
            new SaltedHash().GetHashAndSaltString(password, out var hash, out var salt);

            var user = new UserAuth
            {
                Id = id,
                DisplayName = "DisplayName",
                Email = email ?? "as@if{0}.com".Fmt(id),
                UserName = username,
                FirstName = "FirstName",
                LastName = "LastName",
                PasswordHash = hash,
                Salt = salt,
                Roles = roles,
                Permissions = permissions
            };
            var userRep = Container.Resolve<IUserAuthRepository>();
            userRep.CreateUserAuth(user, password);
        }



        public override List<IVirtualPathProvider> GetVirtualFileSources()
        {
            var existingProviders = base.GetVirtualFileSources();
            existingProviders.Add(new FileSystemMapping("img", MapProjectPath("~/Images")));
            existingProviders.Add(new FileSystemMapping("docs", MapProjectPath("~/Documents")));
            return existingProviders;
        }

        public override void OnExceptionTypeFilter(
            Exception ex, ResponseStatus responseStatus)
        {
            var argEx = ex as ArgumentException;
            var isValidationSummaryEx = argEx is ValidationException;
            if (argEx != null && !isValidationSummaryEx && argEx.ParamName != null)
            {
                var paramMsgIndex = argEx.Message.LastIndexOf("Parameter name:");
                var errorMsg = paramMsgIndex > 0
                    ? argEx.Message.Substring(0, paramMsgIndex)
                    : argEx.Message;

                responseStatus.Errors.Add(new ResponseError
                {
                    ErrorCode = ex.GetType().Name,
                    FieldName = argEx.ParamName,
                    Message = errorMsg,
                });
            }
        }

        public class myAuthProvider : BasicAuthProvider
        {
            public myAuthProvider() : base() { }

            public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
            {
                //Add here your custom auth logic (database calls etc)
                //Return true if credentials are valid, otherwise false


                string sqlServerConnStr = ConfigurationManager.ConnectionStrings["DBContext"].ToString();
                IOrmLiteDialectProvider _dialectProvider = SqlServerDialect.Provider;
                OrmLiteConnectionFactory _dbFactory = new OrmLiteConnectionFactory(sqlServerConnStr, _dialectProvider);
                
                using (var client = _dbFactory.Open())
                {
                    new SaltedHash().GetHashAndSaltString(password, out var hash, out var salt);
                    var user = client.Select<UserAuth>(u => u.UserName ==userName && u.PasswordHash == hash).FirstOrDefault();
                    if(user!=null)
                        return true;
                    else
                        return false;
                }
            }

            public override IHttpResult OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
            {
                //Fill the IAuthSession with data which you want to retrieve in the app 
                //  the base AuthUserSession properties e.g
                session.FirstName = "It's me";
                //session.IsAuthenticated = true;
              
                //...   
                //  derived CustomUserSession properties e.g
                if (session is CustomUserSession)
                    ((CustomUserSession)session).MyData = "It's me";
                //...
                //Important: You need to save the session!
                authService.SaveSession(session, SessionExpiry);
                return null;
            }
        }

        public class CustomUserSession : AuthUserSession
        {

            public string MyData { get; set; }
            
        }
    }
    public class BasicAuthFeature : IPlugin
    {
        public string HtmlRedirect { get; set; }   //User-defined configuration

        public void Register(IAppHost appHost)
        {
            //Register Services exposed by this module
            appHost.RegisterService<AuthenticationService>("/auth", "/auth/{provider}");
            appHost.RegisterService<AssignRolesService>("/assignroles");
            appHost.RegisterService<UnAssignRolesService>("/unassignroles");
            appHost.RegisterService<RegisterService>("/register");

            //Load dependent plugins
            appHost.LoadPlugin(new SessionFeature());
        }
    }


}