using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVelocity;
using NVelocity.Context;
using PIC.Portal.Model;
using PIC.Portal.Template;

namespace PIC.Portal
{
    public class SNService
    {
        #region 成员

        public readonly static SNService Instance = new SNService();

        #endregion

        #region 构造函数

        protected SNService() { }

        #endregion

        #region 实例方法

        /// <summary>
        /// 获取上下文信息
        /// </summary>
        /// <returns></returns>
        public TmplContext GetContext()
        {
            TmplContext ctx = SNService.GetDefContext();

            return ctx;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Enumeration GetSysEnumeration(string code)
        {
            Enumeration sysenum = Enumeration.Get(code);

            return sysenum;
        }

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Parameter GetSysParameter(string code)
        {
            Parameter sysparam = Parameter.Get(code);

            return sysparam;
        }

        /// <summary>
        /// 获取增量代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetIncreaseCode(string snCode)
        {
            Model.Template tmpl = Model.Template.Get(snCode);

            if (tmpl.TemplateType == Model.Template.TemplateTypeEnum.SN)
            {
                string gsn = tmpl.RenderString();

                return gsn;
            }

            return snCode;
        }

        /// <summary>
        /// 获取增量代码
        /// </summary>
        /// <returns></returns>
        public string GetIncreasedCode(string incType, string maxSN, int snLength)
        {
            SelfIncreaseGenerator.IncreaseType inctype = CLRHelper.GetEnum<SelfIncreaseGenerator.IncreaseType>(incType);

            string sn = SelfIncreaseGenerator.GetIncreasedSN(inctype, maxSN, snLength);

            return sn;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 获取默认上下文
        /// </summary>
        /// <returns></returns>
        public static TmplContext GetDefContext()
        {
            var sysCtx = TmplContext.GetSysContext();
            sysCtx.Set("SNService", SNService.Instance);

            return sysCtx;
        }

        #endregion
    }
}
