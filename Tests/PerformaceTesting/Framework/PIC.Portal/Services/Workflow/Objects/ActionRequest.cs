using Newtonsoft.Json;
using PIC.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Workflow
{
    [Serializable]
    public class ActionRequest
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        #endregion

        #region 构造函数

        internal ActionRequest()
        {
            this.BasicInfo = new FlowBasicInfo();

            this.ActionInfo = new FlowActionInfo();

            this.FormData = new FlowFormData();

            this.Tag = new EasyDictionary();
        }

        public ActionRequest(FlowActionInfo actionInfo, FlowBasicInfo basicInfo = null, FlowFormData formData = null)
            : this()
        {
            if (basicInfo != null)
            {
                this.BasicInfo = basicInfo;
            }

            if (formData != null)
            {
                this.FormData = formData;
            }

            if (actionInfo != null)
            {
                this.ActionInfo = actionInfo;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 活动标识
        /// </summary>
        [JsonIgnore]
        public string ActionID
        {
            get { return ActionInfo.ActionID; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonIgnore]
        public string Title
        {
            get { return ActionInfo.Title; }
            set { ActionInfo.Title = value; }
        }

        [JsonIgnore]
        public FlowActionInfo.ActionOperation Operation
        {
            get { return ActionInfo.Operation; }
            set { ActionInfo.Operation = value; }
        }

        /// <summary>
        /// Action操作码
        /// </summary>
        [JsonIgnore]
        public string RouteCode
        {
            get { return ActionInfo.RouteCode; }
            set { ActionInfo.RouteCode = value; }
        }

        /// <summary>
        /// 任务完成率
        /// </summary>
        [JsonIgnore]
        public decimal CompletionRate
        {
            get { return ActionInfo.CompletionRate; }
            set { ActionInfo.CompletionRate = value; }
        }

        /// <summary>
        /// 标识
        /// </summary>
        [JsonIgnore]
        public string Comments
        {
            get { return ActionInfo.Comments; }
            set { ActionInfo.Comments = value; }
        }

        /// <summary>
        /// 基本信息
        /// </summary>
        public FlowBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// Action表单数据
        /// </summary>
        public FlowFormData FormData { get; set; }

        /// <summary>
        /// 活动信息
        /// </summary>
        public FlowActionInfo ActionInfo { get; set; }

        /// <summary>
        /// 活动源
        /// </summary>
        public ActionSourceInfo SourceInfo { get; set; }

        #endregion
    }
}
