using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Modules.Common.Usr
{
    public partial class MyProfile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (RequestActionString)
            {
                case "save":
                    DoSave();
                    break;
            }

            if (!IsAsyncRequest)
            {
                var usr = OrgUser.Find(UserInfo.UserID);
                var usrCfg = usr.GetConfig();

                PageState.Add("UserData", usr);
                PageState.Add("ConfigData", usrCfg.ConfigInfo);
            }
        }

        #region 支持方法

        /// <summary>
        /// 暂存流程数据
        /// </summary>
        private void DoSave()
        {
            var email = RequestData.Get<string>("email");
            var picture = RequestData.Get<MinFileInfo>("picture");
            var bpmcfg = RequestData.Get<UserBpmConfig>("bpmcfg");
            var signature = RequestData.Get<MinFileInfo>("signature");

            // 更新邮件信息
            var usr = OrgUser.Find(UserInfo.UserID);
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
