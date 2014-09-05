using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Common;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web.Modules.Setup.Org
{
    public partial class UserConfig : BaseFormPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");

            OrgUser ent = null;

            switch (RequestActionString)
            {
                case "update":
                case "save":
                    DoSave();
                    break;

            }

            if (!IsAsyncRequest)
            {
                if (op != "c")
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        ent = OrgUser.Find(id);
                        var usrCfg = ent.GetConfig();

                        PageState.Add("UserData", ent);
                        PageState.Add("ConfigData", usrCfg.ConfigInfo);
                    }
                }
            }
        }



        #region 支持方法

        /// <summary>
        /// 暂存流程数据
        /// </summary>
        private void DoSave()
        {
            var userid = RequestData.Get<string>("userid");
            var email = RequestData.Get<string>("email");
            var picture = RequestData.Get<MinFileInfo>("picture");
            var bpmcfg = RequestData.Get<UserBpmConfig>("bpmcfg");
            var signature = RequestData.Get<MinFileInfo>("signature");

            // 更新邮件信息
            var usr = OrgUser.Find(userid);
            usr.Email = email;
            usr.DoUpdate();

            var usrCfg = usr.GetConfig();

            usrCfg.ConfigInfo.Bpm = bpmcfg;
            usrCfg.ConfigInfo.Basic.Picture = picture;
            usrCfg.ConfigInfo.Basic.Signature = signature;

            usrCfg.DoSave();

            this.SetMessage("保存成功！");
        }

        #endregion
    }
}
