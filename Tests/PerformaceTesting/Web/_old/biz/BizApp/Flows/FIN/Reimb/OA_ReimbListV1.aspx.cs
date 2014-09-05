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

namespace PIC.Biz.Web.Reimbursement
{
    public partial class OA_ReimbListV1 : BizListPage
    {
        #region 变量

        private IList<OA_Reimbursement> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            SearchCriterion.PageSize = 2;

			OA_Reimbursement ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<OA_Reimbursement>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    switch (RequestActionString)
                    {
                        case "batchdelete":
                            DoBatchDelete();
                            break;
                        default:
                            DoSelect();
                            break;
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
			ents = OA_Reimbursement.FindAll(SearchCriterion, Expression.Eq(OA_Reimbursement.Prop_CreatorId, UserInfo.UserID));
			this.PageState.Add("OA_ReimbursementList", ents);
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
				OA_Reimbursement.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

