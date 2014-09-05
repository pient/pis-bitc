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
    public partial class PubMsgListView : BaseListPage
    {
        #region 变量

        private Message[] ents = null;

        string catalog = string.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            catalog = RequestData.Get<string>("catalog");

            DoSelect();

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
            ents = Message.FindAll(SearchCriterion,
                Expression.Eq(Message.Prop_Status, "Published"),
                Expression.Eq(Message.Prop_Catalog, catalog),
                Expression.Eq(Message.Prop_SysType, Message.SysTypeEnum.PubInfo.ToString()));

            this.PageState.Add("EntList", ents);
        }

        #endregion
    }
}

