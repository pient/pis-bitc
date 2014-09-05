using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using PIC.Common;
using PIC.Portal;

namespace PIC.Portal.Workflow
{
    public class PortalActivity : NativeActivity
    {
        #region 成员属性

        [Browsable(false)]
        public InOutArgument<PortalState> PortalState { get; set; }

        #endregion

        #region 重载方法

        protected override void Abort(NativeActivityAbortContext context)
        {
            LogService.Log(context.Reason, LogService.WorkflowException);

            base.Abort(context);
        }

        protected override void Execute(NativeActivityContext context)
        {
            
        }

        /// <summary>
        /// 默认导致之久化
        /// </summary>
        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}
