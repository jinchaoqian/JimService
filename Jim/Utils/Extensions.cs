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