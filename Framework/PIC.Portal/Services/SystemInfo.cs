using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Management;
using NHibernate;
using NHibernate.Criterion;
using PIC.Security;
using PIC.Portal.Model;

namespace PIC.Portal
{
    public class SystemInfo
    {
        #region 属性

        /// <summary>
        /// 系统时间
        /// </summary>
        public DateTime Date
        {
            get { return DateTime.Now; }
        }

        public string SystemName
        {
            get;
            private set;
        }

        public string CompanyName
        {
            get;
            private set;
        }

        public string CompanyWebsite
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数

        public SystemInfo()
        {
            SystemName = Parameter.GetValue("System.Name", "");
            CompanyName = Parameter.GetValue("System.CompanyName", "");
            CompanyWebsite = Parameter.GetValue("System.CompanyWebsite", "");
        }

        #endregion
    }
}
