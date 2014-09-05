using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PIC.Doc
{
    public class FileModel
    {
        public Guid? GroupID { get; set; }

        public string FileName { get; set; }

        public int? ContentLength { get; set; }

        public string ContentType { get; set; }

        public Stream Stream { get; set; }

        public byte[] Data { get; set; }
    }
}
