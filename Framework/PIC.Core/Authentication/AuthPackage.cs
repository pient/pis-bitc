using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Common.Authentication
{
    public interface IAuthPackage
    {
        /// <summary>
        /// 登录名
        /// </summary>
        string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// 是否加密
        /// </summary>
        bool PasswordEncrypted { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        string IP { get; set; }

        /// <summary>
        /// 客户端MAC地址
        /// </summary>
        string MAC { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        LoginTypeEnum AuthType { get; set; }
    }

    [Serializable]
    public class AuthPackage : IAuthPackage
    {
        #region IAuthPackage 成员

        public string LoginName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool PasswordEncrypted
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }

        public string MAC
        {
            get;
            set;
        }

        public LoginTypeEnum AuthType
        {
            get;
            set;
        }

        #endregion
    }
}
