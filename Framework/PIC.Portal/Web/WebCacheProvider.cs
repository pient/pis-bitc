using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using PIC.Caching;

namespace PIC.Portal.Web
{
    public class WebCacheProvider : ICacheProvider
    {
        System.Web.Caching.Cache cache = new System.Web.Caching.Cache();

        public void Set(string objId, object o)
        {
            if (cache.Get(objId) != null)
            {
                cache[objId] = o;
            }
            else
            {
                cache.Insert(objId, o);
            }
        }

        public void Remove(string objId)
        {
            cache.Remove(objId);
        }

        public object Get(string objId)
        {
            return cache.Get(objId);
        }
    }
}
