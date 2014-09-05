using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NVelocity;
using NVelocity.App;
using PIC.Portal.Template;

namespace PIC.Portal
{
    public class NumberGenerator : CodeGeneratorBase
    {
        #region 成员

        VelocityEngine vEngine = null;

        #endregion

        #region 属性

        /// <summary>
        /// 模板类型
        /// </summary>
        public string TemplateString { get; set; }

        public string Seperator { get; set; }

        protected TmplContext _GeneratorContext = null;

        /// <summary>
        /// 生成对象的上下文
        /// </summary>
        public TmplContext GeneratorContext
        {
            get
            {
                if (_GeneratorContext == null)
                {
                    _GeneratorContext = SNService.GetDefContext();
                }

                return _GeneratorContext;
            }
        }

        #endregion

        #region 构造函数

        public NumberGenerator()
            : base()
        {
            Init();
        }

        public NumberGenerator(ICodeGenerator generator)
            : base(generator)
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            TemplateString = String.Empty;
            Seperator = String.Empty;

            // Initialize NVelocity
            vEngine = new VelocityEngine();
            vEngine.Init();
        }

        #endregion

        #region CodeGeneratorBase成员

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public override string Generate()
        {
            string code = String.Empty;
            string tcode = String.Empty;

            if (innerGenerator != null)
            {
                code = innerGenerator.Generate();
            }

            if (!String.IsNullOrEmpty(TemplateString))
            {
                StringWriter vltWriter = new StringWriter();

                var vCtx = GeneratorContext.ToVelocityContext();

                vEngine.Evaluate(vCtx, vltWriter, null, TemplateString);
                
                tcode = vltWriter.GetStringBuilder().ToString();
            }

            code += String.Format("{0}{1}{2}", code, Seperator, tcode);

            return code;
        }

        #endregion

        #region 静态方法



        #endregion
    }
}
