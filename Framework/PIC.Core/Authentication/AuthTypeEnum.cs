using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Common.Authentication
{
    /// <summary>
    /// 用户权限类型
    /// </summary>
    public enum AuthTypeEnum
    {
        Application = 1,
        Module = 2,
        Portal = 3,
        Page = 4,
        FieldRead = 5,
        FieldModify = 6,
        Other
    }

    /// <summary>
    /// 用户登陆方式枚举
    /// </summary>
    public enum LoginTypeEnum
    {
        PCClient,   // 客户端机器登陆
        PCIE,   // 客户端IE登陆
        Unknown // 未知
    }

    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStateEnum
    {
        Online, // 在线
        Offline,    // 离线
        Logoff, // 已登出
        Login,  // 已登入
        Unknown // 未知
    }
}
