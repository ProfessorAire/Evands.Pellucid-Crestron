using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.CrestronIO
{
    public static class Directory
    {
        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static DirectoryInfo CreateDirectory(string path)
        {
            return new DirectoryInfo(System.IO.Directory.CreateDirectory(path));
        }
    }
}
