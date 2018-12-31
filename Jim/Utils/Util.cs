using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Web;

namespace Jim
{
    public class Util
    {

        public static ErrorResponse CreateExceptionResponse(string code,string message,Exception exception = null)
        {
            return new ErrorResponse(code,message, exception);
        }

        public static DefaultResponse Success(string message,string code="")
        {
            return  new DefaultResponse(code,message,true);
        }

        public static SuccessResponse<T> SuccessObject<T>(string message, T obj = null, string code="") where T:class
        {
            return new SuccessResponse<T>(code, message, obj);
        }
        public static SuccessListResponse<T> SuccessList<T>(string message, List<T> obj = null, string code = "") where T : class
        {
            return new SuccessListResponse<T>(code, message,  obj);
        }


        public static LogMessage InitLogMessage(string modName, string actionName, string objectName, string status, string message)
        {
            LogMessage newMessage = new LogMessage
            {
                InterFaceName = string.Format("{0}-{1}", modName, actionName),
                ObjectName = objectName,
                Message = message,
                UserName = System.Web.HttpContext.Current.User.Identity.Name,
                IP = GetWebClientIp(),//request.RemoteIp,
                Mac = System.Web.HttpContext.Current.Request.UserHostName,
                RequestUrl = System.Web.HttpContext.Current.Request.RawUrl,
                Status = status,
            };
            return newMessage;
        }


        public static LogMessage InitLogMessage(string modName, string actionName, string message, object obj)
        {
            LogMessage newMessage = new LogMessage
            {
                InterFaceName = string.Format("{0}-{1}", modName, actionName),
                Data = obj.ToJson(),
                Message = message,
                UserName = System.Web.HttpContext.Current.User.Identity.Name,
                IP = GetWebClientIp(),//request.RemoteIp,
                Mac = System.Web.HttpContext.Current.Request.UserHostName,
                RequestUrl = System.Web.HttpContext.Current.Request.RawUrl,
            };
            return newMessage;
        }

        public static LogMessage InitLogMessage(string guid, IRequest request, string objectName,
            string data, string status, string response, string message = "", string interfaceName = "", Exception e = null)
        {
            LogMessage newMessage = new LogMessage
            {
                MesID = guid,
                IP = GetWebClientIp(),//request.RemoteIp,
                Mac = request.UserHostAddress,
                InterFaceNo = "",
                RequestSystem = "测试",
                InterFaceName = interfaceName,
                ObjectName = objectName,
                RequestUrl = request.AbsoluteUri,
                Data = data,
                Status = status,
                Response = response
            };

            //获取上一步的方法名称
            var method1 = new StackTrace().GetFrame(1).GetMethod();
            //获取方法的自定义属性
            var item = method1.GetCustomAttribute(typeof(MethodAttribute)) as MethodAttribute;

            newMessage.InterFaceName = (item != null && string.IsNullOrEmpty(interfaceName)) ? item.Name : interfaceName;

            return newMessage;
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
            /// 访问权限，空值表示无须权限检测
            /// </summary>
            /// <value>The view power.</value>
            public string ViewPower { get; set; }

            /// <summary>
            /// 方法名称
            /// </summary>
            /// <value>The name of the page.</value>
            public string Name { get; set; }
        }

        public static string GetWebClientIp()
        {
            string userIP = "未获取用户IP";

            try
            {
                if (System.Web.HttpContext.Current == null
            || System.Web.HttpContext.Current.Request == null
            || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return "";

                string CustomerIP = "";

                //CDN加速后取到的IP 
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


                if (!String.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                }

                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch { }

            return userIP;
        }

        /// <summary>
        /// 签名方法
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="secret">秘钥</param>
        /// <param name="qhs">是否需要添加秘钥</param>
        /// <returns>加密字符串</returns>
        /// <remarks>聚石塔WebService</remarks>
        public static string SignTopRequest(IDictionary<string, string> parameters, string secret, bool qhs)
        {

            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            if (qhs)
            {
                query.Append(secret);
            }
            System.Diagnostics.Debug.WriteLine(query);


            // 第三步：使用MD5加密
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }
            System.Diagnostics.Debug.WriteLine(result.ToString().Replace("-", "").ToUpper());

            return result.ToString();

        }
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        /// <remarks>聚石塔WebService</remarks>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        /// <remarks>聚石塔WebService</remarks>
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }


        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <remarks>聚石塔WebService</remarks>
        public static string GetConnectionString()
        {
            IAppSettings appSettings = new AppSettings();
            return Decrypt(appSettings.Get<string>("Server"), "jim12345");
        }

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        /// <remarks>聚石塔WebService</remarks>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        /// <remarks>聚石塔WebService</remarks>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }


        /// <summary>
        /// 对比用户明文密码是否和加密后密码一致
        /// </summary>
        /// <param name="dbPassword">数据库中单向加密后的密码</param>
        /// <param name="userPassword">用户明文密码</param>
        /// <returns></returns>
        public static bool ComparePasswords(string dbPassword, string userName, string userPassword)
        {
            string str = String.Format("{0}@#@{1}", userName, userPassword);
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString().Replace("-", "").ToUpper() == dbPassword;

        }

        /// <summary>
        /// 创建用户的数据库密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
		public static string CreateDbPassword(string userName, string userPassword)
        {

            string str = String.Format("{0}@#@{1}", userName, userPassword);
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString().Replace("-", "").ToUpper();

        }

        #region 私有函数
        /// <summary>
        /// 将一个字符串哈希化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] HashString(string str)
        {
            byte[] pwd = System.Text.Encoding.UTF8.GetBytes(str);

            SHA1 sha1 = SHA1.Create();
            byte[] saltedPassword = sha1.ComputeHash(pwd);
            return saltedPassword;
        }
        private static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
        // create a salted password given the salt value
        private static byte[] CreateSaltedPassword(byte[] saltValue, byte[] unsaltedPassword)
        {
            // add the salt to the hash
            byte[] rawSalted = new byte[unsaltedPassword.Length + saltValue.Length];
            unsaltedPassword.CopyTo(rawSalted, 0);
            saltValue.CopyTo(rawSalted, unsaltedPassword.Length);

            //Create the salted hash			
            SHA1 sha1 = SHA1.Create();
            byte[] saltedPassword = sha1.ComputeHash(rawSalted);

            // add the salt value to the salted hash
            byte[] dbPassword = new byte[saltedPassword.Length + saltValue.Length];
            saltedPassword.CopyTo(dbPassword, 0);
            saltValue.CopyTo(dbPassword, saltedPassword.Length);

            return dbPassword;
        }
        #endregion
    }
}