using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Activities;
using Newtonsoft.Json;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    [DataContract]
    public class FlowRequest : BaseFlowRequest
    {
        #region 构造函数

        protected FlowRequest()
        {
        }

        public FlowRequest(string code)
            : base(code)
        {
        }

        public FlowRequest(FlowBasicInfo basicInfo, FlowFormData formData = null, FlowActionInfo actionInfo = null)
            : base(basicInfo, formData, actionInfo)
        {
        }

        #endregion
    }
}
