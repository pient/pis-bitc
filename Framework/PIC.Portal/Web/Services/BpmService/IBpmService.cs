using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Services
{
    [ServiceContract]
    public interface IBpmService : IServiceBase
    {
        /// <summary>
        /// 运行服务
        /// </summary>
        /// <param name="opReq"></param>
        /// <returns></returns>
        [OperationContract]
        OperationResult RunService(object opReq);

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="opReq"></param>
        /// <returns></returns>
        [OperationContract]
        OperationResult ExecCommand(object opReq);
    }
}