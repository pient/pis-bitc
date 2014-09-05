using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using PIC.Data;
using PIC.Doc.Model;
using Castle.ActiveRecord;
using PIC.Portal;

namespace PIC.Doc
{
    /// <summary>
    /// 文档服务
    /// </summary>
    public class DocService
    {
        #region Consts

        public const string TempDocDir = "TEMP";

        #endregion

        #region 构造函数

        // 单体
        private static readonly DocService docserivce = new DocService();

        private static DocService Instance
        {
            get
            {
                return docserivce;
            }
        }

        protected DocService()
        {
        }

        #endregion

        #region 静态方法

        #region 读取操作

        public static byte[] Read(DocFile docFile)
        {
            var mdl = docFile.GetDocModule();
            var mdlProvider = GetModuleProvider(mdl);

            var data = mdlProvider.Read(docFile);

            return data;
        }

        /// <summary>
        /// 获取FileModel
        /// </summary>
        /// <param name="docFile"></param>
        /// <returns></returns>
        public static FileModel GetFileModel(DocFile docFile)
        {
            var provider = GetModuleProvider(docFile);

            var fm = new FileModel()
            {
                FileName = docFile.Name,
                ContentLength = (int)docFile.Size.Value,
                GroupID = docFile.GroupID
            };

            fm.Data = provider.Read(docFile);

            return fm;
        }

        #endregion

        #region 保存操作

        /// <summary>
        /// 保存数据到指定文件夹下
        /// </summary>
        /// <param name="fileCollection"></param>
        /// <param name="dirCode"></param>
        /// <returns></returns>
        public static IList<DocFile> Save(HttpFileCollection fileCollection, string dirCode)
        {
            var docDir = DocDirectory.Get(dirCode);

            return Save(fileCollection, docDir);
        }

        /// <summary>
        /// 保存数据到指定文件夹下
        /// </summary>
        /// <param name="fileCollection"></param>
        /// <param name="docDir"></param>
        /// <returns></returns>
        public static IList<DocFile> Save(HttpFileCollection fileCollection, DocDirectory docDir)
        {
            var fileGroup = new FileModelGroup();

            for (int i = 0; i < fileCollection.Count; i++)
            {
                var f = fileCollection.Get(i);

                if (f.InputStream.Length > 0 && f.ContentLength > 0)
                {
                    fileGroup.FileModels.Add(new FileModel()
                    {
                        FileName = f.FileName,
                        ContentLength = f.ContentLength,
                        ContentType = f.ContentType,
                        Stream = f.InputStream
                    });
                }
            }

            return Save(fileGroup, docDir);
        }

        public static IList<DocFile> Save(FileModelGroup fileGroup, string dirCode)
        {
            var docDir = DocDirectory.Get(dirCode);

            return Save(fileGroup, docDir);
        }

        /// <summary>
        /// 保存提交数据
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="directoryId"></param>
        /// <returns></returns>
        [ActiveRecordTransaction]
        public static IList<DocFile> Save(FileModelGroup fileGroup, DocDirectory docDir)
        {
            var fileList = new List<DocFile>();

            var modelProvider = GetModuleProvider(docDir);

            foreach (var fm in fileGroup.FileModels)
            {
                var docFile = Save(fm, docDir, modelProvider);

                fileList.Add(docFile);
            }

            return fileList;
        }

        public static DocFile Save(FileModel fileModel, string dirCode, IDocModuleProvider provider = null)
        {
            var docDir = DocDirectory.Get(dirCode);

            return Save(fileModel, docDir, provider);
        }

        public static DocFile Save(FileModel fileModel, DocDirectory docDir, IDocModuleProvider provider = null)
        {
            if (provider == null)
            {
                provider = GetModuleProvider(docDir);
            }

            var docFile = provider.Save(fileModel, docDir);

            return docFile;
        }

