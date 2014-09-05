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
    public partial class MsgEdit : BaseFormPage
    {
        #region 变量

        string mode = String.Empty; // 查看类型mgmt, read
        string id = String.Empty;   // 对象id
        string refId = String.Empty;
        string toids = String.Empty;
        string sendtype = String.Empty;   // 发送类型
        string type = String.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            mode = RequestData.Get<string>("mode", "read");
            id = RequestData.Get<string>("id");
            refId = RequestData.Get<string>("refId");
            type = RequestData.Get<string>("type");
            sendtype = RequestData.Get<string>("sendtype");
            toids = RequestData.Get<string>("toids");

            Message ent = null;

            switch (this.RequestActionString)
            {
                case "send":
                case "save":
                    if (String.IsNullOrEmpty(FormData.Get<string>(Message.Prop_MessageID)))
                    {
                        ent = this.GetPostedData<Message>();
                    }
                    else
                    {
                        ent = this.GetMergedData<Message>();
                    }

                    ent.FromID = this.UserInfo.UserID;
                    ent.FromName = this.UserInfo.Name;

                    if (this.RequestActionString == "send")
                    {
                        ent.MessageSendType = CLRHelper.GetEnum<Message.SendTypeEnum>(sendtype, Message.SendTypeEnum.Send);
                        ent.RefID = refId;

                        ent.DoSend();
                    }
                    else
                    {
                        ent.OwnerID = this.UserInfo.UserID;
                        ent.OwnerName = this.UserInfo.Name;
                        ent.DoSave();
                    }
                    break;
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Message.Find(id);

                    if (mode != "mgmt"
                        && ent.OwnerID == UserInfo.UserID 
                        && ent.MessageType == Message.TypeEnum.Received
                        && ent.MessageStatus == Message.StatusEnum.New)
                    {
                        ent.MessageStatus = Message.StatusEnum.Readed;

                        ent.DoUpdate();

                        // 标识读操作
                        PageState.Add("IsReadAction", true);
                    }
                }

                this.SetFormData(ent);

                if (!String.IsNullOrEmpty(refId))
                {
                    var refMsg = Message.Find(refId);

                    if (refId != null)
                    {
                        PageState.Add("RefMessage", refMsg);
                    }
                }

                PageState.Add("MsgImportantEnum", Enumeration.GetEnumDict("SysUtil.Message.Important"));
            }
        }

        #endregion
    }
}
