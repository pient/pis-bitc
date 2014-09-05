using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Setup.Mdl
{
    public partial class ModuleMgmt : BasePage
    {
        #region 属性

        string op = String.Empty; // 用户编辑操作
        string mid = String.Empty;

        IList<string> ids = null;   // 节点列表
        IList<string> pids = null;   // 父节点列表

        #endregion

        #region 变量

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            mid = RequestData.Get<string>("mid");

            ids = RequestData.GetList<string>("ids");
            pids = RequestData.GetList<string>("pids");

            switch (this.RequestActionString)
            {
                case "refreshsys":
                    PortalService.RefreshSystemContext();
                    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }

            if (!IsAsyncRequest)
            {
                DataEnum de = ModuleType.GetModuleTypeEnum();
                this.PageState.Add("MdlTypeEnum", de);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            Module[] mdls = null;

            if (RequestActionString == "querychildren")
            {
                if (ids != null && ids.Count > 0 || pids != null && pids.Count > 0)
                {
                    ICriterion crit = null;

                    if (ids != null && ids.Count > 0)
                    {
                        IEnumerable<string> distids = ids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));
                        crit = Expression.In(Module.Prop_ModuleID, distids.ToArray());
                    }

                    if (pids != null && pids.Count > 0)
                    {
                        IEnumerable<string> dispids = pids.Distinct().Where(tent => !String.IsNullOrEmpty(tent));

                        if (crit != null)
                        {
                            crit = SearchHelper.UnionCriterions(crit, Expression.In(Module.Prop_ParentID, dispids.ToArray()));
                        }
                        else
                        {
                            crit = Expression.In(Module.Prop_ParentID, dispids.ToArray());
                        }
                    }

                    mdls = Module.FindAll(crit);
                }
                else if (String.IsNullOrEmpty(mid) || mid == "root")
                {
                    mdls = Module.FindAll("FROM Module as mdl WHERE mdl.ParentID IS NULL");
                }
                else
                {
                    mdls = Module.FindAll("FROM Module as mdl WHERE mdl.ParentID = ?", mid);
                }
            }
            else
            {
                var portalApp = Model.Application.GetPortalApp();
                mdls = Module.FindAll("FROM Module as mdl WHERE mdl.ApplicationID = ? AND mdl.ParentID IS NULL", portalApp.ApplicationID);
            }

            mdls = mdls.OrderBy(v => v.SortIndex).ThenBy(v => v.CreateDate).ToArray();

            this.PageState.Add("MdlList", mdls);
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
                Module.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}
