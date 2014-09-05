using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 用户信息相关，都应该实现此接口，并可获取最小用户信息
    /// </summary>
    public interface IUserInfo
    {
        MinUserInfo GetMinUserInfo();
    }

    /// <summary>
    /// 简化版用户信息，一般用于流程审批等过程的人员记录
    /// </summary>
    public class MinUserInfo
    {
        public string UserID { get; set; }
        public string Name { get; set; }
    }
}
