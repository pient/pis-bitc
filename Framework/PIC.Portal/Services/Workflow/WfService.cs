using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Common;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    public class WfService
    {
        #region Consts

        public const string BPM_SERVICE_PROVIDER_KEY = "BpmServiceProvider";

        #endregion

        #region 成员

        protected IBpmServiceProvider _provider = null;

        #endregion

        #region 构造函数

        protected readonly static WfService ws = new WfService();

        protected WfService()
        {
            string typeName = PICConfigurationManager.AppSettings[BPM_SERVICE_PROVIDER_KEY];

            _provider = CLRHelper.CreateInstance<IBpmServiceProvider>(typeName);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="formData"></param>
        /// <param name="actionInfo"></param>
        public static OperationResult Run(FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            FlowRequest fReq = new FlowRequest(basicInfo, formData, actionInfo);

            var opResult = ws._provider.RunService(fReq);

            return opResult;
        }

        public static OperationResult Exec(string cmdName, string insId, EasyDictionary args = null)
        {
            var opResult = ws._provider.ExecCommand(cmdName, insId, args);

            return opResult;
        }

        /// <summary>
        /// 保存服务实例
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="formData"></param>
        /// <param name="actionInfo"></param>
        /// <returns></returns>
        public static WfInstance SaveWfInstance(FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
        {
            var ins = WfServer.SaveWfInstance(basicInfo, formData, actionInfo);

            return ins;
        }

        #endregion
    }
}
