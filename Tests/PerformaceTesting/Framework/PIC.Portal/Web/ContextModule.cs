using System;
using System.Threading;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Castle.ActiveRecord;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Data;
using PIC.Portal.Model;

namespace PIC.Portal.Web
{
    /// <summary>
    /// 处理用户的验证
    /// </summary>
    public class ContextModule : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);

            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        public void Dispose()
        {

        }

        #endregion

        /// <summary>
        /// 认证请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            // 防外部链接
            //if (!WebHelper.IsOuterLink())
            //{
                AcquireRequestIdentity();
            //}
        }

        /// <summary>
        /// 请求开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Items.Contains(NHSessionService.DEFAULT_SESSIONSCOPE_KEY))
            {
                HttpContext.Current.Items.Add(NHSessionService.DEFAULT_SESSIONSCOPE_KEY, new SessionScope(FlushAction.Never));
                // HttpContext.Current.Items.Add(NHSessionService.DEFAULT_SESSIONSCOPE_KEY, new SessionScope(FlushAction.Never));
            }
        }

        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_EndRequest(object sender, EventArgs e)
        {
            try
            {
                SessionScope scope = HttpContext.Current.Items[NHSessionService.DEFAULT_SESSIONSCOPE_KEY] as SessionScope;

                if (scope != null)
                {
                    scope.Dispose();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Error", "EndRequest: " + ex.Message, ex);
            }
        }

        #region 静态方法

        /// <summary>
        /// 获取登陆标识信息
        /// </summary>
        internal static void AcquireRequestIdentity()
        {
            IPrincipal user = HttpContext.Current.User;
            string requestPath = HttpContext.Current.Request.FilePath.ToLower();

            // 只有aspx页面才需要验证
            if (!(requestPath.EndsWith("aspx") || requestPath.EndsWith("ashx")))
            {
                return;
            }

            // 用户认证
            if (user != null && user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
                FormsIdentity formIdentity = user.Identity as FormsIdentity;
                string uistr = formIdentity.Ticket.UserData;

                if (!String.IsNullOrEmpty(uistr))
                {
                    try
                    {
                        UserInfo ui = JsonHelper.GetObject<UserInfo>(uistr);
                        // UserInfo ui = AuthService.GetUserInfo(uistr);

                        SysIdentity si = new SysIdentity(ui);
                        SysPrincipal sp = new SysPrincipal(si);
                        HttpContext.Current.User = sp;
                    }
                    catch
                    {
                        AuthService.Logout();
                    }
                }
            }
            // for testing - by ray 2014-3-13
            else
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("test_user_name");

                if (cookie != null && !String.IsNullOrEmpty(cookie.Value))
                {
                    OrgUser orgUser = OrgUser.FindFirstByProperties(OrgUser.Prop_LoginName, cookie.Value);

                    if (orgUser != null)
                    {
                        UserInfo ui = new UserInfo()
                        {
                            UserID = orgUser.UserID,
                            LoginName = orgUser.LoginName,
                            Name = orgUser.Name
                        };

                        SysIdentity si = new SysIdentity(ui);
                        SysPrincipal sp = new SysPrincipal(si);
                        HttpContext.Current.User = sp;
                    }
                }
            }

            // RewriteRequestPath();
        }

        /// <summary>
        /// rewrite request
        /// </summary>
        internal static void RewriteRequestPath()
        {
            string requestPath = HttpContext.Current.Request.FilePath;

            HttpContext.Current.RewritePath(requestPath);
        }

        #endregion
    }
    
}
