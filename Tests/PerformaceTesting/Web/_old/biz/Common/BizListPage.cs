using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Biz.Web
{
    public class BizListPage : BizBasePage
    {
        #region 私有成员

        #endregion

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

        public BizListPage()
        {
            SearchCriterion.AllowPaging = true;
            SearchCriterion.GetRecordCount = true;
        }

        #endregion

        #region 事件

        protected override void Page_PreRender(object sender, EventArgs e)
        {
            PageState.Add(SearchCriterionStateKey, SearchCriterion);

            base.Page_PreRender(sender, e);
        }

        #endregion
    }
}
