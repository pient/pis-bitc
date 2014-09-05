using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Template
{
    public class DataTemplateConfig : TemplateConfig
    {
        #region 属性

        public string DataContextString { get; set; }

        #endregion

        #region 构造函数

        public DataTemplateConfig() { }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取数据上下文
        /// </summary>
        /// <returns></returns>
        protected virtual TmplContext GetDataContext(EasyDictionary ctxParams = null, bool isSysContext = true)
        {
            TmplContext ctx = null;

            var sysCtx = TmplContext.GetSysContext();
            sysCtx.Set("CtxParams", ctxParams);

            string ctxString = TmplHelper.Render(this.DataContextString, sysCtx);

            var ctxCfg = JsonHelper.GetObject<DataContextConfig>(ctxString);

            if (ctxCfg != null)
            {
                ctx = ctxCfg.GetDataContext(ctxParams, isSysContext);
            }

            return ctx;
        }

        /// <summary>
        /// 默认显然为Json String
        /// </summary>
        /// <returns></returns>
        public override string RenderString(EasyDictionary ctxParams = null)
        {
            var ctx = this.GetDataContext(ctxParams, false);

            var str = JsonHelper.GetJsonString(ctx);

            return str;
        }

        #endregion

        #region 支持方法

        private void GetJsonObj(string tmplStr)
        {
            // JsonHelper.GetObject<EasyDictionary>(tmplStr);
        }

        #endregion
    }
}
