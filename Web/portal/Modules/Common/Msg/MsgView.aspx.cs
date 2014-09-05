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
    public partial class MsgView : BaseFormPage
    {
        #region 变量

        string id = String.Empty;   // 对象id
        string refId = String.Empty;   // 参考标识

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");
            refId = RequestData.Get<string>("refId");

            Message ent = null;

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = Message.Find(id);

                    if (ent.OwnerID == UserInfo.UserID 
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
