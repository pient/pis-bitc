using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Script.Serialization;

using PIC.Data;
using PIC.Common;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class UserMgmt : BaseListPage
    {
        #region 成员属性

        private IList<OrgUser> ents = null;

        private OrgUser ent;
        private string id = String.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");

            switch (RequestActionString)
            {
                case "resetpwd":
                    ResetPwd();
                    break;
                case "savegroup":
                    SaveGroup();
                    break;
                case "batchdelete":
                    DoBatchDelete();
                    break;
                default:
                    DoSelect();
                    break;
            }
        }

        #endregion

        #region 私有方法

        private void DoSelect()
        {
            SearchCriterion.SetOrder(OrgUser.Prop_WorkNo, true);

            ents = OrgUser.FindAll(SearchCriterion);

            this.PageState.Add("EntList", ents);
        }

        private void ResetPwd()
        {
            string pwd = RequestData.Get<string>("pwd", "");

            if (!String.IsNullOrEmpty(id))
            {
                ent = OrgUser.Find(id);
                ent.ResetPassword(pwd);
            }
        }

        private void SaveGroup()
        {
            if (!String.IsNullOrEmpty(id))
            {
                var joinIDs = RequestData.GetList<string>("joinIDs");
                var quitIDs = RequestData.GetList<string>("quitIDs");

                var user = OrgUser.Find(id);

                if (user != null)
                {
                    user.JoinGroupByIDs(null, joinIDs.ToArray());
                    user.QuitGroupByIDs(quitIDs.ToArray());
                }
            }
        }

        private void DoBatchDelete()
        {
            var idList = RequestData.GetIdList();

            if (idList != null && idList.Count > 0)
            {
                OrgUser.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}
