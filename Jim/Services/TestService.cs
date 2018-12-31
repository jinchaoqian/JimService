using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ServiceStack;

namespace Jim
{
    public class TestService:Service
    {
        public object Any(Hello hello)
        {
            return new HelloResponse {
                value = "您好，亲爱的"+hello.Name +"先生",
                img = "http://pos.eifini.com:30006/image/index?goodsno=5180214224021",
                price ="1980"
            };
        }

        public object Any(GetGrid test)
        {
            int pageSize = test.rows;

            var q = getdata();

            int recordCount = q.Count();

            var pageCount = (int)Math.Ceiling((decimal)1.00* recordCount / pageSize);


            int currentCount= pageCount >= test.page?test.page:pageCount;



            return new GetGridResponse()
            {
                list = getdata().Skip((currentCount-1) * pageSize).Take(pageSize).ToList(),
                pagination = new pagination()
                {
                    total= recordCount,
                    pageSize= pageSize,
                    current= currentCount
                }
            };
        }


        public List<GridRow> getdata()
        {
            List<GridRow> rows = new List<GridRow>();
            for (int i = 0; i < 100; i++)
            {
                rows.Add(new GridRow() { id = i, name = string.Format("test{0}",i), age = i*2-20, status = i-5 });
            }
            return rows;
        }

        public object Any(getUserName hello)
        {
            return new HelloResponse1 {
                user_name="金朝钱",
                code=200,
                url= "http://pos.eifini.com:30006/image/index?goodsno=5180214224021"
            };
        }
    }

    [Api("测试")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/hello", "POST", Summary = @"查询问题类型", Notes = "测试")]
    [Route("/hello", "GET", Summary = @"查询问题类型", Notes = "测试")]
    public class Hello
    {
        public string Name { get; set; }
    }



    [Api("getUserName")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/getUserName", "POST", Summary = @"getUserName", Notes = "getUserName")]
    [Route("/getUserName", "GET", Summary = @"getUserName", Notes = "getUserName")]
    public class getUserName
    {
        public string aa { get; set; }
    }

    public class HelloResponse1
    {
        public string user_name { get; set; }
        public int code { get; set; }
        public string url { get; set; }

    }
    public class HelloResponse
    {
        public string value { get; set; }

        public string img { get; set; }

        public string price { get; set; }

        public string name { get; set; }
    }


   

    [Api("test222")]
    [ApiResponse(HttpStatusCode.BadRequest, "您的请求被搁置")]
    [ApiResponse(HttpStatusCode.InternalServerError, "服务端错误")]
    [Route("/test222", "POST", Summary = @"test222", Notes = "test222")]
    [Route("/test222", "GET", Summary = @"test222", Notes = "test222")]
    public class GetGrid
    {
        public int aa { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
    }


    public class GetGridResponse
    {
       public  List<GridRow> list { get; set; }

        public  pagination pagination { get; set; }
    }

    public class pagination
    {
        public int total { get; set; }
        public int pageSize { get; set; }
        public int current { get; set; }
    }

    public class GridRow
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int status { get; set; }
    }

    public class GridColumn
    {
        public string title { get; set; }
        public string dataIndex { get; set; }
        public string key { get; set; }


    }
}