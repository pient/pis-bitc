using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Data;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程帮助类
    /// </summary>
    public class WfActorHelper
    {
        #region 获取成员列表

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="actors"></param>
        /// <param name="flowState"></param>
        /// <returns></returns>
        public IList<OrgUser> GetUserList(WfActorCollection actors, FlowContext ctx = null)
        {
            IList<OrgUser> userList = new List<OrgUser>();

            foreach (var actor in actors)
            {
                var usrs = GetUserList(actor, ctx);

                foreach (var usr in usrs)
                {
                    userList.Add(usr);
                }
            }

            userList = userList.Distinct(new EntityComparer<OrgUser>()).ToList();

            return userList;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="flowState"></param>
        /// <returns></returns>
        public IList<OrgUser> GetUserList(WfActor actor, FlowContext ctx = null)
        {
            IList<OrgUser> userList = new List<OrgUser>();
            OrgRole role = null;
            OrgGroup group = null;

            var groupCode = !String.IsNullOrEmpty(actor.GroupCode) ? actor.GroupCode : actor.Tag.Get<string>("GroupCode");
            var groupId = !String.IsNullOrEmpty(actor.GroupId) ? actor.GroupId : actor.Tag.Get<string>("GroupId");
            var roleCode = !String.IsNullOrEmpty(actor.RoleCode) ? actor.RoleCode : actor.Tag.Get<string>("RoleCode");
            var roleId = !String.IsNullOrEmpty(actor.RoleId) ? actor.RoleId : actor.Tag.Get<string>("RoleId");
            var userCode = !String.IsNullOrEmpty(actor.UserCode) ? actor.UserCode : actor.Tag.Get<string>("WorkNos", actor.Tag.Get<string>("UserCode"));
            var userIds = !String.IsNullOrEmpty(actor.UserIds) ? actor.UserIds : actor.Tag.Get<string>("Ids", actor.Tag.Get<string>("UserIds"));

            var actorType = actor.ActorType;

            if (actorType == WfActor.TypeEnum.Auto)
            {
                if (!String.IsNullOrEmpty(actor.UserCode) || !String.IsNullOrEmpty(actor.UserIds))
                {
                    actorType = WfActor.TypeEnum.User;
                }
                else if (!String.IsNullOrEmpty(actor.RoleCode) || !String.IsNullOrEmpty(actor.RoleId))
                {
                    actorType = WfActor.TypeEnum.Role;

                    if (!String.IsNullOrEmpty(actor.RoleId))
                    {
                        role = OrgRole.Find(actor.RoleId);
                    }
                    else if (!String.IsNullOrEmpty(actor.RoleCode))
                    {
                        role = OrgRole.Get(actor.RoleCode);
                    }
                }
                else if (!String.IsNullOrEmpty(actor.GroupCode) || !String.IsNullOrEmpty(actor.GroupId))
                {
                    actorType = WfActor.TypeEnum.Group;

                    if (!String.IsNullOrEmpty(actor.GroupId))
                    {
                        group = OrgGroup.Find(actor.GroupId);
                    }
                    else if (!String.IsNullOrEmpty(actor.GroupCode))
                    {
                        group = OrgGroup.Get(actor.GroupCode);
                    }
                }
                else if (!String.IsNullOrEmpty(actor.FuncCode))
                {
                    actorType = WfActor.TypeEnum.Func;
                }
            }

            switch (actorType)
            {
                case WfActor.TypeEnum.Role:
                    if (role != null)
                    {
                        if (!String.IsNullOrEmpty(groupId))
                        {
                            userList = role.GetUserListByGroupId(groupId);
                        }
                        else if (!String.IsNullOrEmpty(groupCode))
                        {
                            userList = role.GetUserListByGroupCode(groupCode);
                        }
                    }
                    break;
                case WfActor.TypeEnum.Group:
                    if (group != null)
                    {
                        if (!String.IsNullOrEmpty(roleId))
                        {
                            userList = group.GetUserListByRoleId(roleId);
                        }
                        else if (!String.IsNullOrEmpty(roleCode))
                        {
                            userList = group.GetUserListByRoleCode(roleCode);
                        }
                    }
                    break;
                case WfActor.TypeEnum.Func:
                    userList = GetUserListByFunc(actor, ctx);
                    break;
                case WfActor.TypeEnum.User:
                default:
                    userList = GetUserListByUser(userIds, userCode);
                    break;
            }

            userList = userList.Distinct(new EntityComparer<OrgUser>()).ToList();

            return userList;
        }

        /// <summary>
        /// 由角色获取用户信息
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public IList<OrgUser> GetUserListByRole(string roleCode, string groupCode)
        {
            if (String.IsNullOrEmpty(groupCode))
            {
                return null;
            }

            OrgRole role = OrgRole.Get(roleCode);

            var userList = role.GetUserListByGroupCode(groupCode);

            return userList;
        }

        /// <summary>
        /// 由用户信息获取用户
        /// </summary>
        public IList<OrgUser> GetUserListByUser(string idString, string workNoString)
        {
            IList<OrgUser> userList = new List<OrgUser>();

            if (!String.IsNullOrEmpty(idString))
            {
                var ids = idString.Split(',');

                if (ids.Length > 0)
                {
                    var usrs = OrgUser.FindAllByPrimaryKeys(ids);

                    foreach (var usr in usrs)
                    {
                        userList.Add(usr);
                    }
                }
            }

            if (!String.IsNullOrEmpty(workNoString))
            {
                var workNos = workNoString.Split(',');

                if (workNos.Length > 0)
                {
                    var usrs = OrgUser.FindAll(Expression.In(OrgUser.Prop_WorkNo, workNos));

                    foreach (var usr in usrs)
                    {
                        userList.Add(usr);
                    }
                }
            }

            userList = userList.Distinct().ToList();

            return userList;
        }

        /// <summary>
        /// 由组获取用户
        /// </summary>
        public IList<OrgUser> GetUserListByGroup(string groupCode, string roleCode)
        {
            if (String.IsNullOrEmpty(roleCode))
            {
                return null;
            }

            OrgGroup group = OrgGroup.Get(groupCode);

            var userList = group.GetUserListByRoleCode(roleCode);

            return userList;
        }

        /// <summary>
        /// 由方法获取用户
        /// </summary>
        public IList<OrgUser> GetUserListByFunc(WfActor actor, FlowContext ctx = null)
        {
            IList<OrgUser> userList = new List<OrgUser>();

            var funcTypeString = actor.Tag.Get<string>("FuncType");

            object funcObj = null;
            EasyDictionary paramsDict = null;
            string methodName = actor.FuncCode;

            if (!String.IsNullOrEmpty(funcTypeString))
            {
                funcObj = CLRHelper.CreateInstance<object>(funcTypeString);
            }
            else
            {
                funcObj = new WfActorHelper();
            }

            var funcType = funcObj.GetType();
            var method = funcType.GetMethod(methodName);

            userList = method.Invoke(funcObj, new object[] { actor, ctx, paramsDict }) as IList<OrgUser>;

            userList = userList.Distinct().ToList();

            return userList;
        }

        #endregion

        #region Actor 外部调用方法

        /// <summary>
        /// 获取上级
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public IList<OrgUser> GetReportTo(WfActor actor, FlowContext ctx = null, EasyDictionary paramsDict = null)
        {
            IList<OrgUser> userList = new List<OrgUser>();

            var idsString = actor.Tag.Get<string>("Ids");
            var workNosString = actor.Tag.Get<string>("WorkNos");

            var usr = GetUserListByUser(idsString, workNosString).FirstOrDefault();

            if (usr != null)
            {
                var rptTo = usr.GetReportTo();

                if (rptTo != null)
                {
                    userList.Add(rptTo);
                }
            }

            return userList;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 默认审批人, 当没有流程人审批时, 发给此人
        /// </summary>
        /// <returns></returns>
        public static IList<OrgUser> GetDefaultActorUserList()
        {
            IList<OrgUser> userList = new List<OrgUser>();

            var p = Parameter.Get(WfServiceConfig.BpmDefActorsCode);

            if (!String.IsNullOrEmpty(p.Value))
            {
                var acts = GetActorsByString(p.Value);

                if (acts != null && acts.Count > 0)
                {
                    userList = acts.GetUserList();
                }
            }

            // 若未设置默认用户, 默认用户为系统用户
            if (userList.Count == 0)
            {
                userList.Add(OrgUser.SystemUser);
            }

            return userList;
        }

        /// <summary>
        /// 由字符串获取人员信息
        /// </summary>
        /// <param name="actorsString"></param>
        /// <param name="flowState"></param>
        /// <param name="flowRequest"></param>
        /// <param name="taskState"></param>
        /// <param name="taskRequest"></param>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static WfActorCollection GetActorsByString(string actorsString, FlowContext ctx = null,
            WfDefine flowDefine = null, WfInstance flowInstance = null, FlowState flowState = null, FlowRequest flowRequest = null, TaskState taskState = null, TaskRequest taskRequest = null,
            FlowBasicInfo basicInfo = null, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            string actorsTmplString = WfHelper.GetWfDataString(actorsString, ctx,
                flowDefine, flowInstance, flowState, flowRequest, taskState, taskRequest, 
                basicInfo, formData, actionInfo);

            var actors = JsonHelper.GetObject<WfActorCollection>(actorsTmplString);

            return actors;
        }

        #endregion
    }
}
