using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PIC.Portal.Template;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime.Parser.Node;
using NVelocity.Context;

namespace PIC.Portal
{
    /// <summary>
    /// 模版帮助类
    /// </summary>
    public class TmplHelper
    {
        #region 静态方法

        /// <summary>
        /// 获取表达式值
        /// </summary>
        /// <param name="tmplStr"></param>
        /// <param name="tmplCtx"></param>
        /// <returns></returns>
        public static object Execute(string tmplStr, TmplContext tmplCtx)
        {
            object rtnVal = null;

            NVelocity.Runtime.RuntimeInstance ri = new NVelocity.Runtime.RuntimeInstance();
            ri.Init();

            VelocityContext c = tmplCtx.ToVelocityContext();

            rtnVal = Execute(tmplStr, c, ri);

            return rtnVal;
        }

        /// <summary>
        /// 获取表达式值
        /// </summary>
        /// <param name="ri"></param>
        /// <param name="vCtx"></param>
        /// <param name="tmplStr"></param>
        /// <param name="tmplCtx"></param>
        /// <returns></returns>
        public static object Execute(string tmplStr, VelocityContext vCtx, NVelocity.Runtime.RuntimeInstance ri)
        {
            object rtnVal = null;

            TextReader reader = new StringReader(tmplStr);

            string logTag = "";

            SimpleNode nodeTree = ri.Parse(reader, logTag);

            InternalContextAdapterImpl ica = new InternalContextAdapterImpl(vCtx);
            ica.PushCurrentTemplateName(logTag);

            try
            {
                nodeTree.Init(ica, ri);

                if (nodeTree.ChildrenCount == 1 && nodeTree.GetChild(0) is ASTReference)
                {
                    ASTReference ar = nodeTree.GetChild(0) as ASTReference;

                    rtnVal = ar.Execute(null, ica);
                }
                else
                {
                    StringWriter sw = new StringWriter();

                    nodeTree.Render(ica, sw);

                    rtnVal = sw.GetStringBuilder().ToString();
                }
            }
            finally
            {
                ica.PopCurrentTemplateName();
            }

            return rtnVal;
        }

        /// <summary>
        /// 生成模版结果
        /// </summary>
        /// <param name="tmplStr"></param>
        /// <param name="tmplCtx"></param>
        /// <returns></returns>
        public static string Render(string tmplStr, TmplContext tmplCtx = null)
        {
            string rtnStr = null;

            if (tmplCtx == null)
            {
                tmplCtx = TmplContext.GetSysContext();
            }

            var vCtx = tmplCtx.ToVelocityContext();

            rtnStr = Render(tmplStr, vCtx);

            return rtnStr;
        }

        /// <summary>
        /// 获取邮件模版配置，邮件模版一般为Json格式
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T GetTemplateConfig<T>(string code)
        {
            var taskTmpl = Model.Template.Get(code);
            var cfg = JsonHelper.GetObject<T>(taskTmpl.Config);

            return cfg;
        }

        #endregion

        #region 内部方法

        internal static string Render(string tmplString, VelocityContext vContext, VelocityEngine vEngine = null)
        {
            var dataString = tmplString;

            if (!String.IsNullOrEmpty(tmplString))
            {
                if (vEngine == null)
                {
                    vEngine = new VelocityEngine();
                    vEngine.Init();
                }

                StringWriter vltWriter = new StringWriter();

                vEngine.Evaluate(vContext, vltWriter, null, tmplString);

                dataString = vltWriter.GetStringBuilder().ToString();
            }

            return dataString;
        }

        #endregion
    }
}
