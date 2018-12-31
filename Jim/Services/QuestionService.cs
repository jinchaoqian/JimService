using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;

namespace Jim
{
    public class QuestionService:Service
    {
        public object Any(QuestionTypeRequest request)
        {
            return null;
            //QestionTypeValidator validator = new QestionTypeValidator();
            //ValidationResult result = validator.Validate(request);

            //if (!result.IsValid) return result;


            //using (var db = DBHelper.GetSqlServerConnection())
            //{
            //    var query = db.Query<QuestionTypeList>("select ID,Name title ,'归属:'+typename+' 管理人：'+ManagerUser text,CONVERT(varchar(100), ModifyDate, 23) date  from questiontype ");

            //    return query.ToList().ToSuccessList();

            //}
            
        }
    }
}
