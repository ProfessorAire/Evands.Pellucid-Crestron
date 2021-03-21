#region copyright
// <copyright file="ConsoleCommands.cs" company="Christopher McNeely">
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

using EVS.Pellucid.Terminal.Commands;
using EVS.Pellucid.Terminal.Commands.Attributes;

namespace EVS.Pellucid
{
    /// <summary>
    /// Commands related to configuring the console.
    /// </summary>
    [Command("console", "Provides access to commands related to configuring console output.")]
    internal class ConsoleCommands : TerminalCommandBase
    {
        /// <summary>
        /// Enables console colorization.
        /// </summary>
        /// <param name="enable">Discard parameter.</param>        
        [Verb("color", "Enables or disables colorization of console output.")]
        [Sample("color --enable", "Enables the colorization of console output.")]
        public void Enable(
            [Flag("enable", "Enables the colorization of console output.")] bool enable)
        {
            Options.Instance.ColorizeConsoleOutput = true;
        }

        /// <summary>
        /// Disables console colorization.
        /// </summary>
        /// <param name="disable">Discard parameter.</param>        
        [Verb("color", "")]
        [Sample("color --disable", "Disables the colorization of console output.")]
        public void Disable(
            [Flag("disable", "Disables the colorization of console output.")] bool disable)
        {
            Options.Instance.ColorizeConsoleOutput = false;
        }
    }
}