using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIC.Common;
using PIC.Doc;
using PIC.Doc.Model;
using NHibernate.Criterion;
using System.IO;

namespace PIC.Portal.Web.Modules.Common.Doc
{
    /// <summary>
    /// Summary description for Download1
    /// </summary>
    public class File : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];

            switch (type)
            {
                case "local":   // 本地文件
                    ResponseLocalFile(context);
                    break;
                case "db":  // 数据库文件
                default:
                    ResponseDbFile(context);
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 支持方法

        /// <summary>
        /// 回复本地文件
        /// </summary>
        /// <param name="context"></param>
        private void ResponseLocalFile(HttpContext context)
        {
            string localcode = context.Request["code"];
            string subpath = context.Request["subpath"];

            string filePath = null;

            if (!String.IsNullOrEmpty(subpath))
            {
                switch (localcode)
                {
                    case "flow":
                        filePath = Workflow.WfHelper.GetFlowFilePath(subpath);
                        break;
                }
            }

            if (System.IO.File.Exists(filePath))
            {
                WebHelper.ResponseFile(filePath);
            }
            else
            {
                HttpContext.Current.Response.Write("文件不存在");
            }
        }

        /// <summary>
        /// 回复文件
        /// </summary>
        /// <param name="context"></param>
        private void ResponseDbFile(HttpContext context)
        {
            string fileIdStr = context.Request["id"];

            if (String.IsNullOrEmpty(fileIdStr))
            {
                fileIdStr = context.Request["fid"];
            }

            string istempStr = context.Request["istemp"];

            bool? istemp = istempStr.ToBoolean();

            if (!String.IsNullOrEmpty(fileIdStr))
            {
                Guid? fileId = fileIdStr.ToGuid();

                if (!fileId.IsNullOrEmpty())
                {
                    byte[] bytesData = null;

                    if (istemp == true)
                    {
                        var tmpFile = TempFileData.FindFirst(Expression.Eq(TempFileData.Prop_DataID, fileId));

                        if (tmpFile == null)
                        {
                            HttpContext.Current.Response.Write("文件不存在");
                        }
                        else
                        {
                            bytesData = tmpFile.BinaryData;
                            WebHelper.ResponseFile(bytesData, tmpFile.FileName);
                        }
                    }
                    else
                    {
                        var docFile = DocFile.FindFirst(Expression.Eq(DocFile.Prop_FileID, fileId));

                        if (docFile == null)
                        {
                            HttpContext.Current.Response.Write("文件不存在");
                        }
                        else
                        {
                            bytesData = DocService.Read(docFile);
                            WebHelper.ResponseFile(bytesData, docFile.Name);
                        }
                    }
                }
            }
        }

        #endregion
    }
}