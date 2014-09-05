using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Biz.Model;

namespace PIC.Biz.Web
{
    public partial class OA_YellowPageList : BizListPage
    {
        #region 变量

        private IList<OA_YellowPage> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestAction)
            {
                default:
                    if (RequestActionString == "batchdelete")
                    {
						DoBatchDelete();
                    } 
                    else 
                    {
						DoSelect();
					}
					break;
            }
            
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
			ents = OA_YellowPage.FindAll(SearchCriterion);
			this.PageState.Add("OA_YellowPageList", ents);
		}
		
		/// <summary>
        /// 批量删除
        /// </summary>
		[ActiveRecordTransaction]
		private void DoBatchDelete()
		{
			IList<object> idList = RequestData.GetList<object>("IdList");

			if (idList != null && idList.Count > 0)
			{                   
				OA_YellowPage.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

