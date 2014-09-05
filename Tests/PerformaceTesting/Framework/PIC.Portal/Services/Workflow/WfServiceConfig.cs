using PIC.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Workflow
{
    public class WfServiceConfig
    {
        #region Singleton

        private static WfServiceConfig _instance;

        public static WfServiceConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WfServiceConfig();
                }

                return _instance;
            }
        }

        #endregion

        #region Constructors

        private WfServiceConfig() { }

        #endregion

        #region Properties

        private static string _flowFileFolder = null;

        /// <summary>
        /// 流程文件目录
        /// </summary>
        public static string FlowFileFolder
        {
            get
            {
                if (String.IsNullOrEmpty(_flowFileFolder))
                {
                    var p = Model.Parameter.Get(FlowFileFolderCode);

                    if (p != null)
                    {
                        _flowFileFolder = Path.Combine(PortalService.ResourceFolder, p.Value);
                    }
                }

                return _flowFileFolder;
            }
        }

        /// <summary>
        /// 默认审批人配置编码
        /// </summary>
        public static string BpmDefActorsCode
        {
            get { return "System.Bpm.DefActors"; }
        }

        /// <summary>
        /// 流程图目录配置编码
        /// </summary>
        public static string FlowFileFolderCode
        {
            get { return "System.Bpm.FlowFileFolder"; }
        }

        #endregion
    }
}
