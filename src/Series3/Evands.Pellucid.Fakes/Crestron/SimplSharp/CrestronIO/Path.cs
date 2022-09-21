using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp.CrestronIO
{
    public static class Path
    {
        public static string GetFileNameWithoutExtension(string file)
        {
            return System.IO.Path.GetFileNameWithoutExtension(file);
        }

        public static string Combine(string parta, string partb)
        {
            return System.IO.Path.Combine(parta, partb);
        }
    }
}
