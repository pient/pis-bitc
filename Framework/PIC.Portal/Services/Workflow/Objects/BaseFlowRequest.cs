using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Activities;
using Newtonsoft.Json;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    [DataContract]
    public class BaseFlowRequest
    {
        #region 成员变量

        /// <summary>
        /// 扩展信息
        /// </summary>
        [DataMember]
        public EasyDictionary Tag;

        private Action<FlowContext> onInstanceBegin;

        private Action<FlowContext> onTaskExecute;

        private Action<FlowContext> onActionExecute;

        private Action<FlowContext> onActionEnd;

        private Action<FlowContext> onTaskEnd;

        private Action<FlowContext> onTaskContinue;

        private Action<FlowContext> onInstanceEnd;

        #endregion

        #region 构造函数

        protected BaseFlowRequest()
            : this(null)
        {
        }

        public BaseFlowRequest(string code)
            : this(new FlowBasicInfo() { DefineCode = code })
        {
            Tag = new EasyDictionary();
        }

        public BaseFlowRequest(FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            this.BasicInfo = basicInfo;
            this.FormData = formData;
            this.ActionInfo = actionInfo;

            if (this.FormData == null)
            {
                this.FormData = new FlowFormData();
            }

            if (this.ActionInfo == null)
            {
                this.ActionInfo = new FlowActionInfo();
            }

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

        #region 事件活动

        public Action<FlowContext> OnInstanceBegin
        {
            get { return this.onInstanceBegin; }
            set { this.onInstanceBegin = value; }
        }

        [JsonIgnore]
        public virtual Action<FlowContext> OnTaskExecute
        {
            get { return this.onTaskExecute; }
            set { this.onTaskExecute = value; }
        }

        [JsonIgnore]
        public virtual Action<FlowContext> OnActionExecute
        {
            get { return this.onActionExecute; }
            set { this.onActionExecute = value; }
        }

        [JsonIgnore]
        public virtual Action<FlowContext> OnActionEnd
        {
            get { return this.onActionEnd; }
            set { this.onActionEnd = value; }
        }

        [JsonIgnore]
        public virtual Action<FlowContext> OnTaskEnd
        {
            get { return this.onTaskEnd; }
            set { this.onTaskEnd = value; }
        }

        [JsonIgnore]
        public virtual Action<FlowContext> OnTaskContinue
        {
            get { return this.onTaskContinue; }
            set { this.onTaskContinue = value; }
        }

        [JsonIgnore]
        public virtual Action<FlowContext> OnInstanceEnd
        {
            get { return this.onInstanceEnd; }
            set { this.onInstanceEnd = value; }
        }

        #endregion

        #region 属性

        [JsonIgnore]
        /// <summary>
        /// 流程定义编号（SysWfDefine）
        /// </summary>
        public virtual string DefineCode
        {
            get
            {
                return BasicInfo.DefineCode;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// 流程实例ID（WfInstance）
        /// </summary>
        public virtual string InstanceID
        {
            get { return BasicInfo.InstanceID; }
        }

        [JsonIgnore]
        /// <summary>
        /// 关联实体数据ID
        /// </summary>
        public virtual string ModelID
        {
            get { return BasicInfo.ModelID; }
        }

        [JsonIgnore]
        /// <summary>
        /// 流程实例ID（SysWfInstance）
        /// </summary>
        public virtual string FlowObjectType
        {
            get { return BasicInfo.FlowObjectType; }
        }

        [JsonIgnore]
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title
        {
            get { return BasicInfo.Title; }
        }

        [JsonIgnore]
        /// <summary>
        /// 活动标题格式 
        /// </summary>
        public virtual string ActionTitleFormat
        {
            get { return BasicInfo.ActionTitleFormat; }
        }

        [JsonIgnore]
        /// <summary>
        /// 流程所挂表单
        /// </summary>
        public virtual string FormPath
        {
            get { return BasicInfo.DefineCode; }
        }

        /// <summary>
        /// 表单数据
        /// </summary>
        [DataMember]
        public virtual FlowFormData FormData { get; set; }

        /// <summary>
        /// 基础信息
        /// </summary>
        [DataMember]
        public virtual FlowBasicInfo BasicInfo { get; set; }

        /// <summary>
        /// 审批信息
        /// </summary>
        [DataMember]
        public virtual FlowActionInfo ActionInfo { get; set; }

        #endregion
    }
}
