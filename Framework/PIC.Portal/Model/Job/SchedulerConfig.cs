using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json;

namespace PIC.Portal.Model
{
    public class SchedulerConfig
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        #endregion

        #region 构造函数

        public SchedulerConfig()
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
        /// 任务标识
        /// </summary>
        public string TaskID
        {
            get;
            set;
        }

        /// <summary>
        /// 任务标识
        /// </summary>
        public string TaskName
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 重复次数
        /// </summary>
        public int? RepeatCount
        {
            get;
            set;
        }

        /// <summary>
        /// 重复间隔(分钟为单位)
        /// </summary>
        public double? RepeatInterval
        {
            get;
            set;
        }

        public string CronString
        {
            get;
            set;
        }

        public string Memo
        {
            get;
            set;
        }

        #endregion
    }
}
