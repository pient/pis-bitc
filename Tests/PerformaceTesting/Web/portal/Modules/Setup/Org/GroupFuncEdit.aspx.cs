using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Common;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class GroupFuncEdit : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id
        string gid = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");
            gid = RequestData.Get<string>("gid");

            OrgGroupFunction ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                        this.DoCreate();    // 可能需要批量创建
                        break;
                    case RequestActionEnum.Update:
                        ent = this.GetMergedData<OrgGroupFunction>();
                        ent.DoUpdate();
                        break;
                    case RequestActionEnum.Delete:
                        ent = this.GetTargetData<OrgGroupFunction>();
                        ent.DoDelete();
                        break;
                }
            }

            if (op != "c")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    var dicts = DataHelper.QueryDictList("SELECT TOP 1 * FROM vw_OrgGroupFunction WHERE GroupFunctionID = '" + id + "'");

                    this.SetFormData(dicts.FirstOrDefault());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(gid))
                {
                    var grp = OrgGroup.Find(gid);

                    if (grp != null)
                    {
                        this.SetFormData(new { GroupID = grp.GroupID, GroupName = grp.Name });
                    }
                }
            }
        }

        #region 支持方法

        /// <summary>
        /// 包含批量创建
        /// </summary>
        private void DoCreate()
        {
            var ent = this.GetPostedData<OrgGroupFunction>();

            if (!String.IsNullOrEmpty(ent.GroupID) && !String.IsNullOrEmpty(ent.FunctionID))
            {
                var funcIDs = ent.FunctionID.Split(',');    // 批量创建

                var grp = OrgGroup.Find(ent.GroupID);

                grp.AddFuncByIDs(ent, funcIDs);
            }
        }

        #endregion
    }
}
