using PIC.Common;
using PIC.Portal.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Model
{
    [Serializable]
    public class WfDocTempalte
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public bool IsCheckAuth { get; set; }
        public EasyDictionary Tag { get; set; }

        public WfDocTempalte()
        {
            Tag = new EasyDictionary();
        }

        /// <summary>
        /// 获取模板文件流
        /// </summary>
        /// <returns></returns>
        public FileStream RetrieveFileStream()
        {
            string tmplDocTmplPath = WfHelper.GetFlowFilePath(this.Path);

            FileStream tmplStream = null;
            if (File.Exists(tmplDocTmplPath))
            {
                tmplStream = File.Open(tmplDocTmplPath, FileMode.Open, FileAccess.Read);
            }

            return tmplStream;
        }
    }

    [Serializable]
    public class WfDefineConfig
    {
        #region 属性

        public string FlowType { get; set; }

        public string FlowPath { get; set; }

        public string FlowDiagramPath { get; set; }

        public string FormPath { get; set; }

        public string FormVersion { get; set; }

        public EasyDictionary FormData { get; set; }    // 默认表单数据

        public EasyCollection<WfDocTempalte> DocTemplates { get; set; }    // 文档模版

        public EasyDictionary Tag { get; set; }

        #endregion

        #region 构造函数

        public WfDefineConfig()
        {
            FormVersion = "1.0";
            FormData = new EasyDictionary();
            Tag = new EasyDictionary();
        }

        #endregion

        #region 公共方法

        public Activity NewFlowObject()
        {
            Activity obj = null;

            if (!String.IsNullOrEmpty(this.FlowType))
            {
                obj = CLRHelper.CreateInstance<Activity>(this.FlowType);
            }
            else if (!String.IsNullOrEmpty(this.FlowPath))
            {
                var fullPath = Path.Combine(WfServiceConfig.FlowFileFolder, this.FlowPath);

                if (File.Exists(fullPath))
                {
                    obj = WfHelper.LoadActivity(fullPath);
                }
            }

            return obj;
        }

        #endregion
    }

    /// <summary>
    /// 表单定义信息
    /// </summary>
    [Serializable]
    public class WfFormDefine
    {
        #region 配置信息内容

        public string Config { get; set; }

        #endregion

        #region 构造函数

        public WfFormDefine()
        {
        }

        #endregion
    }
}
