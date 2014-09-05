using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIC.Portal.Model;
using PIC.Portal.Services;

namespace PIC.Biz.Web
{
    [Serializable]
    public class SimpleDataTemplate : DataRequestTemplate
    {
        public string Header
        {
            get;
            set;
        }

        public string Footer
        {
            get;
            set;
        }
    }
}