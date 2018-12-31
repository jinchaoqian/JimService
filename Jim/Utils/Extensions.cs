using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using ServiceStack.FluentValidation.Results;

namespace Jim
{

    public static class Extensions
    {

        //public static string ToSuccessList(this object obj, string code = "", string message = "")
        //{

        //    //string str = string.Format("{{\"list\":{0}}}", ServiceStack.Text.JsonSerializer.SerializeToString(obj));
        //    // System.Diagnostics.Debug.WriteLine(str);
        //    //return

        //    var response = new DefaultResponse() { list = obj };
        //    if (!string.IsNullOrEmpty(code)) response.Code = code;
        //    if (!string.IsNullOrEmpty(message)) response.Message = message;
        //    //string str1 = JsonConvert.SerializeObject(response);
        //    //System.Diagnostics.Debug.WriteLine(str1);

        //    //string str2 = ServiceStack.Text.JsonSerializer.SerializeToString(response);
        //    //System.Diagnostics.Debug.WriteLine(str2);
        //    return "";
        //}
        //public static DefaultResponse ToSuccessData(this object obj, string code = "", string message = "")
        //{
        //    var response = new DefaultResponse() { Success = true, data = obj };
        //    if (!string.IsNullOrEmpty(code)) response.Code = code;
        //    if (!string.IsNullOrEmpty(message)) response.Message = message;
        //    return response;
        //}

        //public static DefaultResponse ToErrorList(this object obj,string code ="",string message ="")
        //{
        //    var response = new DefaultResponse() { Success = false, list = obj };
        //    if (!string.IsNullOrEmpty(code)) response.Code = code;
        //    if (!string.IsNullOrEmpty(message)) response.Message = message;
        //    return response;
        //}
        //public static DefaultResponse ToErrorData(this object obj, string code = "", string message = "")
        //{
        //    var response = new DefaultResponse() { Success =false,data=obj};
        //    if (!string.IsNullOrEmpty(code)) response.Code = code;
        //    if (!string.IsNullOrEmpty(message)) response.Message = message;
        //    return response;
        //}


        public static ServiceException ToServiceException(this ValidationResult result, string code = "", string message = "")
        {
            StringBuilder sb = new StringBuilder();
            foreach (ValidationFailure item in result.Errors)
            {
                sb.AppendLine(string.Format("{0}:{1};", item.PropertyName, item.ErrorMessage));
            }
            return new ServiceException()
            {
                Message = message,
                Code = code,
                Exception = new Exception(sb.ToString())
            };
        }

        public static ServiceException ToServiceException(this Exception e, string code = "", string message = "")
        {
            return new ServiceException()
            {
                Message = message,
                Code = code,
                Exception = e
            };
        }


        public static ErrorResponse ToResponse(this ServiceException e)
        {
            return new ErrorResponse()
            {
                Message = e.Message,
                Code = e.Code,
                Exception = e.Exception
            };
        }

        public static ErrorResponse ToResponse(this ServiceException e, string code = "", string message = "")
        {
            return new ErrorResponse()
            {
                Message = message,
                Code = code,
                Exception = e.Exception
            };
        }

        public static string GetMethodName(object obj)
        {
            MemberInfo info = typeof(object);
            var item = (MethodAttribute)Attribute.GetCustomAttribute(info, typeof(MethodAttribute));

            return item == null ? string.Empty : item.Name;
        }

        public static MethodAttribute GetAttribute(object obj)
        {
            MemberInfo info = typeof(object);
            return (MethodAttribute) Attribute.GetCustomAttribute(info, typeof (MethodAttribute));
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : Attribute
    {
        public MethodAttribute(string name, string power = "")
        {
            Name = name;
            ViewPower = power;
        }

        /// <summary>
        ///     Gets or sets the view power.
        /// </summary>
        /// <value>The view power.</value>
        public string ViewPower { get; set; }

        /// <summary>
        ///     Gets or sets the name of the page.
        /// </summary>
        /// <value>The name of the page.</value>
        public string Name { get; set; }
    }
}