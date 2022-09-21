using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.CrestronIO
{
    public static class File
    {
        public static System.IO.StreamWriter CreateText(string path)
        {
            return System.IO.File.CreateText(path);
        }

        public static string ReadToEndResult = string.Empty;

        public static string ReadToEnd(string path, Encoding encoding)
        {
            return ReadToEndResult;
        }

        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static System.IO.StreamWriter AppendText(string path)
        {
            return System.IO.File.AppendText(path);
        }
    }
}
