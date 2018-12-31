
using Funq;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Jim
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// 注册一个服务类，用于定位其他服务的解析目录
        /// </summary>
        public AppHost()
            : base("Jim", typeof(QuestionService).Assembly) { }

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
                //DebugMode = AppSettings.Get("DebugMode", false),
                //注册网页根目录
                //WebHostPhysicalPath = MapProjectPath("~/my-project/dist"),
                //WebHostPhysicalPath = MapProjectPath("~/wwwroot"),
                //AddRedirectParamsToQueryString = true,
                //UseCamelCase = true,
                //注册接口服务地址
                HandlerFactoryPath = "api"
            });
            //注册测试页
            Plugins.Add(new TemplatePagesFeature());

            //this.Plugins.Add(new CorsFeature());


            this.Plugins.Add(new CorsFeature(allowedMethods: "GET, POST"));

            //注册Swagger插件进行API的Web调试
            Plugins.Add(new SwaggerFeature());


            ////自动扫描验证插件
            //Plugins.Add(new ValidationFeature
            //{
            //    ScanAppHostAssemblies = true
            //});

            //注册验证插件
            //Plugins.Add(new ValidationFeature());
            //container.RegisterValidators(typeof(DefaultValidator).Assembly);

        }
    }
}