using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 流程信息（一般用于将流程信息传给Bus页面）
    /// </summary>
    [CollectionDataContract]
    public class FlowFormData : EasyDictionary
    {
        #region 构造函数 

        public FlowFormData()
        {
        }

        public FlowFormData(IDictionary<string, Object> innerDictionary)
            : base(innerDictionary)
        {
        }

        #endregion

        #region 成员属性



        #endregion

    }
}
