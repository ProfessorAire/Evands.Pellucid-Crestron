#region copyright
// <copyright file="ProConsole.cs" company="Christopher McNeely">
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

namespace EVS.Pellucid
{
    /// <summary>
    /// Basic implementation of the <see cref="ConsoleBase"/>, for use by Pro applications.
    /// <para>
    /// In order to benefit from the provided <see cref="ConsoleCommands"/> you need to inherit
    /// from this class instead of <see cref="ConsoleBase"/> and call the <see cref="InitializeConsole"/>
    /// method.
    /// </para>
    /// </summary>
    public class ProConsole : ConsoleBase
    {
        /// <summary>
        /// Used for configuring console settings from the console itself.
        /// </summary>        
        private static readonly ConsoleCommands ConsoleCommands;

        /// <summary>
        /// Used for configuring debugging commands.
        /// </summary>
        private static readonly EVS.Pellucid.Diagnostics.DebuggingCommands DebuggingCommands;

        /// <summary>
        /// Tracks whether the console has been initialized.
        /// </summary>
        private static bool isInitialized;

        /// <summary>
        /// Initializes static members of the <see cref="ProConsole"/> class.
        /// </summary>        
        static ProConsole()
        {
            ConsoleCommands = new ConsoleCommands();
            DebuggingCommands = new EVS.Pellucid.Diagnostics.DebuggingCommands();
        }

        /// <summary>
        /// Initializes a set of commands for interacting with the Console to set various options.
        /// Pass in the names of the <see cref="Terminal.Commands.GlobalCommand"/>s you want to register these commands with.
        /// </summary>
        /// <param name="globalCommandNames">An array of <see langword="string"/>s that represent the names of the <see cref="Terminal.Commands.GlobalCommand"/> objects to register with.</param>
        public static void InitializeConsole(params string[] globalCommandNames)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                WriteLine();
                Diagnostics.Debug.WriteDebugLine("ProConsole", "Initializing global console commands.");
                for (var i = 0; i < globalCommandNames.Length; i++)
                {
                    Diagnostics.Debug.WriteProgressLine("ProConsole", "Registering console commands with global command '{0}'", globalCommandNames[i]);
                    var result = Terminal.Commands.Manager.Register(globalCommandNames[i], ConsoleCommands);
                    Diagnostics.Debug.WriteProgressLine("ProConsole", "Register result '{0}'.", result);

                    Diagnostics.Debug.WriteProgressLine("ProConsole", "Registering debugging commands with global command '{0}'", globalCommandNames[i]);
                    result = Terminal.Commands.Manager.Register(globalCommandNames[i], DebuggingCommands);
                    Diagnostics.Debug.WriteProgressLine("ProConsole", "Register result '{0}'.", result);
                }
            }
        }
    }
}