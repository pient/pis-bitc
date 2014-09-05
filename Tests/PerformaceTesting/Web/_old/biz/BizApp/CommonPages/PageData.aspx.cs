using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using PIC.Common.Service;
using PIC.Data;
using NHibernate;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using PIC.Common;
using PIC.Portal.Model;
using PIC.Portal.Services;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Biz.Model;

namespace PIC.Biz.Web.CommonPages
{
    public partial class PageData : BaseDataPage
    {
        #region ASP.NET 事件

        protected override void OnInit(EventArgs e)
        {
            this.IsCheckLogon = CLRHelper.ConvertValue<bool>(Request["ischecklogon"], false);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder content = new StringBuilder();

            switch (this.RequestActionString)
            {
                case "qrydata":
                    content = QueryData();
                    break;
            }

            Response.Write(content);
            Response.End();

        }

        #endregion

        #region 获取数据

        ISession session;

        string id, code, subcode;    // 数据编码, 标识 
        DataQueryPath qpath;    // 数据查询路径

        // 获取数据
        private StringBuilder QueryData()
        {
            StringBuilder sb = new StringBuilder();

            string path = this.RequestData.Get<string>("path", "list");
            qpath = new DataQueryPath(path);

            id = this.RequestData.Get<string>("id");
            code = this.RequestData.Get<string>("code");
            subcode = this.RequestData.Get<string>("subcode");

            Type type = Type.GetType("PIC.Biz.Model.BizTreeNodeModelBase`1, PIC.Biz.Model");

            session = DataHelper.OpenHqlSession(type);

            switch (qpath.DataType)
            {
                case "data":
                    sb = getDataString();
                    break;
                default:
                    sb = getListString();
                    break;
            }

            return sb;
        }

        /// <summary>
        /// 获取列表字符传
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private StringBuilder getListString()
        {
            StringBuilder sb = new StringBuilder();

            string hql = this.RequestData.Get<string>("hql");
            int rows = this.RequestData.Get<int>("rows", 10);

            string rtnstr = String.Empty;

            if (!String.IsNullOrEmpty(hql))
            {
                IQuery qry = DataHelper.GetHqlQuery(session, hql);

                if (!String.IsNullOrEmpty(subcode))
                {
                    qry.SetParameter("subcode", subcode);
                }

                qry.SetMaxResults(rows);

                IList lists = qry.List();
                DataHelper.ReleaseHqlSessin(session);

                sb = new StringBuilder(JsonHelper.GetJsonString(lists));
            }

            return sb;
        }

        /// <summary>
        /// 获取数据字符串
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private StringBuilder getDataString()
        {
            StringBuilder sb = new StringBuilder();

            switch (qpath.Module)
            {
                case "portlet":
                    sb = getPortletDataString();
                    break;
                case "portal":
                    sb = getPortalDataString();
                    break;
            }

            return sb;
        }

        private StringBuilder getPortalDataString()
        {
            StringBuilder sb = new StringBuilder();

            switch (qpath.SubModule)
            {
                case "msgtask":
                    sb.Append(this.PackPageState());
                    break;
            }

            return sb;
        }

        /// <summary>
        /// 获取门户数据字符串
        /// </summary>
        /// <returns></returns>
        private StringBuilder getPortletDataString()
        {
            StringBuilder sb = new StringBuilder();
            Portlet tmpl;
            SimpleDataTemplate dttmpl;

            if (!String.IsNullOrEmpty(code))
            {
                tmpl = Portlet.Get(code);
                dttmpl = JsonHelper.GetObject<SimpleDataTemplate>(tmpl.Config);

                IQuery qry = DataHelper.GetHqlQuery(session, dttmpl.Hql);

                if (!String.IsNullOrEmpty(subcode))
                {
                    switch (qpath.SubModule)
                    {
                        case "publicinfo":
                            qry.SetString("type", subcode);
                            break;
                    }
                }

                qry.SetMaxResults(dttmpl.Rows);
                IList lists = qry.List();

                sb.Append(dttmpl.Header);

                string tmpstr = String.Empty;

                for (int i = 0; i < lists.Count; i++)
                {
                    // get data item string
                    tmpstr = dttmpl.Item.Replace("#{ITEM_ORDER}", (i + 1).ToString())
                        .Replace("#{HTTP_HOST}", Request.ServerVariables["HTTP_HOST"]);

                    switch (qpath.SubModule)
                    {
                        case "publicinfo":
                            tmpstr = getPublicInfoString(lists[i], tmpstr);
                            break;
                    }

                    sb.Append(tmpstr);
                }

                sb.Append(dttmpl.Footer);
            }
            else if (!String.IsNullOrEmpty(id))
            {
                switch (qpath.SubModule)
                {
                    case "infocontent":
                        OA_PublicInfo ent = OA_PublicInfo.Read(id);

                        sb.Append(JsonHelper.GetJsonString(ent));
                        break;
                }
            }

            return sb;
        }

        /// <summary>
        /// 获取图片新闻数据
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        private string getPublicInfoString(object ent, string tmplstr)
        {
            string tmppic = String.Empty;

            OA_PublicInfo item = ent as OA_PublicInfo;            
            tmppic = item.GetValue<string>("Picture");

            if (!String.IsNullOrEmpty(tmppic) && tmppic.IndexOf('_') > 0)
            {
                tmppic = tmppic.Substring(0, tmppic.IndexOf('_'));
            }

            string tmpstr = tmplstr.Replace("${Id}", item.GetValue<string>("Id"))
                .Replace("${Title}", item.GetValue<string>("Title"))
                .Replace("${Picture}", tmppic);

            return tmpstr;
        }

        #endregion
    }
}