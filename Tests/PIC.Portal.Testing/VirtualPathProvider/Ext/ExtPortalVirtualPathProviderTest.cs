using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using PIC.Portal.Web;

namespace PIC.Portal.Testing.VirtualPathProvider.Ext
{
    [TestFixture]
    public class ExtPortalVirtualPathProviderTest
    {
        public PortalVirtualPathProvider epvpp = null;

        [TestFixtureSetUp]
        public void SetUp()
        {
            epvpp = WebPortalService.GetPortalVirtualPathProvider() as PortalVirtualPathProvider;
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            epvpp = null;
        }

        [Test]
        public void ExtPortalVirtualPathProviderInstanceTest()
        {
            // Assert.AreEqual(epvpp.Params.VirtualPath, ExtPortalVirtualPathProvider.ProviderVirtualPath);
            Assert.IsTrue(epvpp.FileExists(epvpp.Params.SiteMasterPageLocation));
            Assert.IsTrue(epvpp.FileExists(epvpp.Params.FormMasterPageLocation));
        }

        [Test]
        public void ExtPortalVirtualFileInstanceTest()
        {
            PortalVirtualFile smppvf = new PortalVirtualFile(epvpp.Params.SiteMasterPageLocation.TrimStart('~'), epvpp);

            Assert.IsNotNull(smppvf.Open());

            //Assembly assembly = epvpp.GetType().Assembly;
            //Stream stream = assembly.GetManifestResourceStream("PIC.Portal.Web.VirtualPathProvider.Ext.Resources.Masters.site.master");
            //Assert.IsNotNull(stream);
        }
    }
}
