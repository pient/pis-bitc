using PIC.Aop;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Data;
using PIC.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PIC.AppServices
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            InitApplication();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ProcessRequest();
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

        #region 支持方法

        /// <summary>
        /// 初始化应用
        /// </summary>
        private void InitApplication()
        {
            Portal.PortalService.Initialize();

            // 载入执行计划
            Portal.SchedulerService.ReloadSchedule();

            //日志
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        private void ProcessRequest()
        {
        }

        #endregion
    }
}