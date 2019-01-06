using ServiceStack;
using ServiceStack.VirtualPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jim.Models
{
    public class Disk1Plugin : IPlugin, IPreInitPlugin
    {
        public void Configure(IAppHost appHost)
        {
            appHost.AddVirtualFileSources.Add(new FileSystemMapping("disk1", appHost.MapProjectPath("~/App_Data/mount/hdd")));
            appHost.AddVirtualFileSources.Add(new FileSystemMapping("disk2", "d:\\hdd"));
        }

        public void Register(IAppHost appHost) { }
    }
}