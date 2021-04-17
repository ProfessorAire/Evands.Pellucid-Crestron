using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.CrestronIO
{
    public class DirectoryInfo
    {
        public System.IO.DirectoryInfo internalInfo;

        public DirectoryInfo(string path)
        {
            internalInfo = new System.IO.DirectoryInfo(path);
        }

        public DirectoryInfo(System.IO.DirectoryInfo info)
        {
            internalInfo = info;
        }
    }
}
