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
    public partial class TaskList : BaseListPage
    {
        #region 变量

        private IList<Task> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            Task ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Task>();
                    ent.DoDelete();
                    break;
                default:
                    switch (RequestActionString)
                    {
                        case "batchdelete":
                            DoBatchDelete();
                            break;
                        case "run":
                            DoRun();
                            break;
                        default:
                            DoSelect();
                            break;
                    }
                    break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("SysTaskScheduler.TaskType"));
                PageState.Add("CatalogEnum", Enumeration.GetEnumDict("SysTaskScheduler.TaskCatalog"));
                PageState.Add("StatusEnum", Enumeration.GetEnumDict("SysTaskScheduler.TaskStatus"));
            }
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
            ents = Task.FindAll(SearchCriterion,
                Expression.Not(Expression.Eq(Task.Prop_Type, "Inner")));
            this.PageState.Add("EntList", ents);
		}

        /// <summary>
        /// 立即执行任务
        /// </summary>
        private void DoRun()
        {
            string id = RequestData.Get<string>("id");

            var result = SchedulerService.RunTask(id);

            this.SetMessage(result.Message);
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
				Task.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

