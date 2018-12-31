using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.FluentValidation.Results;

namespace Jim
{
    public class ErrorResponse : DefaultResponse
    {
        public ErrorResponse()
        {
            this.Success = false;
        }

        public ErrorResponse(string code,string message,Exception e =null)
            :base(code,message,false)
        {
            this.Exception = e;
        }

        [ApiMember(Name = "Exception", Description = "错误信息", DataType = "string", IsRequired = false)]
        public Exception Exception { get; set; }
    }

    public class ServiceException : Exception
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <value>返回结果</value>
        //[ApiMember(Name = "Success", Description = "是否请求成功", DataType = "bool", IsRequired = true)]
        public bool Success { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <value>返回消息</value>
        //[ApiMember(Name = "Code", Description = "接口返回代码", DataType = "string", IsRequired = false)]
        public string Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <value>返回消息</value>
        //[ApiMember(Name = "Message", Description = "接口返回信息", DataType = "string", IsRequired = false)]
        public new  string Message { get; set; }

        
        //[ApiMember(Name = "Exception", Description = "错误信息", DataType = "string", IsRequired = false)]
        public Exception Exception { get; set; }

        public ServiceException()
        {
            
        }

        public ServiceException(string message, string code = "")
        {
            Code = code;
            Message = message;
        }

        public ServiceException(string message, string code = "", Exception e = null)
        {
            Exception = e;
            Code = code;
            Message = message;
        }
    }

}
