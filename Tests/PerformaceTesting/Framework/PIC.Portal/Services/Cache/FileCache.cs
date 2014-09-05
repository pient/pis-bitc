using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PIC.Portal
{
    public class FileCache : StreamCache
    {
        public override string CachePath
        {
            get
            {
                return "/Portal/File";
            }
        }

        #region 构造函数

        internal FileCache()
        {
        }

        internal FileCache(long sizeLimit)
            : base(sizeLimit)
        {
        }

        internal FileCache(long sizeLimit, long singleSizeLimit, long warningSize)
            : base(sizeLimit, singleSizeLimit, warningSize)
        {
        }

        #endregion
    }
}
