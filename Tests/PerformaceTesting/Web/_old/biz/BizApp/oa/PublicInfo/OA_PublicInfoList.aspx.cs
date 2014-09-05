using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Biz.Model;

namespace PIC.Biz.Web
{
    public partial class OA_PublicInfoList : BizListPage
    {
        #region 变量

        private IList<OA_PublicInfo> ents = null;

        private string op = String.Empty;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");

			OA_PublicInfo ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<OA_PublicInfo>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
                        DoBatchDelete();
                    }
                    else if (this.RequestActionString.ToLower() == "startflow")  //启动流程
                    {
                        OA_PublicInfo ne = OA_PublicInfo.Find(this.RequestData["Id"].ToString());   //启动流程
                        
                        string formUrl = "/Modules/PubNews/NewsEdit.aspx?op=u&&Id=" + ne.Id;    //表单路径,后面加上参数传入
                        // PIC.WorkFlow.WorkFlow.StartWorkFlow(ne.Id, formUrl, ne.Title, key, this.UserInfo.UserID, this.UserInfo.Name);
                        PageState.Add("message", "启动成功");
                        return;
                    }
                    else if (this.RequestActionString.ToLower() == "submit")
                    {
                        // DoBatchExec();

                        OA_PublicInfo pi = OA_PublicInfo.Find(this.RequestData["Id"].ToString());
                        string status = this.RequestData.Get<string>("status");

                        switch (status)
                        {
                            case "Published":
                                pi.DoPublish();
                                PageState.Add("message", "发布成功");
                                break;
                            case "Recalled":
                                pi.DoRecall();
                                PageState.Add("message", "撤销成功");
                                break;
                            case "UnExpired":
                                pi.DoPublish();
                                PageState.Add("message", "撤销成功");
                                break;
                            default:
                                pi.Status = status;
                                pi.DoSave();
                                PageState.Add("message", "设置成功");
                                break;
                        }

                        return;
                    }
                    else
                    {
                        DoSelect();
                    }
                    break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("BooleanEnum", Enumeration.GetEnumDict("Common.Data.Boolean"));
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Type"));
                PageState.Add("GradeEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Grade"));
                PageState.Add("StatusEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Status"));
            }
            
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
            string type = RequestData.Get<string>("type", "None");

            if (StringHelper.IsEqualsIgnoreCase("r", op))
            {
                SearchCriterion.SetSearch(OA_PublicInfo.Prop_Status, "Published");
            }

            if (!StringHelper.IsEqualsIgnoreCase("All", type))
            {
                SearchCriterion.SetSearch(OA_PublicInfo.Prop_Type, type, SearchModeEnum.Equal);
            }

            ents = OA_PublicInfo.FindAll(SearchCriterion);
            this.PageState.Add("OA_PublicInfoList", ents);
		}

        /// <summary>
        /// 批量处理
        /// </summary>
        [ActiveRecordTransaction]
        private void DoBatchExec()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");
            string status = this.RequestData.Get<string>("status");

            if (idList != null && idList.Count > 0)
            {
                idList.All((id) =>
                {
                    OA_PublicInfo pi = OA_PublicInfo.Find(id);

                    pi.PublishDate = DateTime.Now;
                    pi.Status = status;

                    pi.Save();

                    return true;
                });

                if (status == "Published")
                {
                    PageState.Add("message", "发布成功");
                }
                else
                {
                    PageState.Add("message", "撤销成功");
                }
            }

        }
		
		/// <summary>
        /// 批量删除
        /// </summary>
		[ActiveRecordTransaction]
		private void DoBatchDelete()
		{
			IList<object> idList = RequestData.GetList<object>("IdList");

			if (idList != null && idList.Count > 0)
			{                   
				OA_PublicInfo.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

