using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Biz.Model;

namespace PIC.Biz.Web
{
    public partial class HR_EmployeeList : BizListPage
    {
        #region 变量

        private IList<HR_Employee> ents = null;

        #endregion

        #region 构造函数

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
			HR_Employee ent = null;
            switch (this.RequestAction)
            {
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<HR_Employee>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    break;
                default:
                    if (RequestActionString == "batchdelete")
                    {
						DoBatchDelete();
                    }
                    else if (RequestActionString == "sync")
                    {
                        DoSync();
                    }
                    else if (RequestActionString == "import")
                    {
                        DoImport();
                    }
                    else 
                    {
						DoSelect();
					}
					break;
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("BizHR.Employee.Type"));
                PageState.Add("MajorEnum", Enumeration.GetEnumDict("BizProject.Major.Type"));
                PageState.Add("RegionEnum", Enumeration.GetEnumDict("BizHR.Region"));
                PageState.Add("PeopleEnum", Enumeration.GetEnumDict("HumanGeo.People"));
                PageState.Add("SexEnum", Enumeration.GetEnumDict("HumanGeo.Sex"));
                PageState.Add("AttendanceTypeEnum", Enumeration.GetEnumDict("BizHR.Employee.Attendance.Type"));
                PageState.Add("MarriageStatusEnum", Enumeration.GetEnumDict("HumanGeo.Marriage.Status"));
                PageState.Add("PoliticalStatusEnum", Enumeration.GetEnumDict("HumanGeo.Political.Status"));
                PageState.Add("BooleanEnum", Enumeration.GetEnumDict("Common.Data.Boolean"));
                PageState.Add("PostQualEnum", Enumeration.GetEnumDict("BizHR.Employee.Basic.PostQual"));
                PageState.Add("InnerTitleEnum", Enumeration.GetEnumDict("BizHR.Employee.Basic.InnerTitle"));
                PageState.Add("EducationEnum", Enumeration.GetEnumDict("Common.Education"));
            }
        }

        #endregion

        #region 私有方法
		
		/// <summary>
        /// 查询
        /// </summary>
		private void DoSelect()
		{
			ents = HR_Employee.FindAll(SearchCriterion);
			this.PageState.Add("HR_EmployeeList", ents);
		}

        /// <summary>
        /// 同步数据
        /// </summary>
        private void DoSync()
        {
            IList<object> idList = RequestData.GetList<object>("IdList");

            if (idList != null && idList.Count > 0)
            {
                HR_Employee.DoBatchSync(idList.ToArray());
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
				HR_Employee.DoBatchDelete(idList.ToArray());
			}
		}

        /// <summary>
        /// 导入数据
        /// </summary>
        [ActiveRecordTransaction]
        private void DoImport()
        {
            string ffid = RequestData.Get<string>("ffid");
            string code = RequestData.Get<string>("code");    // 模板编码

            try
            {
                var ent = DataImportTemplate.Get(code);

                DataImportService.ImportData(ent.Config, ffid, delegate(DataTable dt)
                {
                    return true;
                }, null);
            }
            finally
            {
                // 导入完成后删除原文件
                FileService.DeleteFileByFullID(ffid);
            }
        }

        #endregion
    }
}

