using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PIC.Portal.Workflow
{
    [Serializable]
    public class ActionState
    {
        #region 成员

        protected ActionRequest request;

        #endregion

        #region 构造函数

        public ActionState(ActionRequest request)
        {
            this.request = request;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 活动请求
        /// </summary>
        public ActionRequest Request
        {
            get { return this.request; }
        }

        #endregion
    }
}
