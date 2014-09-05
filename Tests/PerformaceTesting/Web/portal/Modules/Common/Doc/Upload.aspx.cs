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
using PIC.Doc;
using PIC.Doc.Model;

namespace PIC.Portal.Web.Modules.Common.Doc
{
    public partial class Upload : BasePage
    {
        #region 私有方法

        private Guid? dirid = null;
        private Guid? grpid = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            dirid = RequestData.GetGuid("dirid", RequestData.GetGuid("did"));
            grpid = RequestData.GetGuid("grpid", RequestData.GetGuid("gid"));

            switch (RequestActionString)
            {
                case "upload":
                    DoUpload();
                    break;
                default:
                    DoSelect();
                    break;
            }

            if (!IsAsyncRequest)
            {
                // 返回文件目录信息
                if (!dirid.IsNullOrEmpty())
                {
                    var docDir = DocDirectory.Find(dirid);

                    PageState.Add("DocDir", docDir);
                }
            }
        }

        #endregion

        #region 私有方法

        private void DoUpload()
        {
            if (this.Request.Files.Count > 0)
            {
                if (!dirid.IsNullOrEmpty())
                {
                    var docDir = DocDirectory.Find(dirid);

                    if (docDir != null)
                    {
                        var fileList = DocService.Save(this.Request.Files, docDir);

                        PageState.Add("FileList", fileList);
                    }
                }
                else
                {
                    var fileList = DocService.SaveTemp(this.Request.Files, grpid);

                    PageState.Add("TempFileList", fileList);
                }

                Response.Write(JsonHelper.GetJsonString(PageState));
                Response.End();
            }

            // 等1秒再开始下一次上传，确保保存成功
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void DoSelect()
        {
        }

        #endregion
    }
}
