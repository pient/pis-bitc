using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Caching
{
    public class CacheEventArgs : EventArgs
    {
        public string Xpath
        {
            get;
            private set;
        }

        public object Object
        {
            get;
            private set;
        }

        public CacheEventArgs(string xpath, object obj)
        {
            Xpath = xpath;
            Object = obj;
        }
    }
}
