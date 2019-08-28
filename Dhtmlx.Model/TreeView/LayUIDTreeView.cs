using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.TreeView
{
    public class Status
    {
        /** 状态码*/
        public int code { get; set; }
        /** 信息标识*/
        public string message { get; set; }
    }

    public class DTreeResponse
    {
        public int code { get; set; }
        public string msg { get; set; }
        public Status status { get; set; }
        /** 数据*/
        public List<DTree> data { get; set; }
    }

    public class CheckArr
    {
        /** 复选框标记*/
        public string type { get; set; }
        /** 复选框是否选中*/
        public string @checked { get; set; }
    }

    public class DTree
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 父节点名称
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 是否最后一级节点
        /// </summary>
        public bool last { get; set; }
        /** 自定义图标class*/
        public string iconClass { get; set; }
        /// <summary>
        /// 复选框
        /// </summary>
        public List<CheckArr> checkArr { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<DTree> children { get; set; }
    }
}
