using AutoMapper;
using log4net;
using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Jim
{
    public class UserService : Service
    {
        private ILog log = LoggerHelper.GetAdoNetLogger("Login", "ADOExchange");
        public object Post(LoginRequest request)
        {
            
            using (var client = new OrmClient())
            {
                if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                    return Util.CreateExceptionResponse("U001", "账号或者密码不能为空！");

                string userAccount = request.UserName;
                string userPwd = Util.CreateDbPassword(request.UserName, request.Password);
                var user = client.Select<SysUser>(u => u.UserAccount == userAccount && u.UserPwd == userPwd).FirstOrDefault();

                if (user == null)
                    return Util.CreateExceptionResponse("U002", "账号密码错误！");
                if (user.Status != 1)
                    return Util.CreateExceptionResponse("U003", "用户未激活，不允许登录！");
                else
                {
                    log.Info(Util.InitLogMessage("Login","登录",user.UserName,"","登录成功"));
                    return new LoginResponse()
                    {
                        status = "ok",
                        type = request.Type,
                        currentAuthority = "admin"
                    };
                }

            }
        }

        public object Post(RegisterUserRequest request)
        {
            using (var client = new OrmClient())
            {
                SysUser user = Mapper.Map<SysUser>(request);

                user.UserPwd = Util.CreateDbPassword(request.UserAccount,request.UserAccount);


                var result = client.Insert<SysUser>(user);

                if (result > 0)
                    return Util.Success("注册成功");
                else
                    return Util.Success("注册失败");
            }
        }


        public object Get(GetUsersRequest request)
        {
            using (var client = new OrmClient())
            {
                var q = client.Select<SysUser>();
                q = PageGrid(request, q);
                return Util.SuccessList("查询成功",q);
            }
        }

        public List<Tto> PageGrid<TFrom,Tto>( TFrom page ,IList<Tto> to) 
            where Tto : class 
            where TFrom : BaseListRequest
        {
            //请求的页码
            int pageIndex = page.PageIndex;
            //请求的页大小
            int pageSize = page.PageSize;
            //总行数
            int recordCount = to.Count();
            //总页数
            var pageCount = (int)Math.Ceiling((decimal)1.00 * recordCount / pageSize);
            //如果总页数大于等于请求页码，则用请求页码，否则用当前页码
            pageIndex = pageCount >= pageIndex ? pageIndex : pageCount;
            //进行分页操作
            var q = to.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return q;
        }
    }


    public class BaseListRequest
    {

        [ApiMember(Name = "PageIndex", Description = "当前页", DataType = "int", IsRequired = true)]
        public int PageIndex { get; set; }

        [ApiMember(Name = "PageSize", Description = "分页大小", DataType = "int", IsRequired = true)]
        public int PageSize { get; set; }

        [ApiMember(Name = "Format", Description = "请求格式，默认json", DataType = "string")]
        public string Format { get; set; } = "json";
    }

    public class LoginResponse
    {
        [Alias("status")]
        public string status { get; set; }

        [Alias("type")]
        public string type { get; set; }

        [Alias("currentAuthority")]
        public string currentAuthority { get; set; }
    }

    [Api("login")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/login", "POST", Summary = @"用户登录", Notes = "用户登录")]
    public class LoginRequest:IReturn<LoginResponse>
    {
        [ApiMember(Name = "UserName", Description = "账号", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }

        [ApiMember(Name = "Password", Description = "密码", DataType = "string", IsRequired = true)]
        public string Password { get; set; }


        [ApiMember(Name = "Type", Description = "登录类型account,mobile", DataType = "string", IsRequired = true)]
        public string Type { get; set; }
    }

    [Route("/getUser/{ID}", "GET")]
    public class GetUserRequest
    {
        [ApiMember(Name = "ID", Description = "用户主键ID", DataType = "string", IsRequired = true)]
        public string ID { get; set; }
    }
    [Route("/getUserList", "GET")]
    public class GetUsersRequest: BaseListRequest
    {
    }

    [Route("/user/{ID}", "PUT")]
    public class UpdateUserRequest : DBEntity
    {
        [ApiMember(Name = "ID", Description = "用户主键ID", DataType = "string", IsRequired = true)]
        public string ID { get; set; }
    }

    [Route("/user/{ID}", "DELETE")]
    public class DeleteUserReqeust
    {
        [ApiMember(Name = "ID", Description = "用户主键ID", DataType = "string", IsRequired = true)]
        public string ID { get; set; }
    }

    [Api("register")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/register", "POST", Summary = @"注册用户", Notes = "注册用户")]
    public class RegisterUserRequest:DBEntity
    {
        [ApiMember(Name = "UserName", Description = "用户名", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }
        [ApiMember(Name = "UserAccount", Description = "账号", DataType = "string", IsRequired = true)]
        public string UserAccount { get; set; }

        [ApiMember(Name = "Mobile", Description = "手机号码", DataType = "string")]
        public string Mobile { get; set; }

        [ApiMember(Name = "Phone", Description = "电话", DataType = "string")]
        public string Phone { get; set; }

        [ApiMember(Name = "Email", Description = "邮箱", DataType = "string")]
        public string Email { get; set; }

        [ApiMember(Name = "QQ", Description = "QQ号码", DataType = "string")]
        public string QQ { get; set; }

        [ApiMember(Name = "DingTalkAccount", Description = "钉钉账号", DataType = "string")]
        public string DingTalkAccount { get; set; }

        [ApiMember(Name = "WechatAccount", Description = "微信OpenID", DataType = "string")]
        public string WechatAccount { get; set; }

        [ApiMember(Name = "Status", Description = "启用状态", DataType = "string")]
        public int Status { get; set; }

        [ApiMember(Name = "TitleID", Description = "职称", DataType = "string")]
        public string TitleID { get; set; }

        [ApiMember(Name = "UserPwd", Description = "密码", DataType = "string")]
        public string UserPwd { get; set; }

        [ApiMember(Name = "DeptID", Description = "部门", DataType = "string")]
        public string DeptID { get; set; }

        [ApiMember(Name = "Birthday", Description = "生日", DataType = "string")]
        public string Birthday { get; set; }

        [ApiMember(Name = "ReMark", Description = "备注", DataType = "string")]
        public string ReMark { get; set; }
    }


    public class SysUser : DBEntity
    {

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// QQ账号
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 钉钉账号
        /// </summary>
        public string DingTalkAccount { get; set; }

        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string WechatAccount { get; set; }



        /// <summary>
        /// 启用状态
        /// </summary>
        [ApiAllowableValues("Value", typeof(ItemStatus))]
        public int Status { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string TitleID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ReMark { get; set; }
    }


}