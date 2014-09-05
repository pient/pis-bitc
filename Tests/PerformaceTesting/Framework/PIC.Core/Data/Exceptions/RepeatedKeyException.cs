using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PIC.Data
{
    /// <summary>
    /// 重复键
    /// </summary>
    public class RepeatedKeyException : Exception
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public RepeatedKeyException()
            : base("存在重复键")
        {

        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        public RepeatedKeyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="message">异常的消息</param>
        /// <param name="inner">内部的异常</param>
        public RepeatedKeyException(string message, System.Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="info">存有有关所引发异常的序列化的对象数据</param>
        /// <param name="context">包含有关源或目标的上下文信息</param>
        protected RepeatedKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        #endregion
    }
}
