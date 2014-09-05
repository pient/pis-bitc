using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIC.Portal.Workflow;
using PIC.Portal.ServicesProvider.BpmService;

namespace PIC.Portal.ServicesProvider
{
    public class BpmServiceProvider : IBpmServiceProvider
    {
        #region 成员

        private IBpmService bsc = null;

        #endregion

        #region 构造函数

        public BpmServiceProvider()
        {
            bsc = new BpmServiceClient();
        }

        #endregion

        /// <summary>
        /// 执行流程服务
        /// </summary>
        /// <param name="flowReq"></param>
        /// <returns></returns>
        public OperationResult RunService(FlowRequest flowReq)
        {
            OperationResult opResult = null;

            try
            {
                OperationRequest opReq = PortalService.NewOperationRequest();

                if (flowReq != null )
                {
                    opReq.Params.Set("FlowRequest", flowReq);
                }

                opResult = bsc.RunService(opReq.ToJsonString());
            }
            catch (Exception ex)
            {
                opResult = new OperationResult(ex);
            }

            return opResult;
        }

        /// <summary>
        /// 执行流程命令(Terminate)
        /// </summary>
        /// <returns></returns>
        public OperationResult ExecCommand(string cmdName, string insId, EasyDictionary args = null)
        {
            OperationResult opResult = null;

            try
            {
                OperationRequest opReq = PortalService.NewOperationRequest();
                
                opReq.Params.Set("CommandName", cmdName);
                opReq.Params.Set("InstanceID", insId);
                opReq.Params.Set("Arguments", args);

                opResult = bsc.ExecCommand(opReq.ToJsonString());
            }
            catch (Exception ex)
            {
                opResult = new OperationResult(ex);
            }

            return opResult;
        }
    }
}
