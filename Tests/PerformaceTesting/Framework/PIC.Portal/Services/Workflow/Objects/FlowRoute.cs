using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程路径对象
    /// </summary>
    [Serializable]
    public class FlowRoute
    {
        #region 构造函数

        public FlowRoute()
        {
            ToRoles = new List<string>();
            ToUsers = new List<string>();
            ToGroups = new List<string>();
        }

        #endregion

        #region 属性

        [Browsable(true)]
        public string Code { get; set; }

        [Browsable(true)]
        public string Name { get; set; }

        public IList<string> ToRoles { get; internal set; }
        public IList<string> ToUsers { get; internal set; }
        public IList<string> ToGroups { get; internal set; }

        #endregion
    }
}
