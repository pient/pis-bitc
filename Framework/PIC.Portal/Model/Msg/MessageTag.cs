using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace PIC.Portal.Model
{
    public class MessageTag
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        #endregion

        #region 构造函数

        public MessageTag()
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

        #region 属性

        /// <summary>
        /// 消息相关链接
        /// </summary>
        public virtual string Link
        {
            get;
            set;
        }

        /// <summary>
        /// 弹出次数
        /// </summary>
        public virtual int PopupCount
        {
            get;
            set;
        }

        #endregion
    }
}
