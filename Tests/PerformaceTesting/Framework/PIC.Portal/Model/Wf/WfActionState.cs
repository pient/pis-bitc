using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace PIC.Portal.Workflow
{
    public class WfActionState
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        /// <summary>
        /// 正对所有者的扩展信息
        /// </summary>
        public EasyDictionary OwnerTag;

        #endregion

        #region 属性

        public ActionRequest Request { get; set; }

        #endregion

        #region 构造函数

        public WfActionState()
        {
            Tag = new EasyDictionary();
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
    }
}
