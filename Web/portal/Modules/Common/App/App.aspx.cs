using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using NHibernate;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using PIC.Common.Service;
using PIC.Data;
using PIC.Common;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;
using APEntity = PIC.Portal.Model;
using PIC.Portal.Template;

namespace PIC.Portal.Web.Modules.Common.App
{
    public partial class App : BaseDataPage
    {
        #region 成员

        public string content = "";
        protected bool ischecklogon = true;

        private string code = "";  // 功能路径编码

        #endregion

        #region ASP.NET 事件

        protected override void OnInit(EventArgs e)
        {
            ischecklogon = CLRHelper.ConvertValue<bool>(Request["ischecklogon"], false);

            this.IsCheckLogon = ischecklogon;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            code = RequestData.Get<string>("__code");

            switch (this.RequestActionString)
            {
                case "exec":
                    content = ExecFunc();
                    break;
                case "qrytext": // 加载文字文件
                    content = QueryText();
                    break;
                case "tmpl":
                    content = GetTmplData();
                    break;
            }

            Response.Write(content);
            Response.End();
        }

        #endregion

        #region 支持方法

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <returns></returns>
        private string ExecFunc()
        {
            switch (code)
            {
                case "verifypwd":
                    return VerifyPwd();
                case "chgpwd":
                    return ChangePwd();
            }

            return "{result: 'fail'}";
        }

        /// <summary>
        /// 查询字符,如文本，js文件等
        /// </summary>
        /// <returns></returns>
        private string QueryText()
        {
            switch (code)
            {
                case "wfformdef":
                    return GetWfFormDefine();
            }

            return null;
        }

        /// <summary>
        /// 获取模版字数据
        /// </summary>
        /// <returns></returns>
        private string GetTmplData()
        {
            String rtnstr = null;
            Model.Template tmpl = null;

            var tid = RequestData.Get<string>("tid");
            var tcode = RequestData.Get<string>("tcode");
            var ctxParams = RequestData.Get<EasyDictionary>("ctxparams");

            if (!String.IsNullOrEmpty(tid))
            {
                tmpl = Model.Template.Find(tid);
            }
            else if (!String.IsNullOrEmpty(tcode))
            {
                tmpl = Model.Template.Get(tcode);
            }

            switch (code)
            {
                case "renderstr":
                    rtnstr = tmpl.RenderString(ctxParams);
                    break;
            }

            return rtnstr;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <returns></returns>
        private string VerifyPwd()
        {
            bool success = false;

            var uid = RequestData.Get<string>("uid");
            var pwd = RequestData.Get<string>("pwd");
            var encrypted = RequestData.Get<bool>("encrypted", false);

            if (uid == null && this.UserInfo != null)
            {
                uid = UserInfo.UserID;
            }

            if (!String.IsNullOrEmpty(uid))
            {
                var user = OrgUser.Find(uid);

                success = user.VerifyPassword(pwd, encrypted);
            }

            if (success)
            {
                return "{result: 'success'}";
            }
            else
            {
                return "{result: 'fail'}";
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private string ChangePwd()
        {
            var uid = RequestData.Get<string>("id");
            var oldPwd = RequestData.Get<string>("oldpwd");
            var newPwd = RequestData.Get<string>("newpwd");

            if (String.IsNullOrEmpty(uid))
            {
                uid = UserInfo.UserID;
            }

            if (!String.IsNullOrEmpty(oldPwd) && !String.IsNullOrEmpty(newPwd))
            {
                var user = OrgUser.Find(uid);

                if (user.VerifyPassword(oldPwd, false) == true)
                {
                    user.ResetPassword(newPwd);

                    this.SetMessage("修改成功");
                }
                else
                {
                    throw (new MessageException("验证失败!"));
                }
            }

            return "{result: 'success'}";
        }

        #endregion

        #region 文本获取

        /// <summary>
        /// 获取流程表单定义信息
        /// </summary>
        /// <returns></returns>
        private string GetWfFormDefine()
        {
            WfDefine def = null;
            var id = RequestData.Get<string>("id", RequestData.Get<string>("did"));
            var dcode = RequestData.Get<string>("dcode");

            if (!String.IsNullOrEmpty(id))
            {
                def = WfDefine.Find(id);
            }

            if (def == null)
            {
                def = WfDefine.Get(dcode);
            }

            return def.FormDefine;
        }

        #endregion
    }
}