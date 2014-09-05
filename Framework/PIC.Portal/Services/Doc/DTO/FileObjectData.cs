using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal
{
    public class FileObjectData
    {
        public IList<FileObject> files { get; set; }

        public FileObjectData()
        {
            files = new List<FileObject>();
        }

        public string ToJsonString()
        {
            var str = JsonHelper.GetJsonString(this);

            return str;
        }

        public static FileObjectData FromJsonString(string str)
        {
            var fileData = JsonHelper.GetObject<FileObjectData>(str);

            return fileData;
        }
    }
}
