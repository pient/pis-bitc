using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Castle.ActiveRecord;
using PIC.Security;
using PIC.Common;
using PIC.Common.Authentication;
using PIC.Common.Service;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Services
{
    /// <summary>
    /// 用户Session服务实现
    /// </summary>
    public class UserSessionService : IUserSessionService
    {
        /// <summary>
        /// 用户Session服务提供对象
        /// </summary>
        public static UserSessionServer Server = UserSessionServer.Instance;

        static UserSessionService()
        {
        }

        #region IUserSessionService Members

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        public string AuthenticateUser(string authMsg)
        {
            try
            {
                return Server.AuthenticateUser(authMsg);
            }
            catch (Exception ex)
            {
                return String.Format("ex:{0}", ex.Message);
            }
        }

        /// <summary>
        /// 判断用户Session状态(判断用户是否已注销)
        /// </summary>
        /// <param name="sessionID">此用户的UserSession标识</param>
        /// <returns>true-有效  false-无效</returns>
        public bool CheckUserSession(string sessionID)
        {
            try
            {
                return Server.CheckUserSession(sessionID);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 用户注销或者页面超时时释放用户Session
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns>true-释放成功,false--释放失败</returns>
        public bool ReleaseSession(string sessionID)
        {
            try
            {
                return Server.ReleaseSession(sessionID, UserSessionEventEnum.Logout);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置预释放
        /// </summary>
        /// <param name="sessionID"></param>
        public bool SetPrepRelease(string sessionID)
        {
            try
            {
                Server.SetPrepRelease(sessionID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置预释放(包含登录模式)
        /// </summary>
        /// <param name="sessionID"></param>
        public bool SetPrepRelease(string sessionID, LoginTypeEnum logMode)
        {
            try
            {
                Server.SetPrepRelease(sessionID, logMode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 刷新指定用户的状态信息,改变此用户最近的活动时间为当前时间
        /// </summary>
        /// <param name="sessionID"></param>
        public bool RefreshSession(string sessionID)
        {
            try
            {
                Server.RefreshSession(sessionID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetUserData(string msg)
        {
            try
            {
                OpMessage opMsg = new OpMessage(msg);
                opMsg.Lable = "GetUserData";

                return ExecuteServiceByMsgObj(opMsg);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取系统数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetSystemData(string msg)
        {
            try
            {
                OpMessage opMsg = new OpMessage(msg);
                opMsg.Lable = "GetSystemData";

                return ExecuteServiceByMsgObj(opMsg);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] ExecuteService(string msg)
        {
            try
            {
                OpMessage opMsg = new OpMessage(msg);

                return ExecuteServiceByMsgObj(new OpMessage(msg));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 私有方法

        private byte[] ExecuteServiceByMsgObj(OpMessage opMsg)
        {
            try
            {
                byte[] data = null;
                Object dataObj = null;

                if (String.IsNullOrEmpty(opMsg.Operation))
                {
                    return null;
                }

                string label = (opMsg.Lable == null ? String.Empty : opMsg.Lable).ToLower();
                string op = (opMsg.Operation == null ? String.Empty : opMsg.Operation).ToLower();

                if (label == "getuserdata")
                {
                    UserInfo logonInfo = Server.GetLogonInfo(opMsg.SessionID);

                    if (PortalService.RunningMode == PortalService.RunningModeEnum.PerformanceTesting && logonInfo == null)
                    {
                        logonInfo = Server.GetLogonInfoByLoginName(opMsg.SessionID);    // 此时的SessionID为用户登录名
                    }

                    IList<string> ids = new List<string>();

                    if (logonInfo != null)
                    {
                        switch (op)
                        {
                            case "getlogoninfo":
                                dataObj = logonInfo;
                                break;
                            case "getuserinfo":
                                dataObj = new SimpleUserInfo(logonInfo);
                                break;
                        }
                    }
                }
                else if (label == "getsystemdata")
                {
                    switch (opMsg.Operation)
                    {
                        case "getallapplications":
                            dataObj = new List<Application>(Application.FindAll());
                            break;
                        case "getallmodules":
                            dataObj = new List<Module>(Module.FindAll());
                            break;
                        case "getallgroups":
                            dataObj = new List<OrgGroup>(OrgGroup.FindAll());
                            break;
                        case "getallusers":
                            dataObj = new List<OrgUser>(OrgUser.FindAll());
                            break;
                        case "getallroles":
                            dataObj = new List<OrgRole>(OrgRole.FindAll());
                            break;
                        case "getallauths":
                            dataObj = new List<Auth>(Auth.FindAll());
                            break;
                        case "getonlineusercount":
                            dataObj = Server.GetOnlineUserCount();
                            break;
                        case "getonlineuserlist":
                            dataObj = Server.GetOnlineUserList();
                            break;
                        case "":
                            break;
                    }
                }
                else
                {
                    try
                    {
                        switch (op)
                        {
                            case "checkusersession":
                                dataObj = Server.CheckUserSession(opMsg.SessionID);
                                break;
                            case "releasesession":
                                dataObj = Server.ReleaseSession(opMsg.SessionID);
                                break;
                            case "setpreprelease":
                                if (opMsg["logmode"].Type != TypeCode.Empty)
                                {
                                    Server.SetPrepRelease(opMsg.SessionID, (LoginTypeEnum)opMsg["logmode"].Value);
                                }
                                else
                                {
                                    Server.SetPrepRelease(opMsg.SessionID);
                                }

                                Server.SetPrepRelease(opMsg.SessionID);
                                break;
                            case "refreshsession":
                                Server.RefreshSession(opMsg.SessionID);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        dataObj = false;
                    }
                }

                if (dataObj != null)
                {
                    data = ServiceHelper.SerializeToBytes(dataObj);
                }

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
