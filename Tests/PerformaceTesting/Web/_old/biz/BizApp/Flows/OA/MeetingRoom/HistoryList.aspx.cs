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
using PIC.Biz.Model.Reimbursement;

namespace PIC.Biz.Web.MeetingRoom
{
    public partial class HistoryList : BizListPage
    {
        #region 变量

        private IList<OA_Reimbursement> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.RequestAction)
            {
                default:
                    switch (RequestActionString)
                    {
                        default:
                            DoSelect();
                            break;
                    }
					break;
            }

            if (!IsAsyncRequest)
            {
                DateTime lastMonth = DateTime.Now.AddMonths(-1);
                DateTime lastYear = DateTime.Now.AddYears(-1);

                decimal lastMonthExpense = OA_Reimbursement.GetMonthExpense(lastMonth.Year, lastMonth.Month);
                decimal lastYearExpense = OA_Reimbursement.GetMonthExpense(lastYear.Year, lastYear.Month);

                PageState.Add("LastMonthExpense", lastMonthExpense);
                PageState.Add("LastYearExpense", lastYearExpense);
            }
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
			ents = OA_Reimbursement.FindAll(SearchCriterion
                , Expression.Not(Expression.Eq(OA_Reimbursement.Prop_Status, "New"))
                , Expression.Not(Expression.Eq(OA_Reimbursement.Prop_Status, "")));

			this.PageState.Add("OA_ReimbursementList", ents);
		}

        #endregion
    }
}

