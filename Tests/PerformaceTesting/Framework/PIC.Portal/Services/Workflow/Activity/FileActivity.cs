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
    /// <summary>
    /// 归档
    /// </summary>
    [Designer(typeof(TaskActivityDesigner))]
    public class FileActivity : TaskActivity
    {
        #region 构造函数

        static FileActivity()
        {
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
