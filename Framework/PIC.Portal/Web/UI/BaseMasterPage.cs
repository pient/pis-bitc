using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace PIC.Portal.Web.UI
{
    /// <summary>
    /// MasterPage基类
    /// </summary>
    public abstract class BaseMasterPage : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Title元素
        /// </summary>
        public abstract HtmlGenericControl PageTitle
        {
            get;
        }

        /// <summary>
        /// Body元素
        /// </summary>
        public abstract HtmlGenericControl PageBody
        {
            get;
        }

        /// <summary>
        /// 表单元素
        /// </summary>
        public abstract HtmlForm PageForm
        {
            get;
        }
    }
}
