using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Jim
{
    [Api("查询问题类型")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/GetQuestionType", "POST", Summary = @"查询问题类型", Notes = "查询问题类型")]
    [Route("/GetQuestionType", "GET", Summary = @"查询问题类型", Notes = "查询问题类型")]
    public class QuestionTypeRequest: DefaultRequest<DefaultResponse>
    {
       
    }

    public class QestionTypeValidator : DefaultValidator<QuestionTypeRequest>
    {
        public QestionTypeValidator() : base(){}
    }

    [Api("查询问题类型")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/GetQuestionTypeDetail", "POST", Summary = @"查询问题类型", Notes = "查询问题类型")]
    [Route("/GetQuestionTypeDetail", "GET", Summary = @"查询问题类型", Notes = "查询问题类型")]
    public class QuestionTypeDetailRequest : DefaultRequest<DefaultResponse>
    {
        public string ID { get; set; }
    }

    public class QestionTypeDetailValidator : DefaultValidator<QuestionTypeDetailRequest>
    {
        public QestionTypeDetailValidator() : base() {

            RuleFor(r => r.AppKey).NotEmpty();
        }
    }






}
