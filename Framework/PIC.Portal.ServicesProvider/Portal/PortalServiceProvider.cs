using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ComponentModel.Composition;
using NServiceBus;
using PIC.Common;
using PIC.Common.Service;
using PIC.Common.Authentication;
using PIC.Portal;
using PIC.Portal.Web.Services;
using System.Threading;

namespace PIC.Portal.ServicesProvider
{
    [PartMetadata(ContantValue.PIC_MEF_EXPORT_KEY, ContantValue.PIC_MEF_EXPORT)]
    [Export(typeof(IPortalServiceProvider))]
    public class PortalServiceProvider : IPortalServiceProvider
    {
        #region 成员

        private UserSessionService.UserSessionServiceClient ussc;
        // private IUserSessionService ussc = null;

        #endregion

        #region 构造函数

        public PortalServiceProvider()
        {
            ussc = new UserSessionService.UserSessionServiceClient();
            // ussc = new Web.Services.UserSessionService();
        }

        #endregion

        #region 属性

        protected UserSessionService.IUserSessionService USService
        {
            get
            {
                if (ussc == null)
                {
                    ussc = new UserSessionService.UserSessionServiceClient();
                    // ussc = new Web.Services.UserSessionService();
                }

                if (ussc.State == CommunicationState.Closed || ussc.State == CommunicationState.Faulted)
                {
                    ussc = new UserSessionService.UserSessionServiceClient();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取当前系统令牌
        /// </summary>
        /// <returns></returns>
        public virtual SysIdentity GetCurrentSysIdentity()
        {
            throw new NotImplementedException();
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
            try
            {
                if (USService.CheckUserSession(sessionID))
                {
                    return UserSessionState.Valid;
                }

                return UserSessionState.None;
            }
            catch (TimeoutException)
            {
                ussc.Abort();
            }
            catch (CommunicationException)
            {
                ussc.Abort();
            }

            return UserSessionState.None;
        }

        public virtual void RefreshSession(string sessionID)
        {
            try
            {
                USService.RefreshSession(sessionID);
            }
            catch (TimeoutException)
            {
                ussc.Abort();
            }
            catch (CommunicationException)
            {
                ussc.Abort();
            }
        }

        public virtual void ReleaseSession(string sessionID)
        {
            try
            {
                USService.ReleaseSession(sessionID);
            }
            catch (TimeoutException)
            {
                ussc.Abort();
            }
            catch (CommunicationException)
            {
                ussc.Abort();
            }
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

            string sid = String.Empty;

            try
            {
                sid = USService.AuthenticateUser(am.ToString());

                this.SetCurrentPrincipal(sid);
            }
            catch (TimeoutException)
            {
                ussc.Abort();
            }
            catch (CommunicationException)
            {
                ussc.Abort();
            }

            return sid;
        }

        /// <summary>
        /// 登出系统
        /// </summary>
        public virtual void Logout(string sessionID)
        {
            ReleaseSession(sessionID);             
        }

        /// <summary>
        /// 获取在线用户数
        /// </summary>
        /// <returns></returns>
        public virtual int GetOnlineUserCount()
        {
            int onlineUserCount = GetSystemData<int>("getonlineusercount");

            return onlineUserCount;
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

            T obj = default(T);

            try
            {
                byte[] usrdata = USService.GetUserData(opMsg.ToString());

                obj = ServiceHelper.DeserializeFromBytes<T>(usrdata);
            }
            catch (TimeoutException)
            {
                ussc.Abort();
            }
            catch (CommunicationException)
            {
                ussc.Abort();
            }

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

            T obj = default(T);

            try
            {
                byte[] usrdata = USService.GetSystemData(opMsg.ToString());

                obj = ServiceHelper.DeserializeFromBytes<T>(usrdata);
            }
            catch (TimeoutException)
            {
                ussc.Abort();
            }
            catch (CommunicationException)
            {
                ussc.Abort();
            }

            return obj;
        }

        #endregion
    }
}
