using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace PIC.Portal.Workflow
{
    public class WfTaskState
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        #endregion

        #region 属性

        public TaskState TaskState { get; set; }

        #endregion

        #region 构造函数

        public WfTaskState()
        {
            Tag = new EasyDictionary();
        }

        public WfTaskState(TaskState tstate)
            : this()
        {
            this.TaskState = tstate;
        }

        [JsonIgnore]
        public virtual object this[string key]
        {
            get
            {
                return Tag[key];
            }
            set
            {
                Tag[key] = value;
            }
        }

        #endregion

        #region 属性成员

        /// <summary>
        /// 触发任务的活动ID
        /// </summary>
        public string TriggerActionID { get; internal set; }
        
        #endregion
    }
}
