using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal
{
    /// <summary>
    /// 模版预处理器
    /// </summary>
    public interface ITemplateConfigProcessor<T>
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        T GetObject(string configstr);
    }

    /// <summary>
    /// 基模版配置处理
    /// </summary>
    public abstract class TemplateConfigProcessor<T> : ITemplateConfigProcessor<T>
    {
        #region 成员属性

        public const string WRAPPER_BEGIN_CHAR = "{";   // 左包装字符
        public const string WRAPPER_END_CHAR = "}";  // 右包装字符

        /// <summary>
        /// 前缀
        /// </summary>
        public abstract string Prefix
        {
            get;
        }

        /// <summary>
        /// 左包装
        /// </summary>
        public string WRAPPER_BEGIN
        {
            get
            {
                return Prefix + WRAPPER_BEGIN_CHAR;
            }
        }

        /// <summary>
        /// 左包装
        /// </summary>
        public string WRAPPER_END
        {
            get
            {
                return WRAPPER_END_CHAR;
            }
        }

        #endregion

        #region 构造函数

        public TemplateConfigProcessor() { }

        #endregion

        #region 私有方法

        /// <summary>
        /// 预处理配置信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual string Preprocess(string configstr)
        {
            return configstr;
        }

        /// <summary>
        /// 后继处理信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual T Postprocess(T configobj)
        {
            return configobj;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="configstr"></param>
        /// <returns></returns>
        public virtual T GetObject(string configstr)
        {
            if (!configstr.Contains(WRAPPER_BEGIN))
            {
                return default(T);
            }
            else
            {
                string peeloffstr = configstr.PeerOff(WRAPPER_BEGIN, WRAPPER_END);

                string tconfigstr = Preprocess(peeloffstr).Wrap(WRAPPER_BEGIN_CHAR, WRAPPER_END_CHAR);

                T tconfigobj = JsonHelper.GetObject<T>(tconfigstr);

                if (tconfigobj != null)
                {
                    tconfigobj = Postprocess(tconfigobj);
                }

                return tconfigobj;
            }
        }

        #endregion
    }
}
