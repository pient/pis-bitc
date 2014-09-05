using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace PIC.Common.Authentication
{
    public class SysIdentity : IIdentity
    {
        #region 私有函数

        private UserInfo userInfo = null;

        public virtual LoginTypeEnum AuthType { get; set; }

        #endregion

        #region 属性

        /// <summary>
        /// 返回用户SessionID
        /// </summary>
        public string UserSID
        {
            get { return this.UserInfo.SessionID; }
        }

        /// <summary>
        /// 认证信息
        /// </summary>
        public UserInfo UserInfo
        {
            get { return this.userInfo; }
            set { this.userInfo = value; }
        }

        #endregion

        #region 构造函数

        public SysIdentity(UserInfo ui)
        {
            this.userInfo = ui;
        }

        #endregion

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "PIC"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get
            {
                if (userInfo != null)
                {
                    return userInfo.Name;
                }

                return null;
            }
        }

        #endregion
    }
}
