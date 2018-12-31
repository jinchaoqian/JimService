using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jim.AutoMapper
{
    public class Configuration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RegisterUserRequest, SysUser>();
                //cfg.AddProfile<Profiles.OrderProfile>();
                //cfg.AddProfile<Profiles.CalendarEventProfile>();
            });
        }
    }
}