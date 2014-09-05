using System;
using System.Reflection;
using System.ComponentModel;
using System.Activities.Presentation.PropertyEditing;


namespace PIC.Portal.Workflow
{
    public class FlowRouteCollectionEditor : DialogPropertyValueEditor
    {
        #region 构造函数

        #endregion

        #region 重载

        public override void ShowDialog(PropertyValue propertyValue, System.Windows.IInputElement commandSource)
        {
            base.ShowDialog(propertyValue, commandSource);
        }

        #endregion
    }
}
