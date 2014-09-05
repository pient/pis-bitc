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
    public partial class HR_EmployeeJobTitleList : BizListPage
    {
        #region 变量

        private IList<HR_EmployeeJobTitle> ents = null;
        string eid = String.Empty;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            eid = RequestData.Get<string>("eid");

			HR_EmployeeJobTitle ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<HR_EmployeeJobTitle>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
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

            if (!IsAsyncRequest)
            {
                PageState.Add("TitleEnum", Enumeration.GetEnumDict("BizHR.Employee.JobTitle"));
            }
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
        {
            if (!String.IsNullOrEmpty(eid))
            {
                SearchCriterion.AddSearch(HR_EmployeeJobTitle.Prop_EmployeeId, eid, SearchModeEnum.Equal);

                ents = HR_EmployeeJobTitle.FindAll(SearchCriterion);
            }

			this.PageState.Add("HR_EmployeeJobTitleList", ents);
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
				HR_EmployeeJobTitle.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

