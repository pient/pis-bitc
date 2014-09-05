using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Doc.Model;

namespace PIC.Doc
{
    public interface IDocModuleProvider
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileModel"></param>
        /// <param name="docDir"></param>
        /// <returns></returns>
        DocFile Save(FileModel fileModel, DocDirectory docDir);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        void Delete(DocFile file);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="docFile"></param>
        /// <returns></returns>
        byte[] Read(DocFile docFile);

        /// <summary>
        /// 保存临时文件
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        TempFileData SaveTemp(FileModel fileModel);

        /// <summary>
        /// 删除临时文件
        /// </summary>
        /// <param name="fileID"></param>
        void DeleteTemp(Guid fileID);

        /// <summary>
        /// 由组信息获取文件列表
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        FileModel GetTempFileModel(Guid fileID);
    }
}
