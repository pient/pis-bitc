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

namespace PIC.Biz.Web.RecruitNeeds
{
    public partial class List : BizSimpleListPage
    {
        #region 变量

        private IList<FormInstance> ents = null;
        private string formCode = null;
        private string formName = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            formCode = RequestData.Get<string>("code");
            formName = RequestData.Get<string>("name");

            if (!String.IsNullOrEmpty(formName))
            {
                this.Title = formName;
            }

            FormInstance ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<FormInstance>();
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
            if (!String.IsNullOrEmpty(formCode))
            {
                ents = FormInstance.FindAll(SearchCriterion,
                    Expression.Eq(FormInstance.Prop_FormCode, formCode),
                    Expression.Eq(FormInstance.Prop_CreatorID, UserInfo.UserID));

                this.PageState.Add("DataList", ents);
            }
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
                FormInstance.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

