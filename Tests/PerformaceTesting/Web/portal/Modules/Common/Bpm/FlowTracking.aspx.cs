using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Common.Bpm
{
    public partial class FlowTracking : BaseListPage
    {
        #region 变量

        string iid = String.Empty;   // 对象分类id

        private IList<WfTask> ents = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            iid = RequestData.Get<string>("iid", RequestData.Get<string>("id"));

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
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (!String.IsNullOrEmpty(iid))
            {
                if (SearchCriterion.Orders.Count == 0)
                {
                    SearchCriterion.SetOrder(WfTask.Prop_StartedTime, false);
                }

                SearchCriterion.SetSearch(WfTask.Prop_InstanceID, iid);

                ents = WfTask.FindAll(SearchCriterion);
                this.PageState.Add("EntList", ents);
            }
        }

        #endregion
    }
}

