using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace PIC.Common.Authentication
{
    public class SysPrincipal : IPrincipal
    {
        #region 私有成员

        private SysIdentity identity;

        #endregion

        #region 构造函数

        public SysPrincipal(UserInfo userInfo)
        {
            this.identity = new SysIdentity(userInfo);
        }

        public SysPrincipal(SysIdentity identity)
        {
            this.identity = identity;
        }

        #endregion

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        #endregion
    }
}
