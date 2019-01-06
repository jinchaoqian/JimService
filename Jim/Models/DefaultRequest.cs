using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace Jim
{
    public class DefaultRequest : IReturn<DefaultResponse> 
    {
        [ApiMember(Name = "AppKey", Description = "应用名，测试先默认greatonce", DataType = "string", IsRequired = false)]
        public string AppKey { get; set; }

        [ApiMember(Name = "Sign", Description = "签名，测试先默认12345", DataType = "string", IsRequired = false)]
        public string Sign { get; set; }

        [ApiMember(Name = "TimeStamp", Description = "时间戳，会话超时,有效期5分钟,现在到1970-01-01的时间差,测试期间暂时不启用", DataType = "double", IsRequired = false)]
        public double TimeStamp { get; set; }
    }

    public class DefaultRequest<T> : IReturn<T> 
    {
        [ApiMember(Name = "AppKey", Description = "应用名，测试先默认greatonce", DataType = "string", IsRequired = false)]
        public string AppKey { get; set; }

        [ApiMember(Name = "Sign", Description = "签名，测试先默认12345", DataType = "string", IsRequired = false)]
        public string Sign { get; set; }

        [ApiMember(Name = "TimeStamp", Description = "时间戳，会话超时,有效期5分钟,现在到1970-01-01的时间差,测试期间暂时不启用", DataType = "double", IsRequired = false)]
        public double TimeStamp { get; set; }


        [ApiMember(Name = "Format", Description = "请求格式，默认json", DataType = "string")]
        public string Format { get; set; } = "json";
    }


    public class DefaultListRequest<T> : IReturn<T> 
    {

        [ApiMember(Name = "PageIndex", Description = "当前页", DataType = "int", IsRequired = true)]
        public int PageIndex { get; set; }

        [ApiMember(Name = "PageSize", Description = "分页大小", DataType = "int", IsRequired = true)]
        public int PageSize { get; set; }


        [ApiMember(Name = "Format", Description = "请求格式，默认json", DataType = "string")]
        public string Format { get; set; } = "json";

        //[ApiMember(Name = "Key", Description = "默认主键", DataType = "string")]
        //public string Key { get; set; }
    }
}