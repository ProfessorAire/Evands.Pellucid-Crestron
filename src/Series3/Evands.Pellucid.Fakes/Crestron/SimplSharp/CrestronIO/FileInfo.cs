using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.CrestronIO
{
    public class FileInfo
    {
        public System.IO.FileInfo internalInfo;

        public FileInfo(string path)
        {
            internalInfo = new System.IO.FileInfo(path);
        }

        public string DirectoryName
        {
            get { return internalInfo.DirectoryName; }
        }

        public string Extension
        {
            get { return internalInfo.Extension; }
        }
    }
}
