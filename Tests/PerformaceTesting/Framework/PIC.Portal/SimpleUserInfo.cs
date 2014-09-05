using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Common;

namespace PIC.Portal
{
    /// <summary>
    /// 简单的用户信息
    /// </summary>
    [Serializable]
    public class SimpleUserInfo : UserInfo
    {
        #region 构造函数

        public SimpleUserInfo(string sessionID)
            : this(AuthService.GetUserInfo(sessionID))
        {
        }

        public SimpleUserInfo(UserInfo ui)
        {
            this.SessionID = ui.SessionID;
            this.UserID = ui.UserID;
            this.LoginName = ui.LoginName;
            this.Name = ui.Name;
            this.MAC = ui.MAC;
            this.IP = ui.IP;
        }

        #endregion
    }
}
