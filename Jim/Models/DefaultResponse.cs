using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer.Collaboration;
using System.Web;
using ServiceStack;

namespace Jim
{
    public class DefaultResponse
    {
        public DefaultResponse(){}

        public DefaultResponse(string code,string message,bool success)
        {
            this.Code = code;
            this.Message = message;
            this.Success = success;
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <value>返回结果</value>
        [ApiMember(Name = "Success", Description = "是否请求成功", DataType = "bool", IsRequired = true)]
        public bool Success { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <value>返回消息</value>
        [ApiMember(Name = "Message", Description = "接口返回信息", DataType = "string", IsRequired = false)]
        public string Message { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <value>返回消息</value>
        [ApiMember(Name = "Code", Description = "接口返回代码", DataType = "string", IsRequired = false)]
        public string Code { get; set; }
        
    }

    public class DefaultResponse<T>:DefaultResponse where T:class 
    {
        public DefaultResponse()
        {
        }

        public DefaultResponse(string code, string message,object obj)
        :base(code,message,true)
        {
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        /// <value>返回消息</value>
        [ApiMember(Name = "Data", Description = "返回对象", DataType = "object", IsRequired = false)]
        public T Data { get; set; }

    }

    public class DefaultListResponse<T> : DefaultResponse where T : class
    {
        public DefaultListResponse()
        {
        }

        public DefaultListResponse(string code, string message, List<T> obj)
            : base(code, message, true)
        {
            this.Data = obj;
        }

        /// <summary>
        /// 返回对象集合
        /// </summary>
        /// <value>返回消息</value>
        [ApiMember(Name = "List", Description = "返回对象集合", DataType = "object", IsRequired = false)]
        public List<T> Data { get; set; }

    }
}