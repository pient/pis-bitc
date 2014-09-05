using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.ActiveRecord;
using PIC.Security;
using PIC.Common;
using PIC.Common.Configuration;
using PIC.Common.Service;
using PIC.Common.Authentication;
using PIC.Portal.Model;

namespace PIC.Portal.Web.Services
{
    /// <summary>
    /// UserSession事件枚举
    /// </summary>
    public enum UserSessionEventEnum
    {
        Login,  // 登入
        Logout, // 登出
        Timeout,    // 超时
        PrepTimeout,    // 预登出(手动控制登出时间)
        Dispose,    // 系统处理
        Unknown     // 未知
    }

    /// <summary>
    /// 用户Session服务管理
    /// </summary>
    public sealed class UserSessionServer : IDisposable
    {
        #region 成员属性

        // 默认每隔100秒轮询刷新
        private long _scanInterval = 100000;

        public static long ScanInterval
        {
            get
            {
                return Instance._scanInterval;
            }
        }

        // 定时器（用于定时刷新用户状态）
        private static System.Timers.Timer timer = null;

        private static UserSessionServer _instance;

        /// <summary>
        /// 服务实例(单体)
        /// </summary>
        public static UserSessionServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSessionServer();

                    if (PICConfigurationManager.ServicesConfiguration != null)
                    {
                        _instance._scanInterval = PICConfigurationManager.ServicesConfiguration.UserSession.Get<long>("ScanInterval", 100) * 1000;
                    }

                    timer = new System.Timers.Timer(_instance._scanInterval);

                    timer.Enabled = true;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

                    // 默认登入系统用户，并记录系统用户SessionID, 此处ActiveRecord还没有初始化，不能进行数据库操作
                    // var sysUserSession = _instance.CreateSession(SysSystem.SYS_USERID, "local", "local", LoginTypeEnum.Unknown);
                    // AuthService.SetSysSessionID(sysUserSession.SessionID);
                }

