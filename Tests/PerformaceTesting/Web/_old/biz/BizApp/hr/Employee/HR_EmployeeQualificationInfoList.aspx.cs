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
    public partial class HR_EmployeeQualificationInfoList : BizListPage
    {
        #region 变量

        private IList<HR_EmployeeQualificationInfo> ents = null;

        string qtype = String.Empty;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            qtype = RequestData.Get<string>("qtype");

			HR_EmployeeQualificationInfo ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<HR_EmployeeQualificationInfo>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                    
                default:
                    if (RequestActionString == "batchdelete")
                    {
						DoBatchDelete();
                    }
                    else if (RequestActionString == "save")
                    {
                        DoSave();
                    }
                    else
                    {
                        DoSelect();
                    }
					break;
            }

            if (!IsAsyncRequest)
            {
                if (!String.IsNullOrEmpty(qtype))
                {
                    PageState.Add("SexEnum", Enumeration.GetEnumDict("HumanGeo.Sex"));
                    PageState.Add("EmployeeTypeEnum", Enumeration.GetEnumDict("BizHR.Employee.Type"));
                    PageState.Add("TypeEnum", Enumeration.GetByValue("BizHR.Employee.Qualification.Type", qtype).GetDict());
                }
            }
            
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
			ents = HR_EmployeeQualificationInfo.FindAll(SearchCriterion);
			this.PageState.Add("HR_EmployeeQualificationInfoList", ents);
		}

        [ActiveRecordTransaction]
        private void DoSave()
        {
            IList<string> entStrList = RequestData.GetList<string>("data");

            foreach (string entStr in entStrList)
            {
                HR_EmployeeQualificationInfo tent = JsonHelper.GetObject<HR_EmployeeQualificationInfo>(entStr) as HR_EmployeeQualificationInfo;

                HR_EmployeeQualification.DoSave(tent);
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
				HR_EmployeeQualificationInfo.DoBatchDelete(idList.ToArray());
			}
		}

        #endregion
    }
}

