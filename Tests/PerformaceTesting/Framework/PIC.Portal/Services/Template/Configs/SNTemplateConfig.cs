using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Template
{
    public class SNTemplateConfig : TemplateConfig
    {
        #region 属性

        public string IncreaseType { get; set; }    // 增长类型
        public int? Length { get; set; }  // 序列号长度
        public string CurrentSN { get; set; }   // 当前序列号

        [JsonIgnore]
        public SelfIncreaseGenerator.IncreaseType SNIncreaseType
        {
            get { return CLRHelper.GetEnum<SelfIncreaseGenerator.IncreaseType>(this.IncreaseType); }
            set { this.IncreaseType = value.ToString(); }
        }

        #endregion

        #region 构造函数

        public SNTemplateConfig() { }

        #endregion

        #region 公共方法

        /// <summary>
        /// 默认显然为Json String
        /// </summary>
        /// <returns></returns>
        public override string RenderString(EasyDictionary ctxParams = null)
        {
            SelfIncreaseGenerator incGenerator = new SelfIncreaseGenerator(
                this.SNIncreaseType,
                this.CurrentSN,
                this.Length.GetValueOrDefault() > 0 ? this.Length.Value : this.CurrentSN.Length);

            string gsn = incGenerator.Generate();

            this.CurrentSN = gsn;

            return gsn;
        }

        #endregion
    }
}
