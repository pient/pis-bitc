using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Template
{
    public class StandardTemplateConfig : DataTemplateConfig
    {
        #region 属性

        public string TemplateString { get; set; }

        #endregion

        #region 构造函数

        public StandardTemplateConfig() { }

        #endregion

        #region 公共方法

        /// <summary>
        /// 渲染模版
        /// </summary>
        /// <param name="ctxParams"></param>
        /// <returns></returns>
        public override string RenderString(EasyDictionary ctxParams = null)
        {
            var ctx = this.GetDataContext(ctxParams, true);

            if (ctx == null)
            {
                ctx = TmplContext.GetSysContext();
            }

            ctx.Set("CtxParams", ctxParams);

            var str = TmplHelper.Render(TemplateString, ctx);

            return str;
        }

        #endregion
    }
}
