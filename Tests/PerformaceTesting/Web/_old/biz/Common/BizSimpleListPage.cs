using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Biz.Web
{
    public class BizSimpleListPage : BaseSimplePage
    {
        #region 属性

        /// <summary>
        /// 是否允许分页
        /// </summary>
        public bool AllowPaging
        {
            get { return SearchCriterion.AllowPaging; }
            set { SearchCriterion.AllowPaging = value; }
        }

        #endregion

        #region 构造函数

        public BizSimpleListPage()
        {
            SearchCriterion.AllowPaging = true;
            SearchCriterion.GetRecordCount = true;
        }

        #endregion 构造函数

        #region ASP.NET 事件

        protected override void Page_PreRender(object sender, EventArgs e)
        {
            PageState.Add(SearchCriterionStateKey, SearchCriterion);

            base.Page_PreRender(sender, e);
        }

        #endregion
    }
}