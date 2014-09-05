using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using PIC.Aop;
using PIC.Data;
using PIC.Portal.Model;

namespace PIC.Portal.Services
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
            EntityManager.InitializeEntity(new string[] { "PIC.Portal" });

            // 载入执行计划
            // IScheduleService ss = new PIC.Portal.Services.ScheduleService();
            // ss.Reset();

            SysSystem system = SysSystem.GetCurrentSystemInfo();

            if (system != null)
            {
                Application["SysSystem"] = system;
            }

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

        /// <summary>
        /// 初始化消息队列
        /// </summary>
        protected void InitMSMQ()
        {

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
            // 系统释放
            UserSessionServer.Instance.Dispose();
        }
    }
}