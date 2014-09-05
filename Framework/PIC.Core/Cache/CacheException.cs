using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Caching
{
    public class CacheException : Exception
    {
        #region 构造函数

        public CacheException()
            : base()
        {
        }

        public CacheException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
