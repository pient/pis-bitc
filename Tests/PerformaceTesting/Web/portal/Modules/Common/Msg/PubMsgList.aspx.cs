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
    public partial class PubMsgList : BaseListPage
    {
        #region 变量

        private Message[] ents = null;

        string catalog = string.Empty;
        string status = string.Empty;
        string mode = String.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            catalog = RequestData.Get<string>("catalog");
            status = RequestData.Get<string>("status");
            mode = RequestData.Get<string>("mode", "read"); // mgmt, read, 管理模式和阅读模式两种

            Message ent = null;
            switch (this.RequestAction)
            {
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
                PageState.Add("PubInfoStatusEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Status"));
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
            if (mode != "mgmt" || !AuthService.IsSysAdmin(UserInfo))
            {
                if (!String.IsNullOrEmpty(status))
                {
                    return;
                }

                SearchCriterion.SetSearch(Message.Prop_OwnerID, this.UserInfo.UserID, SearchModeEnum.Equal);
            }

            if (!String.IsNullOrEmpty(status))
            {
                SearchCriterion.SetSearch(Message.Prop_Status, status, SearchModeEnum.Equal);
            }

            switch (status)
            {
                case "Published":
                    SearchCriterion.SetOrder(Message.Prop_SentDate, false);
                    break;
                default:
                    SearchCriterion.SetOrder(Message.Prop_CreatedDate, false);
                    break;
            }

            ents = Message.FindAll(SearchCriterion,
                Expression.Eq(Message.Prop_Catalog, catalog),
                Expression.Eq(Message.Prop_SysType, Message.SysTypeEnum.PubInfo.ToString()));

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

