using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PIC.Data;

namespace PIC.Portal.Web.UI
{
    public class BaseFormPage : BasePage
    {
        #region 构造函数

        public BaseFormPage()
        {
            PageType = PageTypeEnum.Form;
        }

        #endregion
    }
}
