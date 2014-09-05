using PIC.Common.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Common
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo : IUserInfo
    {
        #region 成员

        EasyDictionary _tag;

        #endregion

        #region 构造函数

        public UserInfo()
        {
        }

        #endregion

        #region 属性

        public virtual string SessionID { get; set; }
        public virtual string UserID { get; set; }
        public virtual string LoginName { get; set; }
        public virtual string Name { get; set; }
        public virtual string IP { get; set; }
        public virtual string MAC { get; set; }
        public virtual LoginTypeEnum AuthType { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public EasyDictionary ExtData
        {
            get
            {
                if (_tag == null)
                {
                    _tag = new EasyDictionary();
                }

                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取简单用户信息
        /// </summary>
        /// <returns></returns>
        public MinUserInfo GetMinUserInfo()
        {
            return new MinUserInfo()
            {
                UserID = this.UserID,
                Name = this.Name
            };
        }

        #endregion
    }
}
