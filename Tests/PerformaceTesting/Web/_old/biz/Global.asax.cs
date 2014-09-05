using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;

using PIC.Aop;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Web;
using PIC.Portal.Model;
using PIC.Biz.Model;

namespace PIC.Biz.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            InitApplication();
        }

        #region 初始化应用

        private void InitApplication()
        {
            // 初始化PortalService
            PortalService.Initialize(new string[] { "PIC.Biz" });

            //日志
            log4net.Config.XmlConfigurator.Configure();

            //日志、异常
            LogAttribute.del += WriteLog;
            ExceptionAttribute.del += WriteLog;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        protected void WriteLog(string message)
        {
            //System.Web.HttpContext.Current.Response.Write(message);
        }

        #endregion

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}