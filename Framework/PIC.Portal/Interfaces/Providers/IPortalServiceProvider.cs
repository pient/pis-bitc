using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using NServiceBus;
using PIC.Common;
using PIC.Common.Authentication;

namespace PIC.Portal
{
    /// <summary>
    /// 登录Session状态
    /// </summary>
    public enum UserSessionState
    {
        Valid,  // 有效
        None,   // 不存在
        Timeout,    // 超时
        Released,   // 已释放
        Faulted,    // 已出错
        Unknown     // 未知
    }

    /// <summary>
    /// 入口服务支持类
    /// </summary>
    public interface IPortalServiceProvider
    {
        /// <summary>
        /// 获取NServiceBus
        /// </summary>
        /// <returns></returns>
        IServiceBus RetrieveServiceBus();

        /// <summary>
        /// 获取当前用户令牌
        /// </summary>
        /// <returns></returns>
        SysIdentity GetCurrentSysIdentity();

        /// <summary>
        /// 设置当前用户访问信息
        /// </summary>
        /// <param name="userInfo"></param>
        void SetCurrentPrincipal(string sessionID);

        /// <summary>
        /// 由登录sessionID胡偶去用户信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        UserInfo GetUserInfo(string sessionID);

        /// <summary>
        /// 获取当前登录状态
        /// </summary>
        /// <returns></returns>
        UserSessionState GetUserSessionState(string sessionID);

        /// <summary>
        /// 刷新登录Session
        /// </summary>
        /// <param name="sessionID"></param>
        void RefreshSession(string sessionID);

        /// <summary>
        /// 释放登录Session
        /// </summary>
        /// <param name="sessionID"></param>
        void ReleaseSession(string sessionID);

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="authMessage">登录信息</param>
        /// <returns>登录sessionId</returns>
        string AuthenticateUser(IAuthPackage authPackage);

        /// <summary>
        /// 注销当前登录
        /// </summary>
        void Logout(string sessionID);

        /// <summary>
        /// 获取在线用户数
        /// </summary>
        /// <returns></returns>
        int GetOnlineUserCount();
    }
}
