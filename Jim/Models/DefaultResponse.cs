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

        //public DefaultResponse(string code,string message,bool success)
        //{
        //    this.Success = success;
        //    this.Message = message;
        //    this.Code = code;
        //}

        public bool Success { get; set; }
        public string Code { get; set; }

        public string Message { get; set; }
    }

    public class DefaultResponse<T>:DefaultResponse
    {

        //public DefaultResponse(string code, string message, bool success)
        //:base(code,message,success)
        //{
        //}
        /// <summary>
        /// 返回对象
        /// </summary>
        /// <value>返回消息</value>
        [ApiMember(Name = "data", Description = "返回对象", DataType = "object", IsRequired = false)]
        public T data { get; set; }

    }

    public class DefaultListResponse<T> : DefaultResponse 
    {
        //public DefaultListResponse(string code, string message, bool success)
        //    : base(code, message, success)
        //{
        //}
        public string id {get;set;}

        public List<T> list { get; set; }

        public pagination pagination { get; set; }
    }

    public class pagination
    {
        public int total { get; set; }
        public int pageSize { get; set; }
        public int current { get; set; }
    }
    
}