using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Security;
using NServiceBus;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Portal;
using PIC.Portal.Web;

namespace PIC.Portal.ServicesProvider
{
    [PartMetadata(ContantValue.PIC_MEF_EXPORT_KEY, ContantValue.PIC_MEF_EXPORT)]
    [Export(typeof(IPortalServiceProvider))]
    public class WebPortalServiceProvider : PortalServiceProvider
    {
        #region PortalServiceProvider 成员

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public override UserInfo GetUserInfo(string sessionID)
        {
            UserInfo userInfo = null;

            if (HttpContext.Current.Session != null 
                && HttpContext.Current.Session.Keys.Contains("UserInfo", true))
            {
                userInfo = HttpContext.Current.Session["UserInfo"] as UserInfo;
            }

            if (userInfo == null)
            {
                userInfo = base.GetUserInfo(sessionID);

                userInfo.ExtData.Set("LastAccessTime", DateTime.UtcNow);
            }
            else
            {
                var lastAccessTime = userInfo.ExtData.Get<DateTime>("LastAccessTime");

                var refreshSeconds = PICConfigurationManager.ServicesConfiguration.UserSession.Get<int>("WebSessionRefreshIntervalSeconds", 10);

                // 10秒钟去刷新AppService中的用户登录信息一次（防止过多的访问AppService，影响性能）
                if ((DateTime.UtcNow - lastAccessTime) > TimeSpan.FromSeconds(refreshSeconds))
                {
                    userInfo = base.GetUserInfo(sessionID);

                    userInfo.ExtData.Set("LastAccessTime", DateTime.UtcNow);
                }
            }

            return userInfo;
        }

        /// <summary>
        /// 获取NServiceBus
        /// </summary>
        /// <returns></returns>
        public override IServiceBus RetrieveServiceBus()
        {
            //IBus bus = NServiceBus.Configure.WithWeb()
            //    .Log4Net()
            //    .DefaultBuilder()
            //    .XmlSerializer()
            //    .MsmqTransport()
            //        .IsTransactional(false)
            //        .PurgeOnStartup(false)
            //    .UnicastBus()
            //        .ImpersonateSender(false)
            //    .CreateBus()
            //    .Start();

            IBus bus = null;

            return new ServiceBus(bus);
        }

        /// <summary>
        /// 获取当前系统令牌
        /// </summary>
        /// <returns></returns>
        public override SysIdentity GetCurrentSysIdentity()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            SysIdentity sysIdentity = WebHelper.GetSysIdentity(HttpContext.Current.User);

            // for testing - by ray 2014-3-13
            // return sysIdentity;

            if (sysIdentity != null && !String.IsNullOrEmpty(sysIdentity.UserSID))
            {
                if (sysIdentity.UserInfo == null)
                {
                    sysIdentity.UserInfo = GetUserInfo(sysIdentity.UserSID);
                }

                return sysIdentity;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置当前认证信息
        /// </summary>
        /// <param name="sessionID"></param>
        public override void SetCurrentPrincipal(string sessionID)
        {
            if (!String.IsNullOrEmpty(sessionID) && sessionID.Length >= 30)
            {
                UserInfo ui = this.GetUserInfo(sessionID);

                // 判断返回的是否用户状态
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(ui.LoginName, false);

                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                // string uistr = ServiceHelper.SerializeToBase64(ui);
                string uistr = JsonHelper.GetJsonString(ui);
                // string uistr = sessionID;

                FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                    ticket.Version, ticket.Name, ticket.IssueDate,
                    ticket.Expiration, ticket.IsPersistent, uistr);

                authCookie.Value = FormsAuthentication.Encrypt(newTicket);

                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        /// <summary>
        /// 登出系统
        /// </summary>
        public override void Logout(string sessionID)
        {
            base.Logout(sessionID);

            HttpContext.Current.Session.Abandon();

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
        }

        #endregion
    }
}
