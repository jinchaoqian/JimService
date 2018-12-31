using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim
{

     public class LogMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        public LogMessage()
        {
            //IP = Util.getLocalIP();
            //Mac = Util.getLocalMac();
            //Url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        }
        /// <summary>
        /// 客户单IP地址
        /// </summary>
        /// <value>The ip.</value>
        public string IP { get; set; }
        /// <summary>
        /// 客户端MAC地址
        /// </summary>
        /// <value>The mac.</value>
        public string Mac { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        /// <value>The name of the mod.</value>
        public string ModName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// 单据名或对象名
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// guid
        /// </summary>
        public string MesID { get; set; }

        /// <summary>
        /// 接口编号
        /// </summary>
        /// <value>The name of the mod.</value>
        public string InterFaceNo { get; set; }

        /// <summary>
        /// 请求系统名称
        /// </summary>
        /// <value>The name.</value>
        public string RequestSystem { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        /// <value>The name of the mod.</value>
        public string InterFaceName { get; set; }

        /// <summary>
        /// 对象名
        /// </summary>
        /// <value>The name of the action.</value>
        public string ObjectName { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        /// <value>The message.</value>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <value>The name of the user.</value>
        public string Data { get; set; }

        /// <summary>
        /// 请求状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public string Response { get; set; }
    }

}
