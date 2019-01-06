using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jim
{
    public class JimBaseService:Service
    {
        public DefaultListResponse<Tto> PageGrid<TFrom, Tto>(DefaultListRequest<TFrom> page, IList<Tto> to)
            where Tto : class
            where TFrom : class
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

            return new DefaultListResponse<Tto>()
            {
                //id =page.Key,
                list = q,
                pagination = new pagination()
                {
                    current = pageIndex,
                    pageSize = pageSize,
                    total = recordCount
                }
            };
        }
    }
}