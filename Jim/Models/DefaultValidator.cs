using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.FluentValidation;

namespace Jim
{
    public class DefaultValidator<T> : AbstractValidator<T> where T : DefaultRequest<DefaultResponse>
    {
        public DefaultValidator()
        {
            //RuleFor(r => r.AppKey).NotEmpty();
            //RuleFor(r => r.Sign).NotNull();
            //RuleFor(r => r.TimeStamp).NotNull().NotEmpty();
        }
    }
   

}