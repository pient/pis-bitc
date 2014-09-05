using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIC.Portal.Workflow
{
    [Serializable]
    public class FlowState
    {
        #region 成员

        // 是否第一次进入
        protected bool isFirstEnter = true;

        protected FlowRequest request;

        protected TaskState taskstate;

        #endregion

        #region 构造函数

        public FlowState(FlowRequest request)
        {
            this.request = request;
        }

        #endregion

        #region 属性

        public bool IsFirstEnter
        {
            get { return isFirstEnter; }

            set
            {
                if (isFirstEnter == true)
                {
                    isFirstEnter = value;
                }
                else
                {
                    throw new Exception("Can set value only once");
                }
            }
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState Current
        {
            get { return this.taskstate; }
            set { this.taskstate = value; }
        }

        public string NextRouteCode
        {
            get
            {
                string routeCode = null;

                if (Current != null)
                {
                    routeCode = Current.NextRouteCode;

                    if (String.IsNullOrEmpty(routeCode) && !String.IsNullOrEmpty(Current.RouteExpression))
                    {
                        routeCode = WfHelper.GetWfDataString(Current.RouteExpression, flowState: this);
                    }
                }

                return routeCode;
            }
        }

        /// <summary>
        /// 流程请求
        /// </summary>
        public FlowRequest Request
        {
            get { return this.request; }
        }

        #region Request Info

        /// <summary>
        /// 定义编码
        /// </summary>
        [JsonIgnore]
        public string DefineCode
        {
            get { return request.DefineCode; }
        }

        /// <summary>
        /// 实例id
        /// </summary>
        [JsonIgnore]
        public string InstanceID
        {
            get { return request.InstanceID; }
        }

        /// <summary>
        /// 实体id
        /// </summary>
        [JsonIgnore]
        public string ModelID
        {
            get { return request.ModelID; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonIgnore]
        public string Title
        {
            get { return request.Title; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonIgnore]
        public string ActionTitleFormat
        {
            get { return request.ActionTitleFormat; }
        }

        /// <summary>
        /// 流程表单地址
        /// </summary>
        [JsonIgnore]
        public string FormPath
        {
            get { return request.FormPath; }
        }

        /// <summary>
        /// 表单信息
        /// </summary>
        [JsonIgnore]
        public FlowBasicInfo BasicInfo
        {
            get { return request.BasicInfo; }
            set { request.BasicInfo = value; }
        }

        /// <summary>
        /// 表单信息
        /// </summary>
        [JsonIgnore]
        public FlowFormData FormData
        {
            get { return request.FormData; }
            set { request.FormData = value; }
        }

        /// <summary>
        /// 活动信息
        /// </summary>
        [JsonIgnore]
        public FlowActionInfo ActionInfo
        {
            get { return request.ActionInfo; }
            set { request.ActionInfo = value; }
        }

        #endregion

        #endregion
    }
}
