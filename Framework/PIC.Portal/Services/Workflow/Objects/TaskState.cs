using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    [Serializable]
    public class TaskState : PortalState
    {
        #region 成员

        private WfActorCollection actorList;

        protected TaskRequest request;

        #endregion

        #region 构造函数

        public TaskState()
        {
        }

        public TaskState(string code)
        {
            this.TaskCode = code;
            this.ActorsString = "[]";
            this.TaskStatus = WfTask.StatusEnum.New;

            this.request = new TaskRequest(this.TaskCode);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 流程请求
        /// </summary>
        public TaskRequest Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

        public string TaskCode { get; set; }

        public string Name { get; set; }

        public WfTask.TypeEnum Type { get; set; }

        // 无停滞，为true为立即执行节点
        public bool NoIdle { get; set; }

        /// <summary>
        /// 此属性为true的节点为默认打回节点，所哟偶任务的打回，无特别指定都转到此节点
        /// 理论上只能一个流程中，只能由一个默认打回节点
        /// </summary>
        public bool IsDefaultReject { get; set; }

        public string Status { get; set; }

        /// <summary>
        /// 生成的活动标题
        /// </summary>
        public string ActionTitle { get; set; }

        /// <summary>
        /// 活动操作字符
        /// </summary>
        public string ActionOpString { get; set; }

        /// <summary>
        /// 操作者字符
        /// </summary>
        public string ActorsString { get; set; }

        /// <summary>
        /// 路径表达式
        /// </summary>
        public string RouteExpression { get; set; }

        /// <summary>
        /// 下一步任务路径
        /// </summary>
        [JsonIgnore]
        public string NextRouteCode
        {
            get
            {
                string routeCode = null;

                if (Request != null)
                {
                    routeCode = Request.NextRouteCode;
                }

                return routeCode;
            }
        }

        [JsonIgnore]
        public WfTask.StatusEnum TaskStatus
        {
            get { return CLRHelper.GetEnum<WfTask.StatusEnum>(this.Status, WfTask.StatusEnum.Unknown); }
            internal set { this.Status = value.ToString(); }
        }

        [JsonIgnore]
        public WfActorCollection ActorList
        {
            get { return this.actorList; }
            internal set { this.actorList = value; }
        }

        #endregion
    }
}
