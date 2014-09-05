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
    public interface IScheduleService
    {
        [OperationContract]
        TaskActionResult RunTask(string id);

        [OperationContract]
        TaskActionResult RunSchedule(string id);

        [OperationContract]
        void Start();

        [OperationContract]
        void Stop();

        [OperationContract]
        void Reset();
    }
}