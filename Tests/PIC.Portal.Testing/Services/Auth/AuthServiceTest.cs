using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PIC.Portal.Model;

namespace PIC.Portal.Testing.Services
{
    [TestFixture]
    public class AuthServiceTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();
        }

        [Test]
        public void GetAllUserAuth_Test()
        {
            var user = OrgUser.FindFirstByProperties(OrgUser.Prop_Name, "admin");

            var auths = AuthService.GetAllUserAuth(user);
        }

        [Test]
        public void GetAccessible_Test()
        {
            var user = OrgUser.FindFirstByProperties(OrgUser.Prop_Name, "admin");

            var apps = AuthService.GetAccessibleApplications(user);

            var mdls = AuthService.GetAccessibleModules(user);
        }

        /// <summary>
        /// 北信统一认证系统
        /// </summary>
        [Test]
        public void ICELogin_Test()
        {
            Type idMangerType = Type.GetTypeFromProgID("Idstar.IdentityManager");
            Idstar.CIdentityManager idStarObj = Activator.CreateInstance(idMangerType) as Idstar.CIdentityManager;

            int result1 = idStarObj.CheckPassword("testnk", "testnk");
            int result2 = idStarObj.CheckPassword("testnk", "testnk0");
        }
    }
}