                return _instance;
            }
        }

        private Dictionary<object, UserSession> _sessions = new Dictionary<object, UserSession>();

        /// <summary>
        /// Session
        /// </summary>
        public Dictionary<object, UserSession> Sessions
        {
            get
            {
                if (_sessions == null)
                {
                    _sessions = new Dictionary<object, UserSession>();
                }

                return _sessions;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 单体模式
        /// </summary>
        private UserSessionServer()
        {
        }

        // 定时扫描
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;

            try
            {
                // 轮询超时
                Instance.ScanTimeOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 创建新的用户Session
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        /// <param name="stateEvent"></param>
        /// <returns></returns>
        public UserSession CreateSession(string userID, string ip, string mac, LoginTypeEnum authType)
        {
            UserSession[] orgSession = _sessions.Values.Where(s => s.LogonInfo.UserID == userID).ToArray();
            UserSession session;

            if (orgSession.Count() <= 0)
            {
                session = new UserSession(userID);

                session.LogonInfo.IP = ip;
                session.LogonInfo.MAC = mac;
                session.LogonInfo.AuthType = authType;

                if (authType == LoginTypeEnum.PCIE || authType == LoginTypeEnum.PCClient)
                {
                    session.SessionEvent = UserSessionEventEnum.Login;
                }

                try
                {
                    lock (_sessions)
                    {
                        _sessions.Add(session.SessionID, session);
                    }
                }
                catch (Exception) { }
            }
            else
            {
                session = orgSession[0];

                session.RefreshState();
            }

            /*--写事件日志开始--*/
            Event evt = new Event();
            evt.UserID = session.LogonInfo.UserID;
            evt.LoginName = session.LogonInfo.LoginName;
            evt.IP = session.LogonInfo.IP;
            evt.Record = String.Format("CreateSession({0}, {1}, {2}, {3})", userID, ip, mac, authType.ToString());
            evt.DoCreate();
            /*--写事件日志结束--*/

            return session;
        }

        /// <summary>
        /// 创建新的用户Session
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        /// <param name="stateEvent"></param>
        /// <param name="sessionEvent">触发创建的事件类型</param>
        /// <returns></returns>
        public UserSession CreateSession(string userID, string ip, string mac, LoginTypeEnum authType, UserSessionEventEnum sessionEvent)
        {
            UserSession session = new UserSession(userID);

            session.LogonInfo.IP = ip;
            session.LogonInfo.MAC = mac;
            session.LogonInfo.AuthType = authType;
            session.SessionEvent = sessionEvent;

            try
            {
                lock (_sessions)
                {
                    _sessions.Add(session.SessionID, session);
                }
            }
            catch (Exception) { }

            /*--写事件日志开始--*/
            Event evt = new Event();
            evt.UserID = session.LogonInfo.UserID;
            evt.LoginName = session.LogonInfo.LoginName;
            evt.IP = session.LogonInfo.IP;
            evt.Record = String.Format("CreateSession({0}, {1}, {2}, {3})", userID, ip, authType.ToString(), sessionEvent.ToString());
            evt.DoCreate();
            /*--写事件日志结束--*/

            return session;
        }

        /// <summary>
        /// 获取Session信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns>UserSession</returns>
        public UserSession GetSession(string sessionID)
        {
            UserSession userSession = _sessions[sessionID];

            if (userSession != null)
            {
                return userSession;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断用户是否已注销,即sessionID是否失效
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns>true-有效  false-无效</returns>
        public bool CheckUserSession(string sessionID)
        {
            return _sessions.Keys.Contains(sessionID);
        }

        /// <summary>
        /// 用户注销或者页面超时时释放用户Session
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// 
        public bool ReleaseSession(string sessionID)
        {
            return ReleaseSession(sessionID, UserSessionEventEnum.Unknown);
        }

        /// <summary>
        /// 用户注销或者页面超时时释放用户状态对象
        /// </summary>
        /// <param name="passCode"></param>
        /// <param name="sessionEvent">触发事件方式</param>
        /// <returns></returns>
        public bool ReleaseSession(string sessionID, UserSessionEventEnum sessionEvent)
        {
            if (!_sessions.ContainsKey(sessionID))
            {
                return false;
            }

            UserSession userSession = (UserSession)_sessions[sessionID];
            userSession.SessionEvent = sessionEvent;


            // 若触发事件为logout(主动登出), 则若登入方式为未知的话，则放弃登出
            if (sessionEvent == UserSessionEventEnum.Logout && userSession.SessionEvent != UserSessionEventEnum.Login)
            {
                // return false;
            }

            if (userSession != null)
            {
                /*--写事件日志开始--*/
                Event evt = new Event();
                evt.UserID = userSession.LogonInfo.UserID;
                evt.LoginName = userSession.LogonInfo.LoginName;
                evt.IP = userSession.LogonInfo.IP;
                evt.Record = String.Format("ReleaseSession({0}, {1})", sessionID, sessionEvent.ToString());
                evt.DoCreate();
                /*--写事件日志结束--*/

                lock (_sessions)
                {
                    _sessions.Remove(sessionID);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 遍历所有的用户Session对象,释放超时对象
        /// </summary>
        public void ScanTimeOut()
        {
            int timeInterval = 0;  //当前时间与用户最近一次活动时间的差值

            foreach (UserSession session in _sessions.Values)
            {
                // 系统用户永不过期，这里跳过扫描
                if (session.LogonInfo.UserID == SysSystem.SYS_USERID)
                {
                    continue;
                }

                // 客户端超时判断
                int timeOut = session.TimeOut;

                if (session.LogonInfo.AuthType == LoginTypeEnum.PCClient)
                {
                    timeOut = session.ClientTimeOut;
                }

                DateTime dt;
                TimeSpan diff;
                DateTime now = DateTime.Now;

                dt = Convert.ToDateTime(session.LastActiveTime);
                diff = now - dt;
                timeInterval = (int)diff.TotalSeconds;

                if (timeInterval > timeOut)
                {
                    session.SessionEvent = UserSessionEventEnum.Timeout;
                    this.ReleaseSession(session.SessionID, UserSessionEventEnum.Timeout);
                }
                else
                {
                    //判断是否预登出
                    if (session.SessionEvent == UserSessionEventEnum.PrepTimeout
                        && timeInterval > session.PrepTimeOut)
                    {
                        this.ReleaseSession(session.SessionID, UserSessionEventEnum.PrepTimeout);
                    }
                }
            }//for
        }

        /// <summary>
        /// 设置预登释放
        /// </summary>
        /// <param name="sessionID"></param>
        public void SetPrepRelease(string sessionID)
        {
            SetPrepRelease(sessionID, LoginTypeEnum.Unknown);
        }

        /// <summary>
        /// 设置预登出
        /// </summary>
        /// <param name="logMode">登出模式</param>
        /// <param name="sessionID"></param>
        public void SetPrepRelease(string sessionID, LoginTypeEnum logMode)
        {
            UserSession userSession = _sessions[sessionID];

            if (userSession != null)
            {
                userSession.SessionEvent = UserSessionEventEnum.PrepTimeout;
            }
        }

        /// <summary>
        /// 刷新指定Session状态信息(改变此Session最近的活动时间为当前时间)
        /// </summary>
        /// <param name="sessionID"></param>
        public void RefreshSession(string sessionID)
        {
            UserSession userSession = null;
            if (_sessions.ContainsKey(sessionID))
            {
                userSession = _sessions[sessionID];
            }

            if (userSession != null)
            {
                userSession.RefreshState();
            }
        }

        /// <summary>
        /// 获取某个用户登录信息
        /// 用户的基本信息,包括登录名,加密后的口令,sessionID,用户ID,用户名称,系统名称
        /// </summary>
        /// <param name="sessionID">此用户的sessionID</param>
        /// <returns>此用户状态基本信息的DataForm格式的描述
        /// 如果此用户不存在则返回null</returns>
        public UserInfo GetLogonInfo(string sessionID)
        {
            UserSession userSession = _sessions[sessionID];

            if (userSession != null)
            {
                return userSession.LogonInfo;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <returns></returns>
        public string AuthenticateUser(string msg)
        {
            AuthMessage message = new AuthMessage(msg);

            if (String.IsNullOrEmpty(message.LoginName))
            {
                return null;
            }

            string sessionID = String.Empty;

            // 验证用户, 这里需要完善，不单可以通过数据库验证，还需要可以单点验证
            // OrgUser user = OrgUser.Authenticate(message.LoginName, message.Password, message.PasswordEncrypted);
            int result = VerifyLogin(message.LoginName, message.Password, message.PasswordEncrypted);

            if (result > 0)
            {
                UserSession cus = this.GetSessionByLoginName(message.LoginName);

                // 查看用户是否已经登录(当前若用户在线则强迫当前用户下线, 采用新用户登录)
                if (cus != null)
                {
                    // return UserStatusEnum.Online.ToString();    // 用户仍然在线
                    // this.ReleaseSession(cus.SessionID);

                    sessionID = cus.SessionID;
                }
                else
                {
                    OrgUser user = OrgUser.GetByLoginName(message.LoginName);
                    UserSession us = this.CreateSession(user.UserID, message.IP, message.MAC, message.AuthType);
                    if (us != null) { sessionID = us.SessionID; }
                }
            }

            return sessionID;
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 由登录名获取Session
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        private UserSession GetSessionByLoginName(string loginName)
        {
            UserSession us = Sessions.Values.FirstOrDefault<UserSession>(ent => ent.LogonInfo.Name == loginName);

            return us;
        }

        /// <summary>
        /// 检查用户登录信息
        /// </summary>
        private int VerifyLogin(string loginName, string password, bool passwordEncrypted)
        {
            int result = 0;

            OrgUser user = OrgUser.Authenticate(loginName, password, passwordEncrypted);

            if (user != null)
            {
                result = 1;
            }
            else   // 本地库验证失败，进行ICE认证
            {
                result = Utilities.ICEClientHelper.CheckPassword(loginName, password);
            }

            return result;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// 释放所有用户
        /// </summary>
        public void Dispose()
        {
            foreach (string usid in Sessions.Keys)
            {
                this.ReleaseSession(usid, UserSessionEventEnum.Dispose);
            }
        }

        #endregion
    }

    /// <summary>
    /// 用户Session类
    /// </summary>
    public class UserSession
    {
        #region 成员属性

        private string _sessionID;

        /// <summary>
        /// SessionId
        /// </summary>
        public string SessionID
        {
            get { return _sessionID; }
        }

        private UserInfo _logonInfo;

        /// <summary>
        /// 登录信息
        /// </summary>
        public UserInfo LogonInfo
        {
            get { return _logonInfo; }
        }

        /// <summary>
        /// 用户Session事件状态
        /// </summary>
        public UserSessionEventEnum SessionEvent = UserSessionEventEnum.Unknown;

        private string _lastActiveTime;

        /// <summary>
        /// 用户最后活动时间
        /// </summary>
        public string LastActiveTime
        {
            get { return _lastActiveTime; }
        }

        /// <summary>
        /// 用户超时时间,默认为360000秒,100小时(清除异常登录的垃圾)
        /// </summary>
        private int _timeOut = 6000;    // 100分钟超时

        public int TimeOut
        {
            get { return _timeOut; }
        }

        /// <summary>
        /// 用户预登出时间,默认为30秒
        /// </summary>
        private int _prepTimeOut = 30;

        public int PrepTimeOut
        {
            get { return _prepTimeOut; }
        }

        /// <summary>
        /// 客户端超时事件,默认为1200秒(20分钟)
        /// </summary>
        private int _clientTimeOut = 1200;

        public int ClientTimeOut
        {
            get { return _clientTimeOut; }
        }

        private Hashtable _tag = new Hashtable();

        /// <summary>
        /// 扩展数据
        /// </summary>
        public Hashtable Tag
        {
            get { return _tag; }
        }

        #endregion

        #region 构造函数

        public UserSession(string userID)
        {
            _sessionID = this.NewSessionID();

            using (new SessionScope())
            {
                OrgUser user = OrgUser.Find(userID);
                // user.RetrieveAllAuth(); // 触发Lazy Read

                if (user != null)
                {
                    _logonInfo = new UserLogonInfo(_sessionID, user);
                }
            }

            if (PICConfigurationManager.ServicesConfiguration != null)
            {
                // 用户预登出时间,默认为30秒
                _prepTimeOut = PICConfigurationManager.ServicesConfiguration.UserSession.Get<int>("PrepTimeOut", 30);

                // Session客户端超时时间默认为20分钟
                _clientTimeOut = PICConfigurationManager.ServicesConfiguration.UserSession.Get<int>("ClientTimeOut", 20) * 60;

                // Session 超时时间默认为100分钟
                _timeOut = PICConfigurationManager.ServicesConfiguration.UserSession.Get<int>("TimeOut", 100) * 60;
            }

            _lastActiveTime = DateTime.Now.ToLongTimeString();
        }

        #endregion

        #region 私有函数

        private string NewSessionID()
        {
            return Guid.NewGuid().ToString();
        }

        #endregion

        #region 公有函数

        /// <summary>
        /// 刷新指定Session状态(改变此Session最近的活动时间为当前时间, 若为预登出则并重设SessionEvent)
        /// </summary>
        public void RefreshState()
        {
            lock (this)
            {
                this._lastActiveTime = DateTime.Now.ToString();

                if (SessionEvent == UserSessionEventEnum.PrepTimeout
                    && (_logonInfo.AuthType == LoginTypeEnum.PCClient || _logonInfo.AuthType == LoginTypeEnum.PCIE))
                {
                    SessionEvent = UserSessionEventEnum.Login;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 用户登录信息类
    /// </summary>
    [Serializable]
    public class UserLogonInfo : UserInfo
    {
        #region 属性变量

        OrgUser user;

        /// <summary>
        /// 用户信息
        /// </summary>
        public OrgUser User
        {
            get { return user; }
        }

        #endregion

        #region 构造函数

        public UserLogonInfo(string sessionID, OrgUser user)
        {
            this.user = user;
            this.SessionID = sessionID;

            AuthType = LoginTypeEnum.Unknown;
        }

        #endregion

        #region UserInfo成员

        public override string Name
        {
            get
            {
                if (user != null)
                {
                    return user.Name;
                }
                else
                {
                    return "";
                }
            }
            set { throw new NotSupportedException(); }
        }

        public override string UserID
        {
            get
            {
                if (user != null)
                {
                    return user.UserID;
                }
                else
                {
                    return "";
                }
            }
            set { throw new NotSupportedException(); }
        }

        public override string LoginName
        {
            get
            {
                if (user != null)
                {
                    return user.LoginName;
                }
                else
                {
                    return "";
                }
            }
            set { throw new NotSupportedException(); }
        }

        #endregion
    }
}
