using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using PIC.Common;

namespace PIC
{
    /// <summary>
    /// 操作请求，一般在分布式调用时使用
    /// </summary>
    [Serializable]
    [DataContract]
    public class OperationRequest
    {
        #region 属性

        /// <summary>
        /// 用户信息
        /// </summary>
        [DataMember]
        public string SessionID { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [DataMember]
        public EasyDictionary Params { get; set; }

        #endregion

        #region 构造函数

        public OperationRequest()
        {
            Params = new EasyDictionary();
        }

        public OperationRequest(string sessionID)
            : this()
        {
            this.SessionID = sessionID;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            var jsonStr = JsonHelper.GetJsonString(this);

            return jsonStr;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 由Json字符串获取对象
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static OperationRequest FromObject(object obj)
        {
            OperationRequest req = null;

            if (obj != null)
            {
                if (obj is OperationRequest)
                {
                    req = obj as OperationRequest;
                }
                else if (obj is string)
                {
                    req = JsonHelper.GetObject<OperationRequest>(obj.ToString());
                }
            }

            return req;
        }

        #endregion
    }
}
