using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Web.UI
{
    /// <summary>
    /// 由于DataPage的访问量比较大，特别是在Portlet的访问，这里对其进行特殊处理
    /// </summary>
    public class BaseDataPage : BasePage
    {
        #region 构造函数

        public BaseDataPage()
        {
            IsPortalContentPage = false;

            if (!String.IsNullOrEmpty(PortalService.CurrentUserSID))
            {
                this.IsCheckLogon = false;
            }
        }

        #endregion
    }
}
