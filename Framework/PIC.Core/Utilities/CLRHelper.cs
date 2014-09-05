using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.IO;

namespace PIC
{
    public class CLRHelper
    {
        #region 对象操作

        public static bool IsNullOrEmpty(object obj)
        {
            return (obj == null || String.IsNullOrEmpty(obj.ToString()));
        }

        #endregion

        #region 程序集处理

        /// <summary>
        /// 根据程序集文件名获取程序集
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static Assembly[] GetAssemblysByNames(params string[] assemblyNames)
        {
            if (assemblyNames == null || assemblyNames.Length == 0)
            {
                return null;
            }

            Assembly[] assms = new Assembly[assemblyNames.Length];

            for (int i = 0; i < assemblyNames.Length; i++)
            {
                assms[i] = Assembly.Load(assemblyNames[i]);
            }

            return assms;
        }

        public static T CreateInstance<T>(string assemblyName,string nameSpace, string className)
        {
            try
            { 
                string fullName = nameSpace + "." + className;  //命名空间.类型名
                //此为第一种写法
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);  //加载程序集，创建程序集里面的 命名空间.类型名 实例
                return (T)ect;  //类型转换并返回
            }
            catch
            {                
                //发生异常，返回类型的默认值 
                return default(T);           
            }
        }

        public static T CreateInstance<T>(string typeName)
        {
            try
            {
                //命名空间.类型名,程序集
                Type o = Type.GetType(typeName);//加载类型
                object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值 
                return default(T);
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        public static object ExecStaticMethod(string typeName, string methodName, object[] parameters)
        {
            Type tx = Type.GetType(typeName);
            MethodInfo mi = tx.GetMethod(methodName, BindingFlags.Static, null, new Type[]{}, null);

            var rtn = mi.Invoke(null, parameters);

            return rtn;
        }

        #endregion

        #region 类型处理

        /// <summary>
        /// 过滤Null值
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T FilterNull<T>(object val, T def)
        {
            if (val == null)
            {
                return def;
            }
            else
            {
                return (T)val;
            }
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr, T defValue, bool ignoreCase)
        {
            if (enumStr == null)
            {
                return defValue;
            }

            T enumVal = default(T);

            try
            {
                enumVal = (T)Enum.Parse(typeof(T), enumStr.ToString(), ignoreCase);
            }
            catch
            {
                enumVal = defValue;
            }

            return enumVal;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr, T defValue)
        {
            T enumVal = GetEnum<T>(enumStr, defValue, true);

            return enumVal;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr, bool ignoreCase)
        {
            T enumVal = GetEnum<T>(enumStr, default(T), ignoreCase);

            return enumVal;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEnum<T>(object enumStr)
        {
            T enumVal = GetEnum<T>(enumStr, true);

            return enumVal;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(object value)
        {
            return ConvertValue<T>(value, default(T));
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static T ConvertValue<T>(object value, T defValue)
        {
            T objVal = default(T);

            if (value != null)
            {
                try
                {
                    objVal = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    try
                    {
                        objVal = (T)value;
                    }
                    catch { }
                }
            }
            else
            {
                objVal = defValue;
            }

            return objVal;
        }

        #endregion

        #region 类型处理

        public static int GetWeekOfYear()
        {
            return GetWeekOfYear(DateTime.Now);
        }

        /// <summary>
        /// 获取年中的周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime source)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                source, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public static int GetQuarterOfYear()
        {
            return GetQuarterOfYear(DateTime.Now);
        }

        /// <summary>
        /// 获取年中的季度
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetQuarterOfYear(DateTime source)
        {
            int month = source.Month;
            if (month >= 0 && month <= 3)
            {
                return 1;
            }
            else if(month >=4 && month <= 6)
            {
                return 2;
            }
            else if (month >= 7 && month <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        #endregion

        #region IO操作

        public static byte[] ReadStream(Stream stream)
        {
            return ReadStream(stream, 10000);
        }

        public static byte[] ReadStream(Stream stream, int bufferSize)
        {
            byte[] sbytes = new byte[stream.Length];
            byte[] buffer;

            if (stream.Length < bufferSize)
            {
                buffer = new byte[stream.Length];
            }
            else
            {
                buffer = new byte[bufferSize];
            }

            long dataToRead = stream.Length;
            long dataReaded = 0;
            int length = 0;

            stream.Position = 0;

            while (dataToRead > 0)
            {
                length = stream.Read(buffer, 0, buffer.Length);

                buffer.CopyTo(sbytes, dataReaded);

                dataReaded = dataReaded + length;
                dataToRead = dataToRead - length;

                if (dataToRead < bufferSize)
                {
                    buffer = new byte[dataToRead];
                }
                else
                {
                    buffer = new byte[bufferSize];
                }
            }

            return sbytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyBytes(byte[] from, byte[] to, long fromindex, long toindex)
        {
            if (to == null)
            {
                toindex = 0;

                to = new byte[from.Length - fromindex];
            }

            long dataToCopy = from.Length - fromindex;  // 需要拷贝的数据长度
            long spaceLeft = to.Length - toindex;   // 还剩下的空间

            long copylength = (dataToCopy < spaceLeft) ? dataToCopy : spaceLeft;

            for (int i = 0; i < copylength; i++)
            {
                to[toindex + i] = from[fromindex + i];
            }
        }

        #endregion
    }
}
