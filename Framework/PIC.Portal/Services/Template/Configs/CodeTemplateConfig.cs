using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Template
{
    public class CodeTemplateConfig : DataTemplateConfig
    {
        #region 属性

        public string TemplateString { get; set; }

        #endregion

        #region 构造函数

        public CodeTemplateConfig() { }

        #endregion

        #region 公共方法

        protected override TmplContext GetDataContext(EasyDictionary ctxParams = null, bool isSysContext = true)
        {
            var ctx = base.GetDataContext(ctxParams, isSysContext);

            if (ctx == null)
            {
                ctx = TmplContext.GetSysContext();
            }

            ctx.Set("SNService", SNService.Instance);

            return ctx;
        }

        /// <summary>
        /// 默认显然为Json String
        /// </summary>
        /// <returns></returns>
        public override string RenderString(EasyDictionary ctxParams = null)
        {
            var ctx = this.GetDataContext(ctxParams, true);

            string str = TmplHelper.Render(TemplateString, ctx);

            return str;
        }

        #endregion
    }
}
