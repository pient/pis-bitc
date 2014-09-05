using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PIC.Data;
using PIC.Doc.Model;

namespace PIC.Doc
{
    public class DbDocModuleProvider : IDocModuleProvider
    {
        [ActiveRecordTransaction]
        public DocFile Save(FileModel fileModel, DocDirectory docDir)
        {
            if (fileModel.Data != null && fileModel.Data.Length >=int.MaxValue 
                || fileModel.Stream != null && fileModel.Stream.Length >= int.MaxValue)
            {
                throw new Exception("文件长度过大！");
            }

            var docFile = new DocFile()
            {
                ModuleID = docDir.ModuleID,
                DirectoryID = docDir.DirectoryID,
                GroupID = fileModel.GroupID,
                Name = fileModel.FileName,
                ExtName = System.IO.Path.GetExtension(fileModel.FileName)
            };

            docFile.DoCreate();

            var fileData = new DocFileData()
            {
                FileID = docFile.FileID
            };

            if (fileModel.Data != null && fileModel.Data.Length > 0)
            {
                fileData.BinaryData = fileModel.Data;
            }
            else
            {
                fileData.BinaryData = CLRHelper.ReadStream(fileModel.Stream, (int)fileModel.Stream.Length);
            }

            fileData.DoCreate();

            docFile.DataID = fileData.DataID;
            docFile.Size = fileData.BinaryData.Length;

            docFile.DoUpdate();

            return docFile;
        }

        public void Delete(DocFile file)
        {
            file.DoDelete();
        }

        public byte[] Read(DocFile docFile)
        {
            var fileData = DocFileData.Find(docFile.DataID);

            return fileData.BinaryData;
        }

        public TempFileData SaveTemp(FileModel fileModel)
        {
            if (fileModel.Stream.Length >= int.MaxValue)
            {
                throw new Exception("文件长度过大！");
            }

            var file = new TempFileData()
            {
                GroupID = fileModel.GroupID ?? DataHelper.NewCombId(),
                FileName = fileModel.FileName
            };

            file.BinaryData = CLRHelper.ReadStream(fileModel.Stream, (int)fileModel.Stream.Length);

            file.DoCreate();

            return file;
        }

        public void DeleteTemp(Guid fileID)
        {
            var tmpFile = TempFileData.Find(fileID);

            tmpFile.DoDelete();
        }

        public FileModel GetTempFileModel(Guid fileID)
        {
            var tmpFile = TempFileData.Find(fileID);

            var fm = new FileModel()
            {
                FileName = tmpFile.FileName
            };

            fm.Data = tmpFile.BinaryData;
            fm.ContentLength = tmpFile.BinaryData.Length;

            return fm;
        }
    }
}
