using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;
using PIC.Portal.Web;
using PIC.Portal.Web.UI;
using PIC.Biz.Model;

namespace PIC.Biz.Web
{
    public partial class HR_EmployeeEdit : BizFlowFormPage
    {
        #region 变量

        string op = String.Empty; // 用户编辑操作
        string id = String.Empty;   // 对象id
        string type = String.Empty; // 对象类型
        string origdeptid = String.Empty;   // 原部门

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op");
            id = RequestData.Get<string>("id", RequestData.Get<string>("eid"));
            type = RequestData.Get<string>("type");

            HR_Employee ent = null;

            switch (this.RequestAction)
            {
                case RequestActionEnum.Update:
                    origdeptid = RequestData.Get<string>("origdeptid");

                    ent = this.GetMergedData<HR_Employee>();
                    ent.DoUpdateAndSync();

                    if (!String.IsNullOrEmpty(origdeptid) && !StringHelper.IsEqualsIgnoreCase(origdeptid, ent.DepartmentId))
                    {
                        var grp = OrgGroup.Find(origdeptid);
                        // grp.RemoveUser(ent.GetSysUser());
                    }

                    this.SetMessage("修改成功！");
                    break;
                case RequestActionEnum.Create:
                    ent = this.GetPostedData<HR_Employee>();

                    ent.DoCreateAndSync();
                    this.SetMessage("新建成功！");
                    break;
                case RequestActionEnum.Delete:
                    ent = this.GetTargetData<HR_Employee>();
                    ent.DoDelete();
                    this.SetMessage("删除成功！");
                    return;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = HR_Employee.Find(id);
                }
                
                this.SetFormData(ent);
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("BizHR.Employee.Type"));
                PageState.Add("StatusEnum", Enumeration.GetEnumDict("BizHR.Employee.Status"));
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
    }
}

