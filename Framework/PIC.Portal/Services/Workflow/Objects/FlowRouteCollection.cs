using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.Drawing.Design;

namespace PIC.Portal.Workflow
{
    [CollectionDataContract]
    public class FlowRouteCollection : Collection<FlowRoute>
    {
        #region 构造函数

        public FlowRouteCollection() : base()
        {
        }

        #endregion
    }
}
