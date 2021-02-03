#region copyright
// <copyright file="FileLogger.cs" company="Christopher McNeely">
// The MIT License (MIT)
// Copyright (c) Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
#endregion

using Crestron.SimplSharp.CrestronIO;

namespace Silver.Diagnostics
{
    /// <summary>
    /// Provides simple methods for logging to a file in various locations.
    /// </summary>
    public static class FileLogger
    {
        /// <summary>
        /// Appends a message to the specified file in the NVRAM directory.
        /// </summary>
        /// <param name="data">The <see langword="string"/> data to append.</param>
        /// <param name="fileName">The name of the file to append to. If it doesn't exist, it will be created.</param>        
        public static void LogInfoToNvram(string data, string fileName)
        {
            try
            {
                if (!Directory.Exists("/nvram"))
                {
                    Debug.WriteErrorLine("FileLogger", "The NVRAM directory does not appear to exist. Please confirm the presence of this directory on the system. Logging to NVRAM will not work.");
                    Logger.LogError("FileLogger", "The NVRAM directory does not appear to exist. Please confirm the presence of this directory on the system. Logging to NVRAM will not work.");
                    return;
                }
            }
            catch (InvalidDirectoryLocationException ex)
            {
                Debug.WriteException("FileLogger", ex, "The NVRAM directory appears to be invalid. Unable to log the data '{0}' to the file '{1}' in NVRAM.", data, fileName);
                Logger.LogException("FileLogger", ex, "The NVRAM directory appears to be invalid. Unable to log the data '{0}' to the file '{1}' in NVRAM.", data, fileName);
                return;
            }

            fileName = Path.Combine("/ nvram", fileName);

            if (!File.Exists(fileName))
            {
                using (var fs = File.CreateText(fileName))
                {
                    fs.Write(data);
                }
            }
            else
            {
                using (var fs = File.AppendText(fileName))
                {
                    fs.Write(data);
                }
            }
        }
    }
}