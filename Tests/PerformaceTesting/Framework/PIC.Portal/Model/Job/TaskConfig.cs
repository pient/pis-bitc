using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIC.Portal.Model
{
    public class SysTaskConfig
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 方法参数
        /// </summary>
        public EasyDictionary Args;

        #endregion

        #region 属性

        #endregion

        #region 构造函数

        public SysTaskConfig()
        {
            Type t = this.GetType();
            Tag = new EasyDictionary();
            Args = new EasyDictionary();
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
