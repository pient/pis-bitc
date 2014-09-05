using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Common.Authentication;
using PIC.Common;
using System.Collections;

namespace PIC.Portal
{
    public class AuthService
    {
        public static object uclslocker = new object(); // 添加一个对象作为UserContextList的锁

        #region Consts & Enums

        public const string SysGroupCode = "SYS";
        public const string SysAdminCode = "Sys_Adm";

        #endregion

        #region 成员属性

        public static IPortalServiceProvider PSProvider
        {
            get
            {
                return PortalService.Provider;
            }
        }

        // 初始化化用户上下文列表
        protected Dictionary<string, UserContext> userContextList = new Dictionary<string, UserContext>();

        /// <summary>
        /// 用户上下文列表
        /// </summary>
        internal Dictionary<string, UserContext> UserContextList
        {
            get
            {
                if (userContextList == null)
                {
                    lock (uclslocker)
                    {
                        if (userContextList == null)
                        {
                            userContextList = new Dictionary<string, UserContext>();
                        }
                    }
                }

                return userContextList;
            }
        }

        /// <summary>
        /// 获取当前用户SessionID
        /// </summary>
        public static string CurrentUserSID
        {
            get
            {
                SysIdentity sysIdentity = PSProvider.GetCurrentSysIdentity();

                if (sysIdentity != null)
                {
                    return sysIdentity.UserSID;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static UserInfo CurrentUserInfo
        {
            get
            {
                SysIdentity sysIdentity = PSProvider.GetCurrentSysIdentity();

                if (sysIdentity != null)
                {
                    if (sysIdentity.UserInfo == null)
                    {
                        sysIdentity.UserInfo = PSProvider.GetUserInfo(sysIdentity.UserSID);
                    }

                    return sysIdentity.UserInfo;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取当前用户上下文
        /// </summary>
        public static UserContext CurrentUserContext
        {
            get
            {
                try
                {
                    lock (uclslocker)
                    {
                        if (Instance.UserContextList.ContainsKey(CurrentUserInfo.LoginName))
                        {
                            return Instance.UserContextList[CurrentUserInfo.LoginName];
                        }
                        else if (!String.IsNullOrEmpty(CurrentUserSID) && !String.IsNullOrEmpty(CurrentUserInfo.LoginName))
                        {
                            UserContext uc = new UserContext(CurrentUserSID);

                            Instance.UserContextList.Add(CurrentUserInfo.LoginName, uc);

                            return uc;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception) { return null; }
            }
        }

        #endregion

        #region 构造函数

        // 单体
        private static readonly AuthService authserivce = new AuthService();

        private static AuthService Instance
        {
            get
            {
                return authserivce;
            }
        }

        protected AuthService()
        {
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 由登录SessionId获取用户信息
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string sid)
        {
            return PSProvider.GetUserInfo(sid);
        }

        /// <summary>
        /// 检查当前页是否有权限
        /// </summary>
        /// <returns></returns>
        public static void CheckAuth(string pgKey)
        {
            string upperPgKey = pgKey.ToUpper();

            if (((CurrentUserContext.AccessibleApplications.Where(app => app.Code != null && app.Code.ToUpper() == upperPgKey).Count() > 0)
                    || (CurrentUserContext.AccessibleModules.Where(app => app.Code != null && app.Code.ToUpper() == upperPgKey).Count() > 0)))
            {
                PortalService.Provider.RefreshSession(CurrentUserSID);
            }
            else
            {
                throw new System.Security.SecurityException("您没有使用该系统的权限，请联系管理员。");
            }
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string AuthUser(string uname, string pwd)
        {
            return AuthUser(uname, pwd, false);
        }

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string AuthUser(string uname, string pwd, bool passwordEncrypted)
        {
            IAuthPackage authPackage = WebHelper.GetAuthPackage(uname, pwd, passwordEncrypted);

            string sid = PSProvider.AuthenticateUser(authPackage);

            LogService.Log("Login with sid: " + sid, LogTypeEnum.Info);

            if (!String.IsNullOrEmpty(sid))
            {
                if (Instance.UserContextList.ContainsKey(uname))
                {
                    Instance.UserContextList.Remove(uname);
                }

                UserContext context = Instance.GetUserContext(sid);
                Instance.UserContextList.Add(uname, context);
            }

            return sid;
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public static bool IsUserLogon()
        {
            bool isLogon = false;

            if (!String.IsNullOrEmpty(CurrentUserSID))
            {
                UserSessionState usState = GetUserSessionState(CurrentUserSID);

                if (usState == UserSessionState.Valid)
                {
                    isLogon = true;

                    PSProvider.RefreshSession(CurrentUserSID);
                }
            }

            return isLogon;
        }

        /// <summary>
        /// 检查当前用户是否登录
        /// </summary>
        public static void CheckLogon()
        {
            if (!IsUserLogon())
            {
                // 没有权限访问，先登出，然后抛出异常。
                Logout();

                throw new System.Security.SecurityException("登录超时或没有登录，请重新登录。");
            }
        }

        /// <summary>
        /// 注销当前用户
        /// </summary>
        /// <returns></returns>
        public static void Logout()
        {
            if (!String.IsNullOrEmpty(CurrentUserSID))
            {
                PSProvider.Logout(CurrentUserSID);
            }

            if (CurrentUserInfo != null)
            {
                string loginName = CurrentUserInfo.LoginName;

                lock (uclslocker)
                {
                    if (!String.IsNullOrEmpty(loginName) && Instance.UserContextList.ContainsKey(loginName))
                    {
                        Instance.UserContextList.Remove(loginName);
                    }
                }
            }
        }

        /// <summary>
        /// 检查用户状态
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool CheckUserSession()
        {
            if (!String.IsNullOrEmpty(CurrentUserSID))
            {
                return CheckUserSession(CurrentUserSID);
            }

            return false;
        }

        /// <summary>
        /// 获取用户Session状态
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static UserSessionState GetUserSessionState(string sid)
        {
            return PSProvider.GetUserSessionState(sid);
        }

        /// <summary>
        /// 检查用户状态
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>j
        public static bool CheckUserSession(string sid)
        {
            if (!String.IsNullOrEmpty(sid))
            {
                return PSProvider.GetUserSessionState(sid) == UserSessionState.Valid;
            }

            return false;
        }

        /// <summary>
        /// 系统管理员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static bool IsSysAdmin(UserInfo userInfo)
        {
            var isSysAdmin = HasRole(userInfo, SysAdminCode, SysGroupCode);

            return isSysAdmin;
        }

        public static bool HasRole(UserInfo userInfo, string roleCode, string groupCode = null)
        {
            string groupID = null,
                roleID = null;

            if (!String.IsNullOrEmpty(groupCode))
            {
                var group = OrgGroup.Get(groupCode);

                if (group != null)
                {
                    groupID = group.GroupID;
                }
            }

            if (!String.IsNullOrEmpty(roleCode))
            {
                var role = OrgRole.Get(roleCode);

                if (role != null)
                {
                    roleID = role.RoleID;
                }
            }

            var hasRole = HasRole(userInfo.UserID, roleID, groupID);

            return hasRole;
        }

        /// <summary>
        /// 是否拥有某个组个某个角色
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static bool HasRole(string userID, string roleID, string groupID = null)
        {
            var crits = new List<ICriterion>();

            crits.Add(Restrictions.Eq(OrgUser.Prop_UserID, userID));
            crits.Add(Restrictions.Eq(OrgRole.Prop_RoleID, roleID));

            if (!String.IsNullOrEmpty(groupID))
            {
                crits.Add(Restrictions.Eq(OrgGroup.Prop_GroupID, groupID));
            }

            var hasRole = OrgUserGroup.Exists(crits.ToArray());

            return hasRole;
        }

        /// <summary>
        /// 获取当前用户可访问应用
        /// </summary>
        /// <returns></returns>
        public static IList<Application> GetAccessibleApplications(OrgUser orgUser)
        {
            var authList = orgUser.RetrieveAllAuth();

            IEnumerable<string> appids = authList.Where(v =>
                v.Type == 1
                && String.IsNullOrEmpty(v.ModuleID)
                && !String.IsNullOrEmpty(v.Data)).Select(v => v.Data);

            var appList = PortalService.SystemContext.Applications.Where(tent => appids.Contains(tent.ApplicationID)).ToList();

            return appList;
        }

        /// <summary>
        /// 获取当前用户可访问模块
        /// </summary>
        /// <returns></returns>
        public static IList<Module> GetAccessibleModules(OrgUser orgUser)
        {
            var authList = orgUser.RetrieveAllAuth();

            IEnumerable<string> mdlids = authList.Where(v => 
                v.Type == 1  && !String.IsNullOrEmpty(v.ModuleID)).Select(v => v.ModuleID);

            var mdlList = PortalService.SystemContext.Modules.Where(tent => mdlids.Contains(tent.ModuleID)).ToList();

            return mdlList;
        }

        public static IList<WfDefine> GetAccessibleFlows(OrgUser orgUser)
        {
            var authList = orgUser.RetrieveAllAuth();

            var defIds = authList.Where(v =>
                v.Type == 2 && !String.IsNullOrEmpty(v.ModuleID)).Select(v => v.ModuleID).ToList();

            var defList = WfDefine.FindAll(
                Expression.In(WfDefine.Prop_DefineID, defIds),
                Expression.Eq(WfDefine.Prop_Status, "Enabled")
                ).OrderBy(f => f.SortIndex).ToList();

            return defList;
        }

        public static IList<Auth> GetAllUserAuth(OrgUser orgUser)
        {
            var authList = orgUser.RetrieveAllAuth();

            return authList;
        }

        /// <summary>
        /// 验证操作请求
        /// </summary>
        /// <param name="opReq"></param>
        /// <returns></returns>
        public static bool VerifyOperationRequest(OperationRequest opReq)
        {
            var isValid = AuthService.CheckUserSession(opReq.SessionID);

            if (isValid == true)
            {
                if (String.IsNullOrEmpty(AuthService.CurrentUserSID)
                    || !StringHelper.IsEqualsIgnoreCase(AuthService.CurrentUserSID, opReq.SessionID))
                {
                    AuthService.PSProvider.SetCurrentPrincipal(opReq.SessionID);
                }
            }
            else
            {
                throw new Exception("用户登录信息已过期，请尝试重新登陆！");
            }

            return isValid;
        }

        /// <summary>
        /// 获取系统用户SessionID
        /// </summary>
        /// <returns></returns>
        internal static string GetSysSessionID()
        {
            AuthService.CheckUserSession(); // 激活服务器UserSession服务

            var sysParam = Parameter.Get(SysSystem.SYS_SESSIONID_CODE);

            if (AuthService.CheckUserSession(sysParam.Value))
            {
                return sysParam.Value;
            }

            sysParam.Value = null;
            sysParam.Update();

            return null;
        }

        // 设置系统SessionID
        internal static void SetSysSessionID(string sessionID)
        {
            var sysParam = Parameter.Get(SysSystem.SYS_SESSIONID_CODE);
            sysParam.Value = sessionID;

            sysParam.Update();
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 获取人员上下文
        /// </summary>
        /// <returns></returns>
        private UserContext GetUserContext(string sid)
        {
            UserContext context = new UserContext(sid);

            return context;
        }

        #endregion
    }

    /// <summary>
    /// 用户上下文
    /// </summary>
    public class UserContext
    {
        #region 成员

        private UserInfo userInfo = null;

        private IList<Auth> auths = null;

        private IList<Model.Application> apps = null;
        private IList<Model.Module> mdls = null;

        private Hashtable _tag;

        #endregion

        #region 构造函数

        public UserContext(string sid)
        {
            if (!String.IsNullOrEmpty(sid))
            {
                userInfo = AuthService.GetUserInfo(sid);
            }

            if (userInfo == null)
            {
                throw new MessageException("获取用户信息失败！请尝试重新登录，若问题持续请联系管理员。");
            }
            else
            {
                auths = AuthService.GetAllUserAuth(this.SysUser);
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 系统用户
        /// </summary>
        private OrgUser SysUser
        {
            get
            {
                return OrgUser.GetByLoginName(UserInfo.LoginName);
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get { return userInfo; }
        }

        public IList<Auth> Auths
        {
            get { return auths; }
        }

        /// <summary>
        /// 用户可访问的应用
        /// </summary>
        public IList<Application> AccessibleApplications
        {
            get
            {
                if (apps == null)
                {
                    lock (this)
                    {
                        if (apps == null)
                        {
                            apps = AuthService.GetAccessibleApplications(this.SysUser);
                        }
                    }
                }

                return apps;
            }
        }

        /// <summary>
        /// 用户可访问的模块
        /// </summary>
        public IList<Model.Module> AccessibleModules
        {
            get
            {
                if (mdls == null)
                {
                    lock (this)
                    {
                        if (mdls == null)
                        {
                            mdls = AuthService.GetAccessibleModules(this.SysUser);
                        }
                    }
                }

                return mdls;
            }
        }

        /// <summary>
        /// 用户可访问的流程
        /// </summary>
        public IList<Model.WfDefine> GetAccessibleFlows()
        {
            var flows = AuthService.GetAccessibleFlows(this.SysUser);

            return flows;
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public Hashtable ExtData
        {
            get
            {
                if (_tag == null)
                {
                    _tag = new Hashtable();
                }

                return _tag;
            }
        }

        #endregion
    }
}
