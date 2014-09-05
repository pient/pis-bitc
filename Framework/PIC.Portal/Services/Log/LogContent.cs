using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    /// <summary>
    /// 日志容器
    /// </summary>
    public class LogContent : EasyDictionary
    {
        #region 重载

        public override string ToString()
        {
            string rtnstr = JsonHelper.GetJsonString(this);

            return rtnstr;
        }

        #endregion
    }
}
