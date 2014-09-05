using PIC.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 提交流程时的基本信息
    /// </summary>
    public class FlowBasicInfo
    {
        #region Properties

        public string Title { get; set; }
        public string InstanceID { get; set; }
        public string ModelID { get; set; }
        public string FlowObjectType { get; set; }
        public string ActionTitleFormat { get; set; }
        public string SN { get; set; }
        public string KeyWords { get; set; }
        public string DefineCode { get; set; }
        public string DefineName { get; set; }
        public string Template { get; set; }
        public string ApplicantID { get; set; }
        public string ApplicantName { get; set; }
        public string DeptID { get; set; }
        public string DeptName { get; set; }
        public string Status { get; set; }
        public string FormPath { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? StartedTime { get; set; }
        public EasyDictionary Tag { get; set; }

        #endregion

        #region Constructors

        public FlowBasicInfo()
        {
            // NVelocity表达式
            this.ActionTitleFormat = "${FlowState.Title} - ${TaskState.ActionTitle}";
        }

        #endregion
    }
}
