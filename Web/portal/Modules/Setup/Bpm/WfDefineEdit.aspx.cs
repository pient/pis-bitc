using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PIC.Common;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;
using PIC.Portal.Workflow;

namespace PIC.Portal.Web.Modules.Setup.Bpm
{
    public partial class WfDefineEdit : BaseFlowPage
    {
        string op = String.Empty;
        string id = String.Empty;   // 对象id

        protected void Page_Load(object sender, EventArgs e)
        {
            op = RequestData.Get<string>("op"); // 用户编辑操作
            id = RequestData.Get<string>("id");

            WfDefine ent = null;

            if (IsAsyncRequest)
            {
                switch (RequestAction)
                {
                    case RequestActionEnum.Create:
                    case RequestActionEnum.Update:
                        DoSave();
                        break;
                    case RequestActionEnum.Delete:
                        DoDelete();
                        break;
                    default:
                        switch (RequestActionString)
                        {
                            case "copy":
                                DoCopy();
                                break;
                        }
                        break;
                }
            }

            if (op != "c")
            {
                if (!String.IsNullOrEmpty(id))
                {
                    ent = WfDefine.Find(id);

                    this.SetFormData(ent);

                    // JsonHelper不序列化FormDefine，这里需要单独赋值
                    PageState.Add("FormDefine", ent.FormDefine);
                    PageState.Add("DiagramPath", ent.GetConfig().FlowDiagramPath);
                    PageState.Add("HasInstance", ent.HasInstance());
                }
            }

            if (!IsAsyncRequest)
            {
                this.PageState.Add("WfCatalogEnum", Enumeration.GetEnumDict("SysMag.Workflow.Catalog"));
            }
        }

        #region 支持方法

        /// <summary>
        /// 保存数据
        /// </summary>
        private void DoSave()
        {
            var def = this.GetMergedData<WfDefine>();

            def.FormDefine = RequestData.Get<string>("formDefine");

            def.DoSave();
        }

        /// <summary>
        /// 复制流程
        /// </summary>
        private void DoCopy()
        {
            var def = this.GetPostedData<WfDefine>();

            def.FormDefine = RequestData.Get<string>("formDefine");

            if (!String.IsNullOrEmpty(id))
            {
                var srcDef = WfDefine.Find(id);
                var srcCfg = srcDef.GetConfig();

                var targetCfg = srcDef.GetConfig();

                if (srcCfg != null && !String.IsNullOrEmpty(srcCfg.FlowPath))
                {
                    var srcFlowFilePath = WfHelper.GetFlowFilePath(srcCfg.FlowPath);

                    if (System.IO.File.Exists(srcFlowFilePath))
                    {
                        string srcName = "";
                        string srcPath = "";
                        string srcExName = ".xaml";
                        string targetName = "";

                        if (String.IsNullOrEmpty(def.Code))
                        {
                            def.Code = Model.Template.Get("Sys.Code.Data.WfDefineCode").RenderString();
                        }

                        // 复制相关流程文件
                        srcName = System.IO.Path.GetFileNameWithoutExtension(srcCfg.FlowPath);
                        srcExName = System.IO.Path.GetExtension(srcCfg.FlowPath);
                        srcPath = srcCfg.FlowPath.TrimEnd((srcName + srcExName));

                        if (targetCfg != null)
                        {
                            targetName = System.IO.Path.GetFileNameWithoutExtension(targetCfg.FlowPath);
                        }

                        if (srcName == targetName || String.IsNullOrEmpty(targetName))
                        {
                            targetName = def.Code;
                        }

                        def.DefineConfig.FlowPath = System.IO.Path.Combine(srcPath, targetName + srcExName);

                        var targetFlowFilePath = WfHelper.GetFlowFilePath(def.DefineConfig.FlowPath);

                        if (System.IO.File.Exists(srcFlowFilePath) && !System.IO.File.Exists(targetFlowFilePath))
                        {
                            System.IO.File.Copy(srcFlowFilePath, targetFlowFilePath);
                        }

                        var srcFlowDiagramFilePath = WfHelper.GetFlowFilePath(srcCfg.FlowDiagramPath);

                        if (System.IO.File.Exists(srcFlowDiagramFilePath))
                        {
                            var diagramExName = System.IO.Path.GetExtension(srcCfg.FlowDiagramPath);
                            def.DefineConfig.FlowDiagramPath = System.IO.Path.Combine(srcPath, targetName + diagramExName);

                            var targetFlowDiagramFilePath = WfHelper.GetFlowFilePath(def.DefineConfig.FlowDiagramPath);

                            if (!System.IO.File.Exists(targetFlowDiagramFilePath))
                            {
                                System.IO.File.Copy(srcFlowDiagramFilePath, targetFlowDiagramFilePath);
                            }
                        }
                    }
                }

                def.DoCreate();
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void DoDelete()
        {
            var def = this.GetTargetData<WfDefine>();
            def.DoDelete();
        }

        #endregion
    }
}
