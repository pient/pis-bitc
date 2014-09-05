using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Web.UI
{
    public class BaseFlowPage : BasePage
    {
        #region 构造函数

        public BaseFlowPage()
        {
            PageType = PageTypeEnum.Bpm;
        }

        #endregion
    }
}
