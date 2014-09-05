using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace PIC.Security
{
    /// <summary>
    /// 此处定义的是DES加密,为了便于今后的管理和维护
    /// 请不要随便改动密码,或者改变了密码后请一定要
    /// 牢记先前的密码,否则将会照成不可预料的损失
    /// </summary>
    public class DESEncrypt
    {
        #region "member fields"

        private string iv = "B4443B4F"; // 8位
        private string key = "41AC8E49";    // 8位

        // private Encoding encoding = new UnicodeEncoding();
        private Encoding encoding = UTF8Encoding.UTF8;

        private DES des;

        #endregion

        /// <summary>
        /// 构造函数(internal, 以保护此函数不被滥用)
        /// </summary>
        internal DESEncrypt()
        {
            des = new DESCryptoServiceProvider();
        }

        #region "propertys"

        /// <summary>
        /// 设置加密密钥(8位)
        /// </summary>
        public string EncryptKey
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// 要加密字符的编码模式
        /// </summary>
        public Encoding EncodingMode
        {
            get { return this.encoding; }
            set { this.encoding = value; }
        }

        #endregion

        #region "methods"

        /// <summary>
        /// 加密字符串并返回加密后的结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string EncryptString(string str)
        {
            byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
            byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);//得到加密密钥
            byte[] toEncrypt = this.EncodingMode.GetBytes(str);//得到要加密的内容
            byte[] encrypted;

            ICryptoTransform encryptor = des.CreateEncryptor(keyb, ivb);
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();
            encrypted = msEncrypt.ToArray();
            csEncrypt.Close();
            msEncrypt.Close();

            string encryptedstr = Convert.ToBase64String(encrypted);

            return encryptedstr;
        }

        /// <summary>
        /// 加密指定的文件,如果成功返回True,否则false
        /// </summary>
        /// <param name="filePath">要加密的文件路径</param>
        /// <param name="outPath">加密后的文件输出路径</param>
        public void EncryptFile(string filePath, string outPath)
        {
            bool isExist = File.Exists(filePath);
            if (isExist)//如果存在
            {
                byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
                byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);

                //得到要加密文件的字节流
                FileStream fin = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fin, this.EncodingMode);
                string dataStr = reader.ReadToEnd();
                byte[] toEncrypt = this.EncodingMode.GetBytes(dataStr);
                fin.Close();

                FileStream fout = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                ICryptoTransform encryptor = des.CreateEncryptor(keyb, ivb);
                CryptoStream csEncrypt = new CryptoStream(fout, encryptor, CryptoStreamMode.Write);

                try
                {
                    //加密得到的文件字节流
                    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                    csEncrypt.FlushFinalBlock();
                }
                catch (Exception err)
                {
                    throw new ApplicationException(err.Message);
                }
                finally
                {
                    try
                    {
                        fout.Close();
                        csEncrypt.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("没有找到指定的文件");
            }
        }

        /// <summary>
        /// 文件加密函数的重载版本,如果不指定输出路径,
        /// 那么原来的文件将被加密后的文件覆盖
        /// </summary>
        /// <param name="filePath"></param>
        public void EncryptFile(string filePath)
        {
            this.EncryptFile(filePath, filePath);
        }

        /// <summary>
        /// 解密给定的字符串
        /// </summary>
        /// <param name="str">要解密的字符</param>
        /// <returns></returns>
        public string DecryptString(string str)
        {
            byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
            byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);
            // byte[] toDecrypt = this.EncodingMode.GetBytes(str);
            byte[] toDecrypt = Convert.FromBase64String(str);
            byte[] deCrypted = new byte[toDecrypt.Length];
            ICryptoTransform deCryptor = des.CreateDecryptor(keyb, ivb);
            MemoryStream msDecrypt = new MemoryStream(toDecrypt);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, deCryptor, CryptoStreamMode.Read);
            try
            {
                csDecrypt.Read(deCrypted, 0, deCrypted.Length);
            }
            catch (Exception err)
            {
                throw new ApplicationException(err.Message);
            }
            finally
            {
                try
                {
                    msDecrypt.Close();
                    csDecrypt.Close();
                }
                catch { ;}
            }

            string decryptedstr = this.EncodingMode.GetString(deCrypted);

            return decryptedstr;
        }

        /// <summary>
        /// 解密指定的文件
        /// </summary>
        /// <param name="filePath">要解密的文件路径</param>
        /// <param name="outPath">解密后的文件输出路径</param>
        public void DecryptFile(string filePath, string outPath)
        {
            bool isExist = File.Exists(filePath);
            if (isExist)//如果存在
            {
                byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
                byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);
                FileInfo file = new FileInfo(filePath);
                byte[] deCrypted = new byte[file.Length];

                //得到要解密文件的字节流
                FileStream fin = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                //解密文件
                try
                {
                    ICryptoTransform decryptor = des.CreateDecryptor(keyb, ivb);
                    CryptoStream csDecrypt = new CryptoStream(fin, decryptor, CryptoStreamMode.Read);
                    csDecrypt.Read(deCrypted, 0, deCrypted.Length);
                }
                catch (Exception err)
                {
                    throw new ApplicationException(err.Message);
                }
                finally
                {
                    try
                    {
                        fin.Close();
                    }
                    catch { ;}
                }

                FileStream fout = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                fout.Write(deCrypted, 0, deCrypted.Length);
                fout.Close();
            }
            else
            {
                throw new FileNotFoundException("指定的解密文件没有找到");
            }
        }

        /// <summary>
        /// 解密文件的重载版本,如果没有给出解密后文件的输出路径,
        /// 则解密后的文件将覆盖先前的文件
        /// </summary>
        /// <param name="filePath"></param>
        public void DecryptFile(string filePath)
        {
            this.DecryptFile(filePath, filePath);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DoEncryptString(string str)
        {
            DESEncrypt encrypt = new DESEncrypt();

            return encrypt.EncryptString(str);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string DoEncryptString(string str, string key)
        {
            DESEncrypt encrypt = new DESEncrypt();
            encrypt.EncryptKey = key;

            return encrypt.EncryptString(str);
        }

        /// <summary>
        /// 根据给定的MAC地址加密数据
        /// </summary>
        /// <returns></returns>
        internal static string DoEncryptStringByMAC(string str, string mac)
        {
            string key = GetEncryptKey(mac);

            return DoEncryptString(str, key);
        }

        /// <summary>
        /// 揭秘字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DoDecryptString(string str)
        {
            DESEncrypt encrypt = new DESEncrypt();

            return encrypt.DecryptString(str);
        }

        /// <summary>
        /// 揭秘字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string DoDecryptString(string str, string key)
        {
            DESEncrypt encrypt = new DESEncrypt();
            encrypt.EncryptKey = key;

            return encrypt.DecryptString(str);
        }

        /// <summary>
        /// 根据给定的MAC地址解密数据
        /// </summary>
        /// <returns></returns>
        internal static string DoDecryptStringByMAC(string str, string mac)
        {
            string key = GetEncryptKey(mac);

            return DoDecryptString(str, key);
        }

        /// <summary>
        /// 根据指定字符串获取加密Key
        /// </summary>
        /// <returns></returns>
        internal static string GetEncryptKey(string code)
        {
            return GetEncryptKey(code, 8);
        }

        /// <summary>
        /// 根据指定字符串获取加密Key
        /// </summary>
        /// <param name="code"></param>
        /// <param name="length">加密key长度(默认为8)</param>
        /// <returns></returns>
        internal static string GetEncryptKey(string code, int length)
        {
            // 替换所有非字符类型
            Regex regex = new Regex(@"\W");
            string mackey = regex.Replace(code, "");

            if (mackey.Length > length)
            {
                mackey = mackey.Substring(0, length);
            }
            else if (mackey.Length < length)
            {
                string addstr = StringHelper.GetStrByLength((length - mackey.Length), mackey);

                mackey = mackey + addstr;
            }

            mackey = mackey.ToUpper();

            return mackey;
        }

        #endregion
    }

    /// <summary>
    /// MD5加密类,注意经MD5加密过的信息是不能转换回原始数据的
    /// ,请不要在用户敏感的信息中使用此加密技术,比如用户的密码,
    /// 请尽量使用对称加密
    /// </summary>
    public class MD5Encrypt
    {
        public enum EncodingType
        {
            ASCII,
            BASE64
        }

        private MD5 md5;

        private MD5Encrypt()
        {
            md5 = new MD5CryptoServiceProvider();
        }

        private static MD5Encrypt instance;

        public static MD5Encrypt Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MD5Encrypt();
                }

                return instance;
            }
        }

        /// <summary>
        /// 从字符串中获取散列值
        /// </summary>
        /// <param name="str">要计算散列值的字符串</param>
        /// <returns></returns>
        public string GetMD5FromString(string str)
        {
            return GetMD5FromString(str, EncodingType.BASE64);    // 默认Base64编码
        }

        public string GetMD5FromString(string str, EncodingType encoding)
        {
            byte[] hashed = GetMD5BytesFromString(str);

            switch (encoding)
            {
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetString(hashed);
                default:
                    return Convert.ToBase64String(hashed);
            }
        }

        /// <summary>
        /// 根据文件来计算散列值
        /// </summary>
        /// <param name="filePath">要计算散列值的文件路径</param>
        /// <returns></returns>
        public string GetMD5FromFile(string filePath)
        {
            return GetMD5FromFile(filePath, EncodingType.BASE64);    // 默认Base64编码
        }

        /// <summary>
        /// 根据文件来计算散列值
        /// </summary>
        /// <param name="filePath">要计算散列值的文件路径</param>
        /// <returns></returns>
        public string GetMD5FromFile(string filePath, EncodingType encoding)
        {
            byte[] hashed = GetMD5BytesFromFile(filePath);

            switch (encoding)
            {
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetString(hashed);
                default:
                    return Convert.ToBase64String(hashed);
            }
        }

        /// <summary>
        /// 从字符串中获取散列值
        /// </summary>
        /// <param name="str">要计算散列值的字符串</param>
        /// <returns></returns>
        public byte[] GetMD5BytesFromString(string str)
        {
            if (str == null)
            {
                str = String.Empty;
            }

            byte[] toCompute = Encoding.Unicode.GetBytes(str);
            byte[] hashed = md5.ComputeHash(toCompute, 0, toCompute.Length);

            return hashed;
        }

        /// <summary>
        /// 根据文件来计算散列值
        /// </summary>
        /// <param name="filePath">要计算散列值的文件路径</param>
        /// <returns></returns>
        public byte[] GetMD5BytesFromFile(string filePath)
        {
            bool isExist = File.Exists(filePath);
            if (isExist)//如果文件存在
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(stream, Encoding.Unicode);
                string str = reader.ReadToEnd();
                byte[] toHash = Encoding.Unicode.GetBytes(str);
                byte[] hashed = md5.ComputeHash(toHash, 0, toHash.Length);
                stream.Close();

                return hashed;
            }
            else//文件不存在
            {
                throw new FileNotFoundException("指定的文件没有找到");
            }
        }
    }

    /// <summary>
    /// 用于数字签名的hash类
    /// </summary>
    public class MACTripleDESEncrypt
    {
        private MACTripleDES mact;
        private string _key = "ksn168ch";
        private byte[] _data = null;

        public MACTripleDESEncrypt()
        {
            mact = new MACTripleDES();
        }

        /// <summary>
        /// 获取或设置用于数字签名的密钥
        /// </summary>
        public string Key
        {
            get { return this._key; }
            set
            {
                int keyLength = value.Length;
                int[] keyAllowLengths = new int[] { 8, 16, 24 };
                bool isRight = false;

                foreach (int i in keyAllowLengths)
                {
                    if (keyLength == keyAllowLengths[i])
                    {
                        isRight = true;
                        break;
                    }
                }

                if (!isRight)
                {
                    throw new ApplicationException("用于数字签名的密钥长度必须是8,16,24值之一");
                }
                else
                {
                    this._key = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置用于数字签名的用户数据
        /// </summary>
        public byte[] Data
        {
            get { return this._data; }
            set { this._data = value; }
        }

        /// <summary>
        /// 得到签名后的hash值
        /// </summary>
        /// <returns></returns>
        public string GetHashValue()
        {
            if (this.Data == null)
            {
                throw new Exception(@"没有设置要进行数字签名的用户数据(property:Data)");
            }

            byte[] key = Encoding.ASCII.GetBytes(this.Key);
            this.mact.Key = key;
            byte[] hash_b = this.mact.ComputeHash(this.mact.ComputeHash(this.Data));
            return Encoding.ASCII.GetString(hash_b);
        }
    }
}