        [ActiveRecordTransaction]
        public static IList<TempFileData> SaveTemp(HttpFileCollection fileCollection, Guid? groupID = null)
        {
            var fileList = new List<TempFileData>();

            var modelProvider = GetTempModuleProvider();  // 临时文件，默认数据库供应

            if (groupID == null)
            {
                groupID = DataHelper.NewCombId();
            }

            for (int i = 0; i < fileCollection.Count; i++)
            {
                var f = fileCollection.Get(i);

                if (f.InputStream.Length > 0 && f.ContentLength > 0)
                {
                    var fm = new FileModel()
                    {
                        GroupID = groupID,
                        FileName = f.FileName,
                        ContentLength = f.ContentLength,
                        ContentType = f.ContentType,
                        Stream = f.InputStream
                    };

                    var fileData = modelProvider.SaveTemp(fm);

                    fileList.Add(fileData);
                }
            }

            return fileList;
        }

        #endregion

        #region 归档操作

        public static DocFile Archive(DocFile docFile, string dirCode)
        {
            var docDir = DocDirectory.Get(dirCode);

            return Archive(docFile, docDir);
        }

        /// <summary>
        /// 归档临时数据到指定文件夹下
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="docDir"></param>
        public static DocFile Archive(DocFile docFile, DocDirectory docDir)
        {
            var fm = GetFileModel(docFile);

            var newFile = Save(fm, docDir);

            return newFile;
        }

        public static DocFile ArchiveTemp(Guid fileID, string dirCode)
        {
            var docDir = DocDirectory.Get(dirCode);

            return ArchiveTemp(fileID, docDir);
        }

        /// <summary>
        /// 归档临时数据到指定文件夹下
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="docDir"></param>
        public static DocFile ArchiveTemp(Guid fileID, DocDirectory docDir)
        {
            var tmpProvider = GetTempModuleProvider();
            var tfm = tmpProvider.GetTempFileModel(fileID);

            var docFile = Save(tfm, docDir);

            return docFile;
        }

        public static FileObjectData ArchiveFileObjectData(FileObjectData fileObjData, string dirCode, bool? onlyTemp = true)
        {
            var docDir = DocDirectory.Get(dirCode);

            return ArchiveFileObjectData(fileObjData, docDir, onlyTemp);
        }

        /// <summary>
        /// 归档文件对象，一般用于归档上传文件
        /// </summary>
        /// <param name="docDir"></param>
        /// <param name="fileObjData"></param>
        /// <returns></returns>
        public static FileObjectData ArchiveFileObjectData(FileObjectData fileObjData, DocDirectory docDir, bool? onlyTemp = true)
        {
            if (fileObjData.files != null && fileObjData.files.Count > 0)
            {
                var newFileObjData = new FileObjectData();

                foreach (var fd in fileObjData.files)
                {
                    if (onlyTemp == true && fd.istemp == false)
                    {
                        newFileObjData.files.Add(fd);
                    }
                    else
                    {
                        var f = ArchiveFileObject(fd, docDir);

                        if (f != null)
                        {
                            newFileObjData.files.Add(f);
                        }
                    }
                }

                return newFileObjData;
            }

            return null;
        }

        public static FileObject ArchiveFileObject(FileObject fileObj, string dirCode, bool? onlyTemp = true)
        {
            var docDir = DocDirectory.Get(dirCode);

            return ArchiveFileObject(fileObj, docDir, onlyTemp);
        }

        /// <summary>
        /// 归档文件对象，一般用于归档上传文件
        /// </summary>
        /// <param name="docDir"></param>
        /// <param name="fileObj"></param>
        /// <returns></returns>
        public static FileObject ArchiveFileObject(FileObject fileObj, DocDirectory docDir, bool? onlyTemp = true)
        {
            if (onlyTemp == true && fileObj.istemp == false)
            {
                return null;
            }

            var fileId = fileObj.id.ToGuid();

            if (!fileId.IsNullOrEmpty())
            {
                DocFile docFile = null;

                if (fileObj.istemp == false)
                {
                    var orgDocFile = DocFile.Find(fileId);

                    if (orgDocFile != null)
                    {
                        docFile = Archive(orgDocFile, docDir);
                    }
                }
                else
                {
                    docFile = ArchiveTemp(fileId.Value, docDir);
                }

                if (docFile != null)
                {
                    var newFileObj = new FileObject()
                    {
                        id = docFile.FileID.ToString(),
                        name = docFile.Name,
                        istemp = false
                    };

                    return newFileObj;
                }
            }

            return null;
        }

