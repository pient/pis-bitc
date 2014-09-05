using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace PIC.Common.Service
{
    /// <summary>
    /// WCF服务接口
    /// </summary>
    [ServiceContract]
    public interface IWCFService
    {
        /// <summary>
        /// 执行服务操作
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [OperationContract]
        byte[] ExecuteService(string msg);
    }
}
