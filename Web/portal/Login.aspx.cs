using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using NHibernate.Criterion;
using PIC.Portal.Model;

namespace PIC.Portal.Web
{
    public partial class Login : System.Web.UI.Page
    {
        #region Classes

        /// <summary>
        /// 登录包
        /// </summary>
        public class LoginPackage
        {
            public string uname { get; set; }
            public string pwd { get; set; }
            public bool saveAcount { get; set; }
            public bool savePassword { get; set; }
        }

        #endregion

        #region 成员

        private bool asyncreq = false;

        protected LoginPackage PersistedLoginPkg = null;

        #endregion

        #region ASP.NET 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            asyncreq = CLRHelper.ConvertValue<bool>(Request["asyncreq"], false);

            if (Request["reqaction"] == "login")
            {
                DoLogin();
            }

            if (asyncreq == false)
            {
                PersistedLoginPkg = GetLoginPackageFromCookie();

                this.saveAcount.Checked = PersistedLoginPkg.saveAcount;
                this.savePassword.Checked = PersistedLoginPkg.savePassword;
            }
        }

        #endregion

        #region 支持方法

        /// <summary>
        /// 登录
        /// </summary>
        private void DoLogin()
        {
            var loginPkg = GetLoginPackageFromRequest();

            if (loginPkg != null)
            {
                OrgUser usr = OrgUser.FindFirst(
                    Expression.Or(Expression.Eq(OrgUser.Prop_LoginName, loginPkg.uname), 
                    Expression.Eq(OrgUser.Prop_WorkNo, loginPkg.uname)));

                if (usr == null)
                {
                    Response.Write("用户不存在！");
                }
                else if (usr.Status != 1)
                {
                    Response.Write("用户未激活，请联系管理员！");

                }
                else
                {
                    LoginUser(loginPkg, false);
                }
            }
            else
            {
                Response.Write("请输入用户名！");
            }

            Response.End();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        private void LoginUser(LoginPackage loginPkg, bool pwdEncrypted)
        {
            try
            {
                string sid = AuthService.AuthUser(loginPkg.uname, loginPkg.pwd, pwdEncrypted);

                if (!String.IsNullOrEmpty(sid))
                {
                    // 无效SessionID
                    if (sid.IndexOf("ex") == 0)
                    {
                        Response.Write(sid);
                    }
                    else
                    {
                        PersistLoginPackage(loginPkg);
                    }

                    string url = FormsAuthentication.GetRedirectUrl(loginPkg.uname, true);

                    if (asyncreq)
                    {
                        Response.Write(String.Format("success,{0}", url));
                    }
                    else
                    {
                        Response.Redirect(url);
                    }
                }
                else
                {
                    Response.Write("登陆失败，用户名或密码不正确！");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }

            Response.End();
        }

        /// <summary>
        /// 持久化登录包
        /// </summary>
        /// <param name="loginPkg"></param>
        private void PersistLoginPackage(LoginPackage loginPkg)
        {
            var newLoginPkg = new LoginPackage()
            {
                saveAcount = loginPkg.saveAcount,
                savePassword = loginPkg.savePassword
            };

            if (loginPkg.saveAcount)
            {
                newLoginPkg.uname = loginPkg.uname;
            }

            if (loginPkg.savePassword)
            {
                newLoginPkg.pwd = loginPkg.pwd;
            }

            var loginPkgStr = JsonHelper.GetJsonString(newLoginPkg);
            var secLoginPkgStr = Security.DESEncrypt.DoEncryptString(loginPkgStr);

            Response.SetCookie(new HttpCookie("lp", secLoginPkgStr)
            {
                Expires = DateTime.Now.AddDays(10)  // 保持10天
            });
        }

        /// <summary>
        /// 从请求中获取登录包
        /// </summary>
        /// <returns></returns>
        private LoginPackage GetLoginPackageFromRequest()
        {
            var loginPkg = new LoginPackage()
            {
                uname = Request["uname"],
                pwd = Request["pwd"],
                saveAcount = Request["saveAcount"].ToBoolean(false),
                savePassword = Request["savePassword"].ToBoolean(false)
            };

            return loginPkg;
        }

        /// <summary>
        /// 从Cookie中获取登录包
        /// </summary>
        /// <returns></returns>
        private LoginPackage GetLoginPackageFromCookie()
        {
            LoginPackage loginPkg = new LoginPackage();

            var loginPkgCookie = Request.Cookies.Get("lp");

            if (loginPkgCookie != null)
            {
                var secLoginPkgStr = loginPkgCookie.Value;

                if (!String.IsNullOrEmpty(secLoginPkgStr))
                {
                    var loginPkgStr = Security.DESEncrypt.DoDecryptString(secLoginPkgStr);

                    loginPkg = JsonHelper.GetObject<LoginPackage>(loginPkgStr);
                }
            }

            return loginPkg;
        }

        #endregion
    }
}
