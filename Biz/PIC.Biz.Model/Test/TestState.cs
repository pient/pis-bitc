using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Portal;
using PIC.Portal.Workflow;

namespace PIC.Biz.Model.Test
{
    [Serializable]
    public class TestState
    {
        #region 成员

        [NonSerialized]
        protected FlowRequest request;

        [NonSerialized]
        protected TaskState taskstate;

        #endregion

        #region 构造函数

        public TestState(FlowRequest request)
        {
            this.request = request;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 实例id
        /// </summary>
        public FlowRequest Request
        {
            get { return this.request; }
        }

        /// <summary>
        /// 定义编码
        /// </summary>
        public string DefineCode
        {
            get { return request.DefineCode; }
        }

        /// <summary>
        /// 实例id
        /// </summary>
        public string InstanceID
        {
            get { return request.InstanceID; }
        }

        /// <summary>
        /// 流程表单地址
        /// </summary>
        public string FormUrl
        {
            get { return request.FormUrl; }
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState Current
        {
            get { return this.taskstate; }
            set { this.taskstate = value; }
        }

        #endregion
    }
}
