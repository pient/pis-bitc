using Newtonsoft.Json;
using PIC.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程请求参数
    /// </summary>
    [Serializable]
    public class TaskRequest
    {
        #region Consts & Enums

        public const string ROUTE_CODE_STARTED = "Started";
        public const string ROUTE_CODE_INITIALIZED = "Initialized";

        #endregion

        #region 成员变量

        [NonSerialized]
        public WfActorCollection ActorList;

        /// <summary>
        /// 扩展信息
        /// </summary>
        public EasyDictionary Tag;

        /// <summary>
        /// 引起TaskRequest提交的ActionRequest
        /// </summary>
        private ActionRequest triggerRequest;

        #endregion

        #region 构造函数

        public TaskRequest()
            : this(ROUTE_CODE_INITIALIZED)
        {

        }

        public TaskRequest(ActionRequest actionRequest)
            : this(actionRequest.RouteCode)
        {
            this.triggerRequest = actionRequest;

            this.NextRouteCode = actionRequest.RouteCode;
        }

        public TaskRequest(string routeCode)
        {
            this.NextRouteCode = routeCode;
            ActorList = new WfActorCollection();
            Tag = new EasyDictionary();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 路径编号
        /// </summary>
        public string NextRouteCode { get; set; }

        /// <summary>
        /// 源任务编号
        /// </summary>
        public string SourceTaskCode { get; set; }

        /// <summary>
        /// 处理人列表，这里用于记录信息
        /// </summary>
        public EasyCollection<MinUserInfo> UserActorList { get; set; }

        /// <summary>
        /// 引起TaskRequest提交的ActionRequest
        /// </summary>
        [JsonIgnore]
        internal ActionRequest TriggerRequest
        {
            get { return triggerRequest; }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 重新设置用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        public void ResetUserList(IEnumerable<IUserInfo> userList)
        {
            var _usrList = userList.Select(u => u.GetMinUserInfo());

            this.UserActorList = new EasyCollection<MinUserInfo>(_usrList);
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="userInfo"></param>
        public void AddUser(params IUserInfo[] userInfo)
        {
            if (this.UserActorList == null)
            {
                this.UserActorList = new EasyCollection<MinUserInfo>();
            }

            foreach (var usr in userInfo)
            {
                this.UserActorList.Add(usr.GetMinUserInfo());
            }
        }

        #endregion
    }
}
