using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PIC.Data;

namespace PIC.Portal.Web.UI
{
    public class BaseSimplePage : BasePage
    {
        #region 构造函数

        public BaseSimplePage()
        {
            PageType = PageTypeEnum.Simple;
        }

        #endregion

        #region ASP.NET 事件

        /// <summary>
        /// 初始化之前触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        #endregion
    }
}
