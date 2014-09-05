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
    public partial class OA_PublicInfoEdit : BizFlowFormPage
    {
        #region 变量

        string id = String.Empty;
        string type = String.Empty;
        string op = String.Empty;
        string optype = String.Empty;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            id = RequestData.Get<string>("id");
            type = RequestData.Get<string>("type");
            op = RequestData.Get<string>("op");
            optype = RequestData.Get<string>("optype");

            OA_PublicInfo ent = null;

            switch (this.RequestAction)
            {
                default:
                    if (RequestAction == RequestActionEnum.Update)
                    {
                        ent = this.GetMergedData<OA_PublicInfo>();

                        if (optype == "publish")
                        {
                            ent.DoPublish();
                            this.SetMessage("发布成功！");
                        }
                        else
                        {
                            ent.DoUpdate();
                            this.SetMessage("修改成功！");
                        }
                    }
                    else if (RequestAction == RequestActionEnum.Create)
                    {
                        ent = this.GetPostedData<OA_PublicInfo>();

                        if (optype == "publish")
                        {
                            ent.DoPublish();
                            this.SetMessage("发布成功！");
                        }
                        else
                        {
                            ent.DoCreate();
                            this.SetMessage("新建成功！");
                        }
                    }
                    break;
            }

            if (op != "c" && op != "cs")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = OA_PublicInfo.Find(id);
                }

                this.SetFormData(ent);
            }

            if (!IsAsyncRequest)
            {
                PageState.Add("BooleanEnum", Enumeration.GetEnumDict("Common.Data.Boolean"));
                PageState.Add("TypeEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Type"));
                PageState.Add("GradeEnum", Enumeration.GetEnumDict("SysUtil.PublicInfo.Grade"));
            }
        }

        #endregion
    }
}

