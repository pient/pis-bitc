using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Scopes;

namespace PIC.Data
{
    public class NHSessionService : IDisposable
    {
        #region 成员

        // 默认Session Scope键
        public const string DEFAULT_SESSIONSCOPE_KEY = "PIC_DEFAULT_NHSESSIONSCOPE_KEY";

        // 默认Session键
        public const string DEFAULT_SESSION_KEY = "PIC_DEFAULT_NHSESSION_KEY";

        private static NHSessionService nhss = null;

        /// <summary>
        /// 单体
        /// </summary>
        public static NHSessionService Instance
        {
            get
            {
                if (nhss == null)
                {
                    nhss = new NHSessionService();
                }

                return nhss;
            }
        }

        #endregion

        #region 构造函数

        private NHSessionService()
        {
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}
