using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程信息（一般用于将流程信息传给Bus页面）
    /// </summary>
    [Serializable]
    public class FlowInfo
    {
        #region 构造函数 

        public FlowInfo()
        {
        }

        #endregion

        #region 成员属性

        /// <summary>
        /// 流程状态
        /// </summary>
        public FlowState FlowState
        {
            get;
            set;
        }

        #endregion

    }
}
