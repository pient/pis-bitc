using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Portal.Model;
using NVelocity.App;
using System.IO;

namespace PIC.Portal.Services
{
    public class TemplateService : TemplateServiceBase<string>
    {
        #region TemplateServiceBase Members

        /// <summary>
        /// 生成数据
        /// </summary>
        /// <returns></returns>
        public override string Generate(Model.Template template)
        {
            string rtnstr = String.Empty;
            string config = template.Config;

            TemplateBase<string> tmpl;

            switch (template.Type)
            {
                case "Data":
                case "DataRequest":
                    tmpl = JsonHelper.GetObject<DataRequestTemplate>(config);
                    break;
                default:
                    tmpl = JsonHelper.GetObject<TemplateBase<string>>(config);
                    break;
            }

            rtnstr = tmpl.Generate();

            return rtnstr;
        }

        #endregion

        #region Static Methods

        public static string Generate(string tmplString, NVelocity.VelocityContext vContext, VelocityEngine vEngine = null)
        {
            var dataString = tmplString;

            if (vEngine == null)
            {
                vEngine = new VelocityEngine();
                vEngine.Init();
            }

            if (!String.IsNullOrEmpty(tmplString))
            {
                StringWriter vltWriter = new StringWriter();

                vEngine.Evaluate(vContext, vltWriter, null, tmplString);

                dataString = vltWriter.GetStringBuilder().ToString();
            }

            return dataString;
        }


        #endregion
    }
}
