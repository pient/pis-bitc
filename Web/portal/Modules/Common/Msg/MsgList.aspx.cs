using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Portal.Web.UI;
using PIC.Portal.Model;


namespace PIC.Portal.Web.Modules.Common.Msg
{
    public partial class MsgList : BaseListPage
    {
        #region 变量

        private Message[] ents = null;
        
        string type = string.Empty;
        string mode = String.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            type = RequestData.Get<string>("type");
            mode = RequestData.Get<string>("mode", "read"); // mgmt, read, 管理模式和阅读模式两种

            Message ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<Message>();
                    ent.DoCreate();
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Update:
                    ent = this.GetMergedData<Message>();
                    ent.DoUpdate();
                    this.SetMessage("保存成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<Message>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
                        DoBatchDelete();
                    }
                    else
                    {
                        DoSelect();
                    }
                    break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("MsgStatusEnum", Enumeration.GetEnumDict("SysUtil.Message.Status"));
                PageState.Add("MsgTypeEnum", Enumeration.GetEnumDict("SysUtil.Message.Type"));
                PageState.Add("MsgImportantEnum", Enumeration.GetEnumDict("SysUtil.Message.Important"));
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (mode == "read" && String.IsNullOrEmpty(type))
            {
                type = "Received";
            }

            if (mode != "mgmt" || !AuthService.IsSysAdmin(UserInfo))
            {
                SearchCriterion.SetSearch(Message.Prop_OwnerID, this.UserInfo.UserID, SearchModeEnum.Equal);
            }

            if (!String.IsNullOrEmpty(type))
            {
                SearchCriterion.SetSearch(Message.Prop_Type, type, SearchModeEnum.Equal);
            }

            switch (type)
            {
                case "Sent":
                case "Received":
                    SearchCriterion.SetOrder(Message.Prop_SentDate, false);
                    break;
                default:
                    SearchCriterion.SetOrder(Message.Prop_CreatedDate, false);
                    break;
            }

            ents = Message.FindAll(SearchCriterion,
                Expression.Eq(Message.Prop_SysType, Message.SysTypeEnum.Message.ToString()));

            this.PageState.Add("EntList", ents);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        private void DoBatchDelete()
        {
            var idList = RequestData.GetIdList();

            if (idList != null && idList.Count > 0)
            {
                Message.DoBatchDelete(idList.ToArray());
            }
        }

        #endregion
    }
}

