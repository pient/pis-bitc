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
    public partial class SchedulerList : BaseListPage
    {
        #region 变量

        private IList<Scheduler> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
			Scheduler ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Scheduler>();
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
                        case "stop":
                            DoStop();
                            break;
                        case "reset":
                            DoReset();
                            break;
                        default:
                            DoSelect();
                            break;
                    }
                    break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("SysTaskScheduler.SchedulerType"));
                PageState.Add("CatalogEnum", Enumeration.GetEnumDict("SysTaskScheduler.SchedulerCatalog"));
                PageState.Add("StatusEnum", Enumeration.GetEnumDict("SysTaskScheduler.SchedulerStatus"));
            }
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
            ents = Scheduler.FindAll(SearchCriterion, 
                Expression.Not(Expression.Eq(Scheduler.Prop_Type, "Inner")));

			this.PageState.Add("EntList", ents);
		}

        /// <summary>
        /// 立即执行任务调度
        /// </summary>
        private void DoRun()
        {
            string id = RequestData.Get<string>("id");
            SchedulerService.RunSchedule(id);
        }

        /// <summary>
        /// 重启所有任务调度
        /// </summary>
        private void DoReset()
        {
            SchedulerService.ReloadSchedule();
        }

        /// <summary>
        /// 立即停止任务调度
        /// </summary>
        private void DoStop()
        {
            SchedulerService.StopSchedule();
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
				Scheduler.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

