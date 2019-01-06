using System;
using System.Collections.Generic;
using ServiceStack;

namespace Jim
{
    public class SuccessResponse<T> : DefaultResponse<T> where T:class 
    {
        //public SuccessResponse(string code, string message, bool success)
        //    : base(code, message, true)
        //{
        //}
        //public SuccessResponse(string code, string message, T obj)
        //    : base(code, message, true)
        //{
        //    this.data = obj;
        //}
    }

    //public class SuccessListResponse<T> : DefaultListResponse<T> where T : class
    //{
    //    public SuccessListResponse()
    //    {
    //        this.Success = true;
    //    }

    //    public SuccessListResponse(string code, string message, List<T> list)
    //        : base(code, message, list)
    //    {
    //        this.Data = list;
    //    }

    //}

}