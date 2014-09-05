using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 验证结果
    /// </summary>
    [Serializable]
    [DataContract]
    public class ValidationResult
    {
        #region 属性

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public object Tag { get; set; }

        [DataMember]
        public IList<string> Messages { get; protected set; }

        #endregion

        #region 构造函数

        public ValidationResult()
            : this(true)
        {
        }

        public ValidationResult(String message)
            : this(false)
        {
            this.Messages.Add(message);
        }

        public ValidationResult(bool success)
        {
            this.Messages = new List<String>();

            this.Success = success;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 增加错误消息
        /// </summary>
        public void AddErrorMessage(String message)
        {
            this.Success = false;
            this.Messages.Add(message);
        }

        public override string ToString()
        {
            string msg = "";

            if (this.Success)
            {
                msg += "验证成功。";
            }
            else
            {
                msg += "验证失败。";
            }

            if (this.Messages != null && this.Messages.Count > 0)
            {
                foreach (String t_msg in this.Messages)
                {
                    msg += t_msg;
                }
            }

            return base.ToString();
        }

        #endregion
    }
}
