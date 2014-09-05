using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PIC.Doc.Model;

namespace PIC.Doc
{
    public class OSDocModuleProvider : IDocModuleProvider
    {
        public DocFile Save(FileModel fileModel, DocDirectory docDir)
        {
            throw new NotImplementedException();
        }

        public byte[] Read(DocFile docFile)
        {
            throw new NotImplementedException();
        }

        public void Delete(DocFile file)
        {
            throw new NotImplementedException();
        }

        public TempFileData SaveTemp(FileModel fileModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteTemp(Guid fileID)
        {
            throw new NotImplementedException();
        }

        public FileModel GetTempFileModel(Guid fileID)
        {
            throw new NotImplementedException();
        }

        #region 支持方法

        private static string GetFilePath(DocDirectory docDir)
        {
            string path = String.Empty;

            if (docDir.Parent != null)
            {
                path = GetFilePath(docDir.Parent);

                path = Path.Combine(path, docDir.Code);
            }
            else
            {
                var model = docDir.GetDocModule();

                if (model != null)
                {
                    var paramDict = JsonHelper.GetObject<EasyDictionary>(model.Tag);
                    var rootPath = paramDict.Get<string>("Root", "");

                    path = rootPath;
                }
            }

            return path;
        }

        #endregion
    }
}
