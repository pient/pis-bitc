using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace PIC.Portal.Web.Services
{
    /// <summary>
    /// 数据服务
    /// </summary>
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        string QueryData(string msg);
    }
}