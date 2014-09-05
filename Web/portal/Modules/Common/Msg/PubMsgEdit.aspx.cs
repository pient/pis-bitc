using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Common;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;
using PIC.Data;

namespace PIC.Portal.Web.Modules.Common.Msg
{
    public partial class PubMsgEdit : BaseFormPage
    {
        #region 变量

        string mode = String.Empty; // 查看类型mgmt, read
        string id = String.Empty;   // 对象id

        Message ent = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            mode = RequestData.Get<string>("mode", "read");
            id = RequestData.Get<string>("id");

            switch (this.RequestActionString)
            {
                case "save":
                    DoSave();
                    break;
                case "publish":
                    DoPublish();
                    break;
                case "unpublish":
                    UnPublish();
                    break;
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Message.Find(id);
                }

                this.SetFormData(ent);

                PageState.Add("MsgImportantEnum", Enumeration.GetEnumDict("SysUtil.Message.Important"));
                PageState.Add("PubInfoCatalogEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Catalog"));
                PageState.Add("PubInfoStatusEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Status"));
            }
        }

        #endregion

        #region 支持方法

        /// <summary>
        /// 保存
        /// </summary>
        private void DoSave()
        {
            ent = this.GetMergedData<Message>();

            if (String.IsNullOrEmpty(ent.SysType))
            {
                ent.MessageSysType = Message.SysTypeEnum.PubInfo;
            }

            ent.FromID = this.UserInfo.UserID;
            ent.FromName = this.UserInfo.Name;
            ent.OwnerID = this.UserInfo.UserID;
            ent.OwnerName = this.UserInfo.Name;

            ent.DoSave();
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        public void DoPublish()
        {
            ent = this.GetMergedData<Message>();

            if (String.IsNullOrEmpty(ent.SysType))
            {
                ent.MessageSysType = Message.SysTypeEnum.PubInfo;
            }

            ent.FromID = this.UserInfo.UserID;
            ent.FromName = this.UserInfo.Name;
            ent.OwnerID = this.UserInfo.UserID;
            ent.OwnerName = this.UserInfo.Name;
            ent.SentDate = DateTime.Now;

            if (String.IsNullOrEmpty(ent.Catalog))
            {
                throw new MessageException("必须提供消息类型！");
            }

            ent.Status = "Published";

            ent.DoSave();
        }

        /// <summary>
        /// 撤销发布
        /// </summary>
        public void UnPublish()
        {
            ent = this.GetMergedData<Message>();

            if (ent.Status != "Published")
            {
                throw new MessageException("只能撤销发布已发布的消息！");
            }

            ent.Status = "UnPublished";

            ent.DoSave();
        }

        #endregion
    }
}
