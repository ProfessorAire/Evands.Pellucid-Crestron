#region copyright
// <copyright file="ExampleCommands.cs" company="Christopher McNeely">
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

using EVS.Pellucid;
using EVS.Pellucid.Terminal.Commands;
using EVS.Pellucid.Terminal.Commands.Attributes;

namespace EVS.Pellucid.ProDemo
{
    /// <summary>
    /// Example commands.
    /// </summary>
    [Command("example", "Example console command.")]
    public class ExampleCommands : TerminalCommandBase
    {
        /// <summary>
        /// Echoes a message to the console.
        /// </summary>
        /// <param name="message">The message to echo.</param>
        /// <param name="caps">When true will print the message in all caps.</param>
        [Verb("echo", "Echoes the message provided back to the console.")]
        [Sample("example echo --message \"Just a test.\" -c", "Prints the message \"JUST A TEST.\" to the console.")]
        public void EchoMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps)
        {
            ConsoleBase.WriteCommandResponse(caps ? message.ToUpper() : message);
        }

        /// <summary>
        /// Echoes a message to the console.
        /// </summary>
        /// <param name="message">The message to echo.</param>
        /// <param name="caps">When true will print the message in all caps.</param>
        /// <param name="red">When true will print the message in red.</param>
        [Verb("echo", "Echoes the message provided back to the console in a specified color.")]
        [Sample("example echo --message \"Just a test.\" -cr", "Prints the message \"JUST A TEST.\" to the console in red.")]
        public void EchoRedMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps,
            [Flag("red", 'r', "When present prints the message in red.")] bool red)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightRed, caps ? message.ToUpper() : message);
        }

        /// <summary>
        /// Echoes a message to the console.
        /// </summary>
        /// <param name="message">The message to echo.</param>
        /// <param name="caps">When true will print the message in all caps.</param>
        /// <param name="blue">When true will print the message in blue.</param>
        [Verb("echo", "Echoes the message provided back to the console in a specified color.")]
        [Sample("example echo --message \"Just a test.\" -cb", "Prints the message \"JUST A TEST.\" to the console in blue.")]
        public void EchoBlueMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps,
            [Flag("blue", 'b', "When present prints the message in blue.")] bool blue)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightBlue, caps ? message.ToUpper() : message);
        }

        /// <summary>
        /// Echoes a message to the console in green.
        /// </summary>
        /// <param name="message">The message to echo.</param>
        /// <param name="caps">When true will print the message in all caps.</param>
        [Verb("echo-green", "Echoes the message provided back to the console in green.")]
        [Sample("example echo-green --message \"Just a test.\"", "Prints the message \"Just a test.\" to the console in green.")]
        public void EchoGreenMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightGreen, caps ? message.ToUpper() : message);
        }
    }
}