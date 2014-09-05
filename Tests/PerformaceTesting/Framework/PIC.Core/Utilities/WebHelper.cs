using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using PIC.Common.Authentication;

namespace PIC.Common
{
    /// <summary>
    /// 导出文件类型
    /// </summary>
    public enum ExpFileTypeEnum
    {
        Excel,
        Unknown
    }

    public class WebHelper
    {
        #region 获取系统表标识

        /// <summary>
        /// 由系统Principal获取SysIdentity
        /// </summary>
        /// <param name="princ"></param>
        /// <returns></returns>
        public static SysIdentity GetSysIdentity(IPrincipal princ)
        {
            SysPrincipal p = princ as SysPrincipal;

            if (p == null)
            {
                return null;
            }

            if (princ == null)
            {
                return null;
            }
            else
            {
                return p.Identity as SysIdentity;
            }
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="text"></param>
        public static HttpResponse ResponseTextFile(string text, string fileName)
        {
            HttpResponse response = GetAttachmentResponse(fileName);
            response.Write(text);

            response.Flush();

            return response;
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        public static HttpResponse ResponseFile(string filePath, string fileName = null)
        {
            FileStream iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (String.IsNullOrEmpty(fileName))
            {
                fileName = Path.GetFileName(filePath);
            }

            HttpResponse resp = ResponseFile(iStream, fileName);

            if (iStream != null)
            {
                iStream.Close();
            }

            return resp;
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        public static HttpResponse ResponseFile(FileInfo finfo)
        {
            FileStream iStream = new FileStream(finfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

            HttpResponse resp = ResponseFile(iStream, finfo.Name);

            if (iStream != null)
            {
                iStream.Close();
            }

            return resp;
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        public static HttpResponse ResponseFile(Stream stream, string fileName)
        {
            byte[] buffer = new Byte[10000];
            int length;
            long dataToRead = stream.Length;

            if (HttpContext.Current.Response.IsClientConnected)
            {
                HttpResponse response = GetAttachmentResponse(fileName);

                while (dataToRead > 0)
                {
                    if (response.IsClientConnected)
                    {
                        length = stream.Read(buffer, 0, 10000);
                        response.OutputStream.Write(buffer, 0, length);
                        response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }

                return response;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        public static HttpResponse ResponseFile(byte[] bytes, string fileName)
        {
            byte[] buffer;

            if (bytes.Length < 10000)
            {
                buffer = new byte[bytes.Length];
            }
            else
            {
                buffer = new byte[10000];
            }

            long dataToRead = bytes.Length;
            long dataReaded = 0;

            if (HttpContext.Current.Response.IsClientConnected)
            {
                HttpResponse response = GetAttachmentResponse(fileName);

                while (dataToRead > 0)
                {
                    if (response.IsClientConnected)
                    {
                        CLRHelper.CopyBytes(bytes, buffer, dataReaded, 0);

                        response.OutputStream.Write(buffer, 0, buffer.Length);
                        response.Flush();

                        dataToRead = dataToRead - buffer.Length;
                        dataReaded = dataReaded + buffer.Length;

                        if (dataToRead < 10000)
                        {
                            buffer = new byte[dataToRead];
                        }
                        else
                        {
                            buffer = new byte[10000];
                        }
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }

                return response;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置文件输出HttpResponse
        /// </summary>
        /// <param name="response"></param>
        public static HttpResponse GetAttachmentResponse(string fileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();

            Encoding encoding = Encoding.GetEncoding("UTF-8");

            return GetAttachmentResponse(response, fileName, encoding);
        }

        /// <summary>
        /// 设置文件输出HttpResponse
        /// </summary>
        /// <param name="response"></param>
        public static HttpResponse GetAttachmentResponse(HttpResponse response, string fileName)
        {
            Encoding encoding = Encoding.GetEncoding("UTF-8");

            return GetAttachmentResponse(response, fileName, encoding);
        }

        /// <summary>
        /// 设置文件输出HttpResponse
        /// </summary>
        /// <param name="response"></param>
        public static HttpResponse GetAttachmentResponse(HttpResponse response, string fileName, Encoding encoding)
        {
            response.ContentType = "application/octet-stream";
            response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, encoding));

            return response;
        }

        #endregion

        #region Ext Js 实用

        #region Ext Js导出

        /// <summary>
        /// 导出ExtGrid为指定类型文件
        /// </summary>
        /// <param name="docType"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void ExportExtGrid(string fileName, string content, ExpFileTypeEnum fileType)
        {
            switch (fileType)
            {
                case ExpFileTypeEnum.Excel:
                    ExportExtGridToExcel(fileName, content);
                    break;
            }
        }

        /// <summary>
        /// 导出ExtGrid为Excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static void ExportExtGridToExcel(string fileName, string content)
        {
            fileName = String.IsNullOrEmpty(fileName) ? "export.xls" : fileName;

            HttpContext.Current.Response.Write("&amp;lt;script&amp;gt;document.close();&amp;lt;/script&amp;gt;");
            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            HttpContext.Current.Response.Charset = "";

            System.IO.StringWriter tmpSW = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter tmpHTW = new System.Web.UI.HtmlTextWriter(tmpSW);
            tmpHTW.WriteLine(content);

            HttpContext.Current.Response.Write(tmpSW.ToString());
            HttpContext.Current.Response.End();
        }

        #endregion

        #region Ext Js 树节点


        /// <summary>
        /// ExtTree内容集合
        /// </summary>
        public class ExtTreeNodeCollection : List<ExtTreeNode>
        {
            public ExtTreeNodeCollection()
            {
            }

            public ExtTreeNodeCollection(IEnumerable<ExtTreeNode> nodes)
                : base(nodes)
            {
            }

            public ExtTreeNodeCollection(int capacity)
                : base(capacity)
            {
            }
        }

        /// <summary>
        /// ExtTree节点
        /// </summary>
        public class ExtTreeNode : Hashtable
        {
            public string text
            {
                get { return this["text"] == null ? string.Empty : this["text"].ToString(); }
                set { this["text"] = value; }
            }

            public bool? leaf
            {
                get
                {
                    try
                    {
                        return (bool?)this["leaf"];
                    }
                    catch
                    {
                        return null;
                    }
                }
                set { this["leaf"] = value; }
            }
        }

        #endregion

        #endregion

        #region IIS相关

        /// <summary>
        /// 获取基应用名
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationBaseName()
        {
            string appBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.TrimEnd('\\');
            appBase = appBase.Substring((appBase.LastIndexOf('\\') + 1));

            return appBase;
        }

        /// <summary>
        /// 获取认证包
        /// </summary>
        /// <returns></returns>
        public static AuthPackage GetAuthPackage(string loginName, string pwd, bool passwordEncrypted)
        {
            AuthPackage authPackage = new AuthPackage { LoginName = loginName, Password = pwd, PasswordEncrypted = passwordEncrypted };

            if (HttpContext.Current != null)
            {
                authPackage.IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return authPackage;
        }


        #endregion

        #region 其他实用方法 

        /// <summary>
        /// 是否外部链接 
        /// </summary>
        /// <returns></returns>
        public static bool IsOuterLink()
        {
            string http_referer = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            string http_host = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];

            return (http_referer == null || !http_referer.Contains(http_host));
        }

        /// <summary>
        /// 禁用页面缓存
        /// </summary>
        public static void DisablePageCache()
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
        }

        #endregion
    }
}
