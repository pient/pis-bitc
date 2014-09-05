using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PIC.Caching;

namespace PIC.Portal
{
    public class StreamCache : PortalCache
    {
        #region 成员

        protected long sizeLimit = 200000000;  // Cache大小限制，默认200M(超过时强制清空Cache)
        protected long singleSizeLimit = 20000000;  // Cache大小限制，默认20M(为sizeLimit的1/10, 超过时抛出Cache异常)
        protected long warningSize = 150000000;   // Cache警戒线，默认150M(为sizeLimit的3/4，超过将触发CacheWarning事件)
        protected long totalSize = 0;   // 当前Cache大小

        public event EventHandler CacheWarning;  // 当当前Cache大小超过限制时触发, 默认自动清空

        #endregion 

        #region 属性

        public override string CachePath
        {
            get
            {
                return "/Portal/Stream";
            }
        }

        // Cache大小限制
        public long SizeLimit
        {
            get
            {
                return sizeLimit;
            }
        }

        /// <summary>
        /// 当前Cache大小
        /// </summary>
        public long CurrentSize
        {
            get
            {
                return totalSize;
            }
        }

        private IList<string> cachePathhList = new List<string>();

        #endregion

        #region 构造函数

        internal StreamCache()
        {
        }

        internal StreamCache(long sizeLimit)
        {
            this.sizeLimit = sizeLimit;
            this.singleSizeLimit = (long)(sizeLimit * 0.1);
            this.warningSize = (long)(sizeLimit * 0.75);
        }

        internal StreamCache(long sizeLimit, long singleSizeLimit, long warningSize)
        {
            this.sizeLimit = sizeLimit;
            this.singleSizeLimit = singleSizeLimit;
            this.warningSize = warningSize;
        }

        #endregion

        #region Cache方法

        public override void Set(string path, object o)
        {
            byte[] sbytes = o as byte[];

            if (sbytes != null)
            {
                if (sbytes.Length > singleSizeLimit)
                {
                    throw new CacheException("对象过大，无法添加。");
                }

                long tsize = totalSize + sbytes.Length;

                if (tsize > sizeLimit)
                {
                    Clear();
                }
                else
                {
                    base.Set(path, sbytes);

                    if (tsize > warningSize)
                    {
                        if (CacheWarning != null)
                        {
                            CacheWarning(this, new CacheEventArgs(path, o));
                        }
                    }
                }

                lock (cachePathhList)
                {
                    cachePathhList.Add(path);
                }
            }
            else
            {
                throw new CacheException("Cache对象不能为空");
            }
        }

        public override void Remove(string path)
        {
            byte[] sbytes = Get(path) as byte[];

            if (sbytes != null)
            {
                totalSize -= sbytes.Length;

                base.Remove(path);

                lock (cachePathhList)
                {
                    cachePathhList.Remove(path);
                }
            }
            else
            {
                throw new CacheException("不存在对于Cache对象");
            }
        }

        #endregion

        #region 私有方法

        protected virtual void Clear()
        {
            foreach (string cachePath in cachePathhList)
            {
                Remove(cachePath);
            }

            totalSize = 0;
        }

        #endregion

    }
}
