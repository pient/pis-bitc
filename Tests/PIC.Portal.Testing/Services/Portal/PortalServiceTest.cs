using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PIC.Portal.Model;

namespace PIC.Portal.Testing.Services
{
    [TestFixture]
    public class PortalServiceTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();

            // PortalService.Instance.Initialize(new string[] { "PIC.Examining.Model" });
        }
    }
}
