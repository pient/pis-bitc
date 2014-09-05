using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PIC.Portal.Web.VirtualPathProvider
{
    public class ExtPortalVirtualPathProvider : PortalVirtualPathProvider
    {
        public const string ProviderResourceLocation = "PIC.Portal.Web.VirtualPathProvider.Ext";
        public const string ProviderVirtualPath = "~/Resources";

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtPortalVirtualPathProvider"/> class.
        /// </summary>
        public ExtPortalVirtualPathProvider()
            : base()
        {
        }

        #endregion

        #region PortalVirtualPathProvider成员

        /// <summary>
        /// 获取虚拟目录参数
        /// </summary>
        /// <returns></returns>
        protected override PortalVirtualPathParams GetPortalVirtualPathParams()
        {
            PortalVirtualPathParams pvpp = new PortalVirtualPathParams();

            pvpp.ResourceLocation = ProviderResourceLocation;
            pvpp.VirtualPath = ProviderVirtualPath;
            pvpp.SiteMasterPageLocation = String.Format("{0}/Masters/site.Master", ProviderVirtualPath);
            pvpp.SimpleMasterPageLocation = String.Format("{0}/Masters/simple.Master", ProviderVirtualPath);
            pvpp.FormMasterPageLocation = String.Format("{0}/Masters/form.master", ProviderVirtualPath);
            pvpp.BpmMasterPageLocation = String.Format("{0}/Masters/bpm.master", ProviderVirtualPath);

            return pvpp;
        }

        /// <summary>
        /// 从虚拟文件系统获取文件
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsPathVirtual(virtualPath))
            {
                return new ExtPortalVirtualFile(virtualPath, this);
            }
            else
            {
                return Previous.GetFile(virtualPath);
            }
        }

        #endregion
    }
}
