// <copyright file="ProConsole.cs">
// The MIT License
// Copyright © Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;

namespace Evands.Pellucid
{
    /// <summary>
    /// This class is deprecated. Use <see cref="ConsoleBase.InitializeDefaultConsoleCommands"/> directly instead.
    /// </summary>
    [Obsolete("ProConsole is no longer required. Use ConsoleBase.InitializeDefaultConsoleCommands instead.", true)]
    public class ProConsole : ConsoleBase
    {
        /// <summary>
        /// Initializes a set of commands for interacting with the Console to set various options.
        /// Pass in the names of the <see cref="Terminal.Commands.GlobalCommand"/>s you want to register these commands with.
        /// </summary>
        /// <param name="globalCommandNames">An array of <see langword="string"/>s that represent the names of the <see cref="Terminal.Commands.GlobalCommand"/> objects to register with.</param>
        [Obsolete("Use ConsoleBase.InitializeDefaultConsoleCommands instead.", true)]
        public static void InitializeConsole(params string[] globalCommandNames)
        {
            InitializeDefaultConsoleCommands(globalCommandNames);
        }
    }
}