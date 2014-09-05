using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace PIC.Common
{
    public class ServiceHelper
    {
        /// <summary>
        /// 序列化对象为Base64类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToBase64(object obj)
        {
            return Convert.ToBase64String(SerializeToBytes(obj));
        }

        /// <summary>
        /// 序列化对象为byte数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        /// <summary>
        /// 从byte数组反序列化成指定类型对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeFromBytes<T>(byte[] bytes)
        {
            object obj = DeserializeFromBytes(bytes);
            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        /// <summary>
        /// 从byte数组反序列化对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }

            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new UBinder();
            MemoryStream ms = new MemoryStream(bytes);

            ms.Position = 0;

            return formatter.Deserialize(ms);
        }

        /// <summary>
        /// 从base64字符串反序列化成指定类型对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeFromBase64<T>(string str)
        {
            object obj = DeserializeFromBase64(str);

            if (obj == null)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        /// <summary>
        /// 从Base64反序列化对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeFromBase64(string str)
        {
            byte[] bs = Convert.FromBase64String(str);

            return DeserializeFromBytes(bs);
        }

        public class UBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                return ass.GetType(typeName);
            }
        }
    }


}
