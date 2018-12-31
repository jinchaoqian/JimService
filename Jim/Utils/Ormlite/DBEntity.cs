using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jim
{

    /// <summary>
    /// 所有对象的基础类
    /// </summary>
    public class DBEntity:IDBEntity
    {

        /// <summary>
        /// 主键，guid自动生成
        /// </summary>
        ///
        [Key]
        public string ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [ApiMember(Name = "CreatedDate", Description = "创建时间", DataType = "DateTime")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 创建人员 
        /// </summary>
        [ApiMember(Name = "CreatedBy", Description = "创建人员", DataType = "string", IsRequired = true)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [ApiMember(Name = "ModifyDate", Description = "修改时间", DataType = "DateTime")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 修改人员
        /// </summary>
        [ApiMember(Name = "ModifyBy", Description = "修改人员", DataType = "string", IsRequired = true)]
        public string ModifyBy { get; set; }
    }
}