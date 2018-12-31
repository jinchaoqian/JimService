using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Jim
{
    public enum ItemStatus
    {
        Normal =0,//未审核
        Checked = 1//已审核
    }

    public enum BillStatus
    {
        Normal =0,//未审核
        Checked =1,//已审核
        UnChecked =5,//反审核
        Canceled = 8//已作废
    }
}