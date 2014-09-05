using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 自增编号生成器
    /// </summary>
    public class SelfIncreaseGenerator : ICodeGenerator
    {
        /// <summary>
        /// 增加类型
        /// </summary>
        public enum IncreaseType
        {
            Base2,
            Base8,
            Base10,
            Base16,
            Base26,
            Base36
        }

        #region 成员

        #region 各种进制的元素定义

        public const string AlphabetBase2 = "01";
        public const string AlphabetBase8 = "01234567";
        public const string AlphabetBase10 = "0123456789";
        public const string AlphabetBase16 = "0123456789ABCDEF";
        public const string AlphabetBase26 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string AlphabetBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static Dictionary<IncreaseType, string> IncreaseTypeAlphabet = new Dictionary<IncreaseType, string>();

        #endregion

        public IncreaseType IncType { get; protected set; }
        public string MaxSN { get; set; }
        public int SNLength { get; set; }

        #endregion

        #region 构造函数

        static SelfIncreaseGenerator()
        {
            IncreaseTypeAlphabet.Add(IncreaseType.Base2, AlphabetBase2);
            IncreaseTypeAlphabet.Add(IncreaseType.Base8, AlphabetBase8);
            IncreaseTypeAlphabet.Add(IncreaseType.Base10, AlphabetBase10);
            IncreaseTypeAlphabet.Add(IncreaseType.Base16, AlphabetBase16);
            IncreaseTypeAlphabet.Add(IncreaseType.Base26, AlphabetBase26);
            IncreaseTypeAlphabet.Add(IncreaseType.Base36, AlphabetBase36);
        }

        public SelfIncreaseGenerator(IncreaseType incType)
            : this(incType, String.Empty)
        {
        }

        public SelfIncreaseGenerator(IncreaseType incType, string maxSN)
            : this(incType, maxSN, maxSN.Length)
        {
        }

        public SelfIncreaseGenerator(IncreaseType increaseType, string maxSN, int snLength)
        {
            this.IncType = increaseType;
            this.MaxSN = maxSN;
            this.SNLength = snLength;
        }

        #endregion

        #region ICodeGenerator 成员

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            return GetIncreasedSN(this.IncType, this.MaxSN, this.SNLength);
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 自增操作
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetIncreasedSN(IncreaseType type, string sn, int length)
        {
            string alphabet = GetAlphabetByIncreaseType(type);
            string perfixstr = String.Empty;

            if (sn.Length < length)
            {
                sn = sn.PadLeft(length, '0');
            }
            else if (sn.Length > length)
            {
                perfixstr = sn.Substring(0, sn.Length - length);
                sn = sn.Substring(perfixstr.Length, sn.Length - perfixstr.Length);
            }

            string incsn = AddSN(alphabet, sn);

            incsn = (incsn.Length > length) ? "0".PadLeft(length, '0') : incsn.PadLeft(length, '0');

            if (!String.IsNullOrEmpty(perfixstr))
            {
                incsn = perfixstr + incsn;
            }

            return incsn;
        }

        /// <summary>
        /// 通用序号算法器
        /// </summary>
        /// <param name="alphabet">字母表</param>
        /// <param name="SN">基数</param>
        /// <returns></returns>
        public static string AddSN(string alphabet, string sn)
        {
            char[] alphabets = alphabet.ToCharArray();
            char[] sns = sn.ToUpper().ToCharArray();
            int[] numbers = new int[sns.Length];

            //将字符数组转换为整型数组
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = alphabet.IndexOf(sns[i].ToString());
            }

            //计算整型数据
            int addStep = 1;
            int index = numbers.Length - 1;
            while (addStep != 0 && index >= 0)
            {
                if (numbers[index] < alphabets.Length - 1)
                {
                    numbers[index] += 1;
                    addStep = 0;
                }
                else
                {
                    numbers[index] = 0;
                    addStep = 1;
                }

                index--;
            }

            //将整型数组还原为字符数组
            for (int i = 0; i < numbers.Length; i++)
            {
                sns[i] = alphabets[numbers[i]];
            }

            //返回
            return new string(sns);
        }

        /// <summary>
        /// 由增量类型获取字母表
        /// </summary>
        /// <returns></returns>
        public static string GetAlphabetByIncreaseType(IncreaseType type)
        {
            string alphabet = String.Empty;

            if (IncreaseTypeAlphabet.ContainsKey(type))
            {
                alphabet = IncreaseTypeAlphabet[type];
            }

            return alphabet;
        }

        #endregion
    }
}
