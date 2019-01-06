using ServiceStack;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Jim
{
    public class CustomRequestFilterAttribute : RequestFilterAttribute
    {

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            ////This code is executed before the service
            //string userAgent = req.UserAgent;
            //StatisticManager.SaveUserAgent(userAgent);
        }
    }

    //Async:
    public class CustomAsyncRequestFilterAttribute : RequestFilterAsyncAttribute
    {
        public override Task ExecuteAsync(IRequest req, IResponse res, object requestDto)
        {
            throw new NotImplementedException();
        }
    }


    public interface IRequestFilterBase
    {
        int Priority { get; }      // Prioity of <0 are tun before Global Request Filters. >=0 Run after
        IRequestFilterBase Copy(); // A new shallow copy of this filter is used on every request.
    }

    public interface IHasRequestFilter : IRequestFilterBase
    {
        void RequestFilter(IRequest req, IResponse res, object requestDto);
    }

    public interface IHasRequestFilterAsync : IRequestFilterBase
    {
        Task RequestFilterAsync(IRequest req, IResponse res, object requestDto);
    }

    public class CustomResponseFilterAttribute : ResponseFilterAttribute
    {
        public override void Execute(IRequest req, IResponse res, object responseDto)
        {
            //throw new NotImplementedException();
        }
    }

    //Async:
    public class CustomAsyncResponseFilterAttribute : ResponseFilterAsyncAttribute
    {
        public override Task ExecuteAsync(IRequest req, IResponse res, object responseDto)
        {
            throw new NotImplementedException();
        }
    }

}