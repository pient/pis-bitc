using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PIC.Portal.Workflow
{
    [Serializable]
    public class WfInstanceState
    {
        #region 成员变量

        public EasyDictionary Tag;

        #endregion

        #region 构造函数

        public WfInstanceState()
        {
            this.WfFlowInfo = new FlowInfo();
        }

        public WfInstanceState(string wfInstanceID)
            : this()
        {
            this.WfInstanceID = wfInstanceID;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 流程实例
        /// </summary>
        public string WfInstanceID
        {
            get;
            set;
        }

        /// <summary>
        /// 流程对象类型
        /// </summary>
        public string WfFlowObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 流程对象类型
        /// </summary>
        public FlowInfo WfFlowInfo
        {
            get;
            set;
        }

        #endregion
    }
}
