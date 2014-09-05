using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PIC.Doc
{
    public class FileModelGroup
    {
        public FileModelGroup()
        {
            FileModels = new List<FileModel>();
        }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public IList<FileModel> FileModels { get; set; }
    }
}
