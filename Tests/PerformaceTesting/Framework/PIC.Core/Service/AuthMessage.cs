using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Data;
using PIC.Common.Authentication;

namespace PIC.Common.Service
{
    public class AuthMessage : OpMessage, IAuthPackage
    {
        #region 私有成员

        private LoginTypeEnum authType = LoginTypeEnum.Unknown;

        #endregion

        #region 成员属性

        /// <summary>
        /// 登陆名
        /// </summary>
        public string LoginName
        {
            get { return this.Content["LoginName"]; }
            set
            {
                this.Content["LoginName"] = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return this.Content["Password"]; }
            set
            {
                this.Content["Password"] = value;
            }
        }

        public bool PasswordEncrypted
        {
            get { return CLRHelper.ConvertValue<bool>(this.Content["PasswordEncrypted"], false); }
            set
            {
                this.Content["PasswordEncrypted"] = value.ToString();
            }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP
        {
            get { return this.Content["IP"]; }
            set
            {
                this.Content["IP"] = value;
            }
        }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC
        {
            get { return this.Content["MAC"]; }
            set
            {
                this.Content["MAC"] = value;
            }
        }

        /// <summary>
        /// 登录类型
        /// </summary>
        public LoginTypeEnum AuthType
        {
            get
            {
                return this.authType;
            }
            set
            {
                SetAuthType(value.ToString());
            }
        }

        #endregion

        #region 构造函数

        public AuthMessage()
            : base()
        {
        }

        public AuthMessage(string dataString)
            : base(dataString)
        {
            SetAuthType(this.Content["AuthType"]);

            PasswordEncrypted = CLRHelper.ConvertValue<bool>(this.Content["PasswordEncrypted"], false);
        }

        public AuthMessage(string loginName, string password, string ip, string mac, LoginTypeEnum authType)
        {
            Content["LoginName"] = loginName;
            Content["Password"] = password;
            Content["IP"] = ip;
            Content["MAC"] = mac;
            Content["PasswordEncrypted"] = false.ToString();
            AuthType = authType;
        }

        #endregion

        #region 私有函数

        private void SetAuthType(string authstr)
        {
            if (!String.IsNullOrEmpty(authstr))
            {
                this.Content["AuthType"] = authstr;

                if (Enum.IsDefined(typeof(LoginTypeEnum), authstr))
                {
                    authType = (LoginTypeEnum)(Enum.Parse(typeof(LoginTypeEnum), authstr, true));
                }
            }
        }

        #endregion
    }
}
