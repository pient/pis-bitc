using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using Microsoft.Synchronization.MetadataStorage;
using Microsoft.Windows.Design.Metadata;
using NHibernate.Criterion;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    [Designer(typeof(TaskActivityDesigner))]
    public class EndActivity : TaskActivity
    {
        #region 构造函数

        static EndActivity()
        {
        }

        public EndActivity()
        {
            this.Type = WfTask.TypeEnum.Immediate;
            this.TaskState.NoIdle = true;
        }

        #endregion

        #region 属性

        #endregion

        #region 重载方法

        protected override void Execute(NativeActivityContext context)
        {
            base.Execute(context);
        }

        #endregion
    }
}
