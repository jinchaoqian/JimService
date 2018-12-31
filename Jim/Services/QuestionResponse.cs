using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim
{
    public class QuestionTypeResponse:DefaultResponse
    {
       //public new  List<QuestionType> List { get; set; }
    }

    public class QuestionTypeList
    {
        public string ID { get; set; }

        public string title { get; set; }
        public string text { get; set; }
        public string date { get; set; }
    }

    public class QuestionType
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public string ManagerUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime ModifyDate { get; set; }

        public string ModifyUser { get; set; }
    }
}
