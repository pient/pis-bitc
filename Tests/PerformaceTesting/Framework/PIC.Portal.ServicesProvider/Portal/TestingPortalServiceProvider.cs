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
using System.Threading;

namespace PIC.Portal.ServicesProvider
{
    [PartMetadata(ContantValue.PIC_MEF_EXPORT_KEY, ContantValue.PIC_MEF_EXPORT)]
    [Export(typeof(IPortalServiceProvider))]
    public class TestingPortalServiceProvider : PortalServiceProvider
    {
        #region Strus & Class

        public class MockUserInfo : UserInfo
        {
            #region 构造函数

            public MockUserInfo()
            {
                SessionID = Guid.NewGuid().ToString();
                UserID = "Mock_USER_ID";
                LoginName = "模拟登录名";
                Name = "模拟用户名";
            }

            #endregion
        }

        #endregion

        #region PortalServiceProvider 成员

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
        /// 获取当前系统令牌(多线程时如何传递Prinicipal)
        /// </summary>
        /// <returns></returns>
        public override SysIdentity GetCurrentSysIdentity()
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
            else
            {
                // 设置模拟Identity
                sysIdentity = new SysIdentity(new MockUserInfo());
            }

            return sysIdentity;
        }

        /// <summary>
        /// 登出系统
        /// </summary>
        public override void Logout(string sessionID)
        {
            base.Logout(sessionID);

            Thread.CurrentPrincipal = null;
        }

        #endregion
    }
}
