using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using NServiceBus;
using PIC.Common;
using PIC.Common.Service;
using PIC.Common.Authentication;
using PIC.Portal;
using PIC.Portal.Web.Services;
using PIC.AppServices.Providers;
using PIC.Portal.Model;

namespace PIC.AppServices.Providers
{
    /// <summary>
    /// 供AppService使用的PortalServiceProvider，因为AppService本身提供了ServiceProvider实现
    /// 这里需要比较大的调整，很多地方需要直接调用API
    /// </summary>
    public class AppPortalServiceProvider : IPortalServiceProvider
    {
        #region 成员

        private IUserSessionService ussc = null;

        #endregion

        #region 构造函数

        public AppPortalServiceProvider()
        {
            ussc = new UserSessionService();
        }

        #endregion

        #region 属性

        protected IUserSessionService USService
        {
            get
            {
                if (ussc == null)
                {
                    ussc = new UserSessionService();
                }

                return ussc;
            }
        }

        #endregion

        #region IPortalServiceProvider 成员

        /// <summary>
        /// 获取NServiceBus
        /// </summary>
        /// <returns></returns>
        public virtual IServiceBus RetrieveServiceBus()
        {
            return null;
        }

        /// <summary>
        /// 获取当前系统令牌
        /// </summary>
        /// <returns></returns>
        public virtual SysIdentity GetCurrentSysIdentity()
        {
            SysIdentity sysIdentity = null;

            if (Thread.CurrentPrincipal != null)
            {
                sysIdentity = WebHelper.GetSysIdentity(Thread.CurrentPrincipal);
            }

            if (sysIdentity != null && !String.IsNullOrEmpty(sysIdentity.UserSID))
            {
                if (sysIdentity.UserInfo == null)
                {
                    sysIdentity.UserInfo = GetUserInfo(sysIdentity.UserSID);
                }
            }

            return sysIdentity;
        }

        /// <summary>
        /// 设置当前认证信息
        /// </summary>
        /// <param name="sessionID"></param>
        public virtual void SetCurrentPrincipal(string sessionID)
        {
            if (!String.IsNullOrEmpty(sessionID) && sessionID.Length >= 30)
            {
                UserInfo ui = this.GetUserInfo(sessionID);

                var identity = new SysIdentity(ui);
                var principal = new SysPrincipal(identity);

                // AppDomain.CurrentDomain.SetThreadPrincipal(principal);

                Thread.CurrentPrincipal = principal;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public virtual UserInfo GetUserInfo(string sessionID)
        {
            return GetUserData<UserInfo>(sessionID, "getuserinfo");
        }

        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public virtual UserSessionState GetUserSessionState(string sessionID)
        {
            if (USService.CheckUserSession(sessionID))
            {
                return UserSessionState.Valid;
            }

            return UserSessionState.None;
        }

        public virtual void RefreshSession(string sessionID)
        {
            USService.RefreshSession(sessionID);
        }

        public virtual void ReleaseSession(string sessionID)
        {
            USService.ReleaseSession(sessionID);
        }

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="authPackage"></param>
        /// <returns></returns>
        public virtual string AuthenticateUser(IAuthPackage authPackage)
        {
            AuthMessage am = new AuthMessage();
            am.LoginName = authPackage.LoginName;
            am.Password = authPackage.Password;
            am.PasswordEncrypted = authPackage.PasswordEncrypted;
            am.AuthType = authPackage.AuthType;
            am.IP = authPackage.IP;

            string sid = USService.AuthenticateUser(am.ToString());

            return sid;
        }

        /// <summary>
        /// 登出系统
        /// </summary>
        public virtual void Logout(string sessionID)
        {
            ReleaseSession(sessionID);             
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sid"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        protected T GetUserData<T>(string sid, string op)
        {
            return RetrieveUserData<T>(sid, op);
        }

        /// <summary>
        /// 获取系统数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="op"></param>
        /// <returns></returns>
        protected T GetSystemData<T>(string op)
        {
            return RetrieveSystemData<T>(op);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sid"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected T RetrieveUserData<T>(string sid, string operation)
        {
            OpMessage opMsg = new OpMessage();
            opMsg.SessionID = sid;
            opMsg.Operation = operation;

            byte[] usrdata = USService.GetUserData(opMsg.ToString());

            var obj = ServiceHelper.DeserializeFromBytes<T>(usrdata);

            return obj;
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected T RetrieveSystemData<T>(string operation)
        {
            OpMessage opMsg = new OpMessage();
            opMsg.Operation = operation;

            byte[] usrdata = USService.GetSystemData(opMsg.ToString());

            var obj = ServiceHelper.DeserializeFromBytes<T>(usrdata);

            return obj;
        }

        #endregion
    }
}
