using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PIC.Portal.Model;
using PIC.Doc.Model;

namespace PIC.Portal
{
    public class CacheService
    {
        #region 成员

        private static object lockobj = new object();

        public const int MAX_STREAM_CACHE_SIZE = 500000000; // 最大流Cache大小为500M
        public const int MAX_FILE_CACHE_SIZE = 500000000; // 最大流Cache大小为500M
        public const int MAX_SINGLE_FILE_CACHE_SIZE = 10000000; // 最大单个流Cache大小为10M
        public const int MAX_SINGLE_STREAM_CACHE_SIZE = 10000000; // 最大单个流Cache大小为10M

        protected PortalCache pc;
        protected FileCache fc;
        protected StreamCache sc;

        #endregion

        #region 构造函数

        protected CacheService()
        {
            pc = new PortalCache();
            fc = new FileCache();
            sc = new StreamCache();
        }

        #endregion

        #region 属性方法

        private static CacheService cacheserivce;

        private static CacheService Instance
        {
            get
            {
                if (cacheserivce == null)
                {
                    lock (lockobj)
                    {
                        if (cacheserivce == null)
                        {
                            cacheserivce = new CacheService();
                        }
                    }
                }

                return cacheserivce;
            }
        }

        public static byte[] SetFile(DocFile file)
        {
            Stream fstream = null;

            try
            {
                if (file.Size < MAX_SINGLE_FILE_CACHE_SIZE)
                {
                    var fdata = Doc.DocService.Read(file);

                    if (fdata.Length < MAX_SINGLE_FILE_CACHE_SIZE)
                    {
                        Instance.fc.Set(file.FileID.ToString(), fdata);

                        return GetFile(file.FileID.ToString());
                    }
                }

                throw new Exception("文件过大，无法添加到缓存。");
            }
            catch (Exception)
            {
            }
            finally
            {
                if (fstream != null)
                {
                    fstream.Close();
                    fstream.Dispose();
                }
            }

            return null;
        }

        public static void RemoveFile(string fileId)
        {
            Instance.fc.Remove(fileId);
        }

        public static byte[] GetFile(string fileId)
        {
            byte[] fbytes = Instance.fc.Get(fileId) as byte[];

            return fbytes;
        }

        public static void SetStream(string sid, byte[] sbytes)
        {
            Instance.sc.Set(sid, sbytes);
        }

        public static void RemoveStream(string sid)
        {
            Instance.sc.Remove(sid);
        }

        public static byte[] GetStream(string sid)
        {
            byte[] sbytes = Instance.sc.Get(sid) as byte[];

            return sbytes;
        }

        public static void Set(string path, object o)
        {
            Instance.pc.Set(path, o);
        }

        public static void Remove(string path)
        {
            Instance.pc.Remove(path);
        }

        public static object Get(string path)
        {
            return Instance.pc.Get(path);
        }

        #endregion
    }
}
