using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Web;

namespace PIC.Portal.Web.VirtualPathProvider
{
    public class ExtPortalVirtualFile : PortalVirtualFile
    {
        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPageVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
        public ExtPortalVirtualFile(string virtualPath, PortalVirtualPathProvider provider)
            : base(virtualPath, provider)
        {
        }

        #endregion
    }
}
