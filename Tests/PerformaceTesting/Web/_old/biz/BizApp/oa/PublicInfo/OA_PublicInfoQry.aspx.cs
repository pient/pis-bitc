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
    public partial class OA_PublicInfoQry : BizListPage
    {
        #region 变量

        private IList<OA_PublicInfo> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestAction)
            {
                default:
                    DoSelect();
                    break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("BooleanEnum", Enumeration.GetEnumDict("Common.Data.Boolean"));
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Type"));
                PageState.Add("GradeEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Grade"));
                PageState.Add("StatusEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Status"));
            }
            
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            ents = OA_PublicInfo.FindAll(SearchCriterion);
            this.PageState.Add("OA_PublicInfoList", ents);
        }

        #endregion
    }
}

