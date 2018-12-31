using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace Jim
{
    public class DefaultRequest<T> : IReturn<T> where T : DefaultResponse
    {
        [ApiMember(Name = "AppKey", Description = "应用名，测试先默认greatonce", DataType = "string", IsRequired = false)]
        public string AppKey { get; set; }

        [ApiMember(Name = "Sign", Description = "签名，测试先默认12345", DataType = "string", IsRequired = false)]
        public string Sign { get; set; }

        [ApiMember(Name = "TimeStamp", Description = "时间戳，会话超时,有效期5分钟,现在到1970-01-01的时间差,测试期间暂时不启用", DataType = "double", IsRequired = false)]
        public double TimeStamp { get; set; }
    }
}