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
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Setup.Dev.TS
{
    public partial class TaskInstanceList : BaseListPage
    {
        #region 变量

        private IList<TaskInstance> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
			TaskInstance ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<TaskInstance>();
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
			ents = TaskInstance.FindAll(SearchCriterion);
			this.PageState.Add("SysTaskInstanceList", ents);
		}
		
		/// <summary>
        /// 批量删除
        /// </summary>
		[ActiveRecordTransaction]
		private void DoBatchDelete()
        {
            var idList = RequestData.GetIdList();

			if (idList != null && idList.Count > 0)
			{                   
				TaskInstance.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

