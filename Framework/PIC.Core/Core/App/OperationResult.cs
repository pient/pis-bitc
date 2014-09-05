using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 操作结果，一般在分布式调用时使用
    /// </summary>
    [Serializable]
    [DataContract]
    public class OperationResult
    {
        #region 属性

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public object Tag { get; set; }

        #endregion

        #region 构造函数

        public OperationResult()
            : this(true)
        {
        }

        public OperationResult(Exception ex)
            : this(false, ex.Message)
        {
        }

        public OperationResult(bool success, string message = null, string code = null)
        {
            this.Success = success;
            this.Message = message;
            this.Code = code;
        }

        #endregion
    }
}
