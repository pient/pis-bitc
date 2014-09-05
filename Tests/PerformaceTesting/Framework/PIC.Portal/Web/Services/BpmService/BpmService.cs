using PIC.Portal.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Web.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BpmService : IBpmService
    {
        #region IBpmService成员

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="reqObject"></param>
        /// <returns></returns>
        public OperationResult RunService(object opReq)
        {
            OperationResult opResult = new OperationResult();

            try
            {
                OperationRequest req = OperationRequest.FromObject(opReq);

                if (AuthService.VerifyOperationRequest(req) == true)
                {
                    FlowRequest fReq = req.Params.Get<FlowRequest>("FlowRequest");

                    WfServer.Run(fReq);
                }
            }
            catch (Exception ex)
            {
                opResult = new OperationResult(ex);
            }

            return opResult;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <returns></returns>
        public OperationResult ExecCommand(object opReq)
        {
            OperationResult opResult = new OperationResult();

            try
            {
                OperationRequest req = OperationRequest.FromObject(opReq);

                if (AuthService.VerifyOperationRequest(req) == true)
                {
                    var insId = req.Params.Get<string>("InstanceID");
                    var cmdName = req.Params.Get<string>("CommandName");
                    var args = req.Params.Get<EasyDictionary>("Arguments");

                    WfServer.Exec(cmdName, insId, args);
                }
            }
            catch (Exception ex)
            {
                opResult = new OperationResult(ex);
            }

            return opResult;
        }

        #endregion
    }
}
