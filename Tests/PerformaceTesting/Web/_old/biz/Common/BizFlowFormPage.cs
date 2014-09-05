using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PIC.Data;
using PIC.Portal.Web.UI;

namespace PIC.Biz.Web
{
    public class BizFlowFormPage : BizBasePage
    {
        #region 构造函数

        public BizFlowFormPage()
        {
            PageType = PageTypeEnum.Bpm;
        }

        #endregion
    }
}
