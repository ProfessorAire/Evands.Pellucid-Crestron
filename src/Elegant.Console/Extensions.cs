using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Elegant
{
    public static class Extensions
    {
        /// <summary>
        /// Dumps the specified object to the console.
        /// </summary>
        /// <param name="obj">The object to dump to the console.</param>
        public static void Dump(this object obj)
        {
            CrestronConsole.Print(Terminal.Formatting.Formatters.FormatObjectForConsole(obj));
        }
    }
}