using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 编码生成器
    /// </summary>
    public interface ICodeGenerator
    {
        string Generate();
    }

    /// <summary>
    /// 编码生成器基类
    /// </summary>
    public abstract class CodeGeneratorBase : ICodeGenerator
    {
        #region 成员

        // 装饰模式
        protected ICodeGenerator innerGenerator;

        #endregion

        #region 构造函数

        public CodeGeneratorBase()
        {
        }

        public CodeGeneratorBase(ICodeGenerator generator)
        {
            this.innerGenerator = generator;
        }

        #endregion

        #region ICodeGenerator 成员

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public virtual string Generate()
        {
            string code = String.Empty;

            if (innerGenerator != null)
            {
                code = innerGenerator.Generate();
            }

            return code;
        }

        #endregion
    }
}
