using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PIC.Portal.Model;

namespace PIC.Portal
{
    public class MsgService
    {
        #region 成员属性

        #endregion

        #region 构造函数

        private static readonly MsgService msgserivce = new MsgService();

        private static MsgService Instance
        {
            get
            {
                return msgserivce;
            }
        }

        protected MsgService()
        {
        }

        #endregion

        #region 公共方法

        #endregion
    }
}