        #endregion

        #region 删除操作

        public static void Delete(params DocFile[] docFiles)
        {
            foreach (var f in docFiles)
            {
                var mdl = f.Directory.GetDocModule();
                var mdlProvider = GetModuleProvider(mdl);

                mdlProvider.Delete(f);
            }
        }

        public static void Delete(FileObjectData fileObjData)
        {
            if (fileObjData.files != null && fileObjData.files.Count > 0)
            {
                IDocModuleProvider tmpProvider = null;

                if (fileObjData.files.FirstOrDefault(f => f.istemp == true) != null)
                {
                    tmpProvider = GetTempModuleProvider();
                }

                foreach (var fd in fileObjData.files)
                {
                    Delete(fd, tmpProvider);
                }
            }
        }

        public static void Delete(FileObject fileObj, IDocModuleProvider tmpProvider = null)
        {
            if (String.IsNullOrEmpty(fileObj.id))
            {
                return;
            }

            var fileId = fileObj.id.ToGuid();

            if (fileId.IsNullOrEmpty())
            {
                return;
            }

            if (fileObj.istemp == true)
            {
                if (tmpProvider == null)
                {
                    tmpProvider = GetTempModuleProvider();
                }

                tmpProvider.DeleteTemp(fileId.Value);
            }
            else
            {
                var docFile = DocFile.Find(fileId.Value);

                if (docFile != null)
                {
                    Delete(docFile);
                }
            }
        }

        #endregion

        #endregion

        #region 支持方法

        /// <summary>
        /// 获取文件对象数据字符串
        /// </summary>
        /// <param name="fileObjData"></param>
        /// <returns></returns>
        public static string GetFileObjectDataStr(FileObjectData fileObjData)
        {
            if (fileObjData != null)
            {
                return JsonHelper.GetJsonString(fileObjData);
            }

            return null;
        }

        /// <summary>
        /// 获取文件对象数据
        /// </summary>
        /// <param name="fileObjDataStr"></param>
        /// <returns></returns>
        public static FileObjectData GetFileObjectData(string fileObjDataStr)
        {
            if (!String.IsNullOrEmpty(fileObjDataStr))
            {
                var fileObjData = JsonHelper.GetObject<FileObjectData>(fileObjDataStr);

                return fileObjData;
            }

            return null;
        }

        /// <summary>
        /// 获取目录文件模块供应者 
        /// </summary>
        /// <param name="docDir"></param>
        /// <returns></returns>
        private static IDocModuleProvider GetModuleProvider(DocDirectory docDir)
        {
            var model = docDir.GetDocModule();
            var modelProvider = GetModuleProvider(model);

            return modelProvider;
        }

        /// <summary>
        /// 获取文件模块供应者 
        /// </summary>
        /// <param name="docDir"></param>
        /// <returns></returns>
        private static IDocModuleProvider GetModuleProvider(DocFile docFile)
        {
            var model = docFile.GetDocModule();
            var modelProvider = GetModuleProvider(model);

            return modelProvider;
        }

        /// <summary>
        /// 获取临时文件模块供应者
        /// </summary>
        /// <param name="docModule"></param>
        /// <returns></returns>
        private static IDocModuleProvider GetTempModuleProvider()
        {
            var moduleProvider = new DbDocModuleProvider();

            return moduleProvider;
        }

        /// <summary>
        /// 获取模块供应者
        /// </summary>
        /// <param name="docModule"></param>
        /// <returns></returns>
        private static IDocModuleProvider GetModuleProvider(DocModule docModule)
        {
            IDocModuleProvider moduleProvider = null;

            if (String.IsNullOrEmpty(docModule.Provider))
            {
                moduleProvider = CLRHelper.CreateInstance<IDocModuleProvider>(docModule.Provider);
            }

            if (moduleProvider == null)
            {
                moduleProvider = new DbDocModuleProvider();
            }

            return moduleProvider;
        }

        #endregion
    }
}
