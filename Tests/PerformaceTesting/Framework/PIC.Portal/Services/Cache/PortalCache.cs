using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Caching;
using PIC.Common;

namespace PIC.Portal
{
    public class PortalCache : Cache
    {
        public const string PORTAL_CACHE_PROVIDER_KEY = "CacheProvider";

        #region 属性

        public virtual string CachePath
        {
            get
            {
                return "/Portal/General";
            }
        }

        #endregion

        #region 构造函数

        internal PortalCache()
        {
            string typeName = PICConfigurationManager.SystemConfiguration.AppSettings[PORTAL_CACHE_PROVIDER_KEY];

            cs = (ICacheProvider)Activator.CreateInstance(Type.GetType(typeName));
        }

        #endregion

        #region 重载

        public override void Set(string path, object o)
        {
            if (path.IndexOf('/') != 0)
            {
                path = "/" + path;
            }

            path = CachePath + path;

            base.Set(path, o);
        }

        public override void Remove(string path)
        {
            if (path.IndexOf('/') != 0)
            {
                path = "/" + path;
            }

            path = CachePath + path;

            base.Remove(path);
        }

        public override object Get(string path)
        {
            if (path.IndexOf('/') != 0)
            {
                path = "/" + path;
            }

            path = CachePath + path;

            return base.Get(path);
        }

        #endregion
    }
}
