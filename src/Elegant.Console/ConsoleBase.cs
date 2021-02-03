#region copyright
// <copyright file="ConsoleBase.cs" company="Christopher McNeely">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using Elegant.Terminal;
using Elegant.Terminal.Formatting;

namespace Elegant
{
    /// <summary>
    /// Provides methods for interacting with the Crestron Console in a more simplified manner.
    /// <para>
    /// Generally it is recommended to create your own static Console class in the root of your project namespace,
    /// which inherits from this class. This will allow you to call these methods using the format Console.MethodName,
    /// since a Console class in your root namespace will supersede the <see cref="System.Console"/> class.
    /// </para>
    /// <para>
    /// Alternatively you can use the <see cref="BasicConsole"/> class, which is an implementation of this class.
    /// </para>
    /// </summary>
    public abstract class ConsoleBase
    {
        /// <summary>
        /// Backing field for the <see cref="ColorizeOutput"/> property.
        /// </summary>
        private static bool colorizeOutput = true;

        /// <summary>
        /// Initializes static members of the <see cref="ConsoleBase"/> class.
        /// </summary>
        static ConsoleBase()
        {
            var result = false;
            if (Crestron.SimplSharp.CrestronDataStore.CrestronDataStoreStatic.GetLocalBoolValue(
                string.Format("{0}.Elegant.ColorizeConsoleOutput", InitialParametersClass.ApplicationNumber),
                out result) == Crestron.SimplSharp.CrestronDataStore.CrestronDataStore.CDS_ERROR.CDS_SUCCESS)
            {
                ColorizeOutput = result;
            }
            else
            {
                ColorizeOutput = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to colorize the console output.
        /// </summary>
        public static bool ColorizeOutput
        {
            get
            {
                return colorizeOutput;
            }

            set
            {
                colorizeOutput = value;
                Crestron.SimplSharp.CrestronDataStore.CrestronDataStoreStatic.SetLocalBoolValue(
                    string.Format("{0}.Elegant.ColorizeConsoleOutput", InitialParametersClass.ApplicationNumber),
                    value);
            }
        }

        /// <summary>
        /// Writes colored text to the console output.
        /// </summary>
        /// <param name="colors">The color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(ColorFormat colors, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(colors, message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.Print("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
        }

        /// <summary>
        /// Writes colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">The background color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(ColorCode foreground, ColorCode background, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(foreground, background, message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.Print("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
        }

        /// <summary>
        /// Writes colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(ColorCode foreground, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(foreground, message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.Print("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
        }

        /// <summary>
        /// Writes uncolored text to the console output.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(string message, params object[] args)
        {
            message = string.Format(message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.Print("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
        }

        /// <summary>
        /// Writes a line of colored text to the console output.
        /// </summary>
        /// <param name="colors">The color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(ColorFormat colors, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(colors, message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.PrintLine("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
            else
            {
                CrestronConsole.PrintLine(string.Empty);
            }
        }

        /// <summary>
        /// Writes a line of colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">The background color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(ColorCode foreground, ColorCode background, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(foreground, background, message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.PrintLine("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
            else
            {
                CrestronConsole.PrintLine(string.Empty);
            }
        }

        /// <summary>
        /// Writes a line of colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(ColorCode foreground, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(foreground, message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.PrintLine("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
            else
            {
                CrestronConsole.PrintLine(string.Empty);
            }
        }

        /// <summary>
        /// Writes a line of uncolored text to the console output.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(string message, params object[] args)
        {
            message = string.Format(message, args);
            if (!string.IsNullOrEmpty(message))
            {
                CrestronConsole.PrintLine("[{0}] {1}", InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), message);
            }
            else
            {
                CrestronConsole.PrintLine(string.Empty);
            }
        }

        /// <summary>
        /// Writes an empty line to the console output.
        /// </summary>
        public static void WriteLine()
        {
            CrestronConsole.PrintLine(string.Empty);
        }

        /// <summary>
        /// Writes a line to the console using the <see cref="Colors.Debug"/> <see cref="ColorFormat"/> as the color source.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteDebug(string message, params object[] args)
        {
            WriteLine(Colors.Debug, message, args);
        }

        /// <summary>
        /// Writes a line to the console using the <see cref="Colors.Error"/> <see cref="ColorFormat"/> as the color source.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteError(string message, params object[] args)
        {
            WriteLine(Colors.Error, message, args);
        }

        /// <summary>
        /// Writes a line to the console using the <see cref="Colors.Progress"/> <see cref="ColorFormat"/> as the color source.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteProgress(string message, params object[] args)
        {
            WriteLine(Colors.Progress, message, args);
        }

        /// <summary>
        /// Writes a message to the console using the <see cref="Colors.Progress"/> <see cref="ColorFormat"/> as the color source.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteProgressStep(string message, params object[] args)
        {
            Write(Colors.Progress, message, args);
        }

        /// <summary>
        /// Writes a line to the console using the <see cref="Colors.Subtle"/> <see cref="ColorFormat"/> as the color source.
        /// </summary>
        /// <param name="message">The message to write to the console.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteSubtle(string message, params object[] args)
        {
            WriteLine(Colors.Subtle, message, args);
        }

        /// <summary>
        /// Writes a command response to the console using the colors specified.
        /// </summary>
        /// <param name="colors">The <see cref="ColorFormat"/> to print the response using.</param>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(ColorFormat colors, string message, params object[] args)
        {
            CrestronConsole.ConsoleCommandResponse(Formatters.GetColorFormattedString(colors, message, args));
        }

        /// <summary>
        /// Writes a command response to the console using the colors specified.
        /// </summary>
        /// <param name="foreground">The <see cref="TerminalColor"/> to print the response foreground using.</param>
        /// <param name="background">The <see cref="TerminalColor"/> to print the response background using.</param>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(ColorCode foreground, ColorCode background, string message, params object[] args)
        {
            CrestronConsole.ConsoleCommandResponse(Formatters.GetColorFormattedString(foreground, background, message, args));
        }

        /// <summary>
        /// Writes a command response to the console using the colors specified.
        /// </summary>
        /// <param name="foreground">The <see cref="TerminalColor"/> to print the response foreground using.</param>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(ColorCode foreground, string message, params object[] args)
        {
            CrestronConsole.ConsoleCommandResponse(Formatters.GetColorFormattedString(foreground, ColorCode.None, message, args));
        }

        /// <summary>
        /// Writes a command response to the console.
        /// </summary>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(string message, params object[] args)
        {
            CrestronConsole.ConsoleCommandResponse(message, args);
        }

        /// <summary>
        /// Attempts to remove the specified console command.
        /// </summary>
        /// <param name="commandName">The command to remove.</param>
        public static void RemoveConsoleCommand(string commandName)
        {
            try
            {
                CrestronConsole.RemoveConsoleCommand(commandName);
            }
            catch (Exception ex)
            {
                WriteError("Unable to remove the command '{0}', an exception occurred.\n{1}", commandName, ex.ToString());
            }
        }

        /// <summary>
        /// Provides access to standard console colors as <see cref="ColorFormat"/> objects.
        /// </summary>
        public static class Colors
        {
            /// <summary>
            /// Bright green foreground with no background.
            /// </summary>
            public static readonly ColorFormat BrightGreen;

            /// <summary>
            /// Bright red foreground with no background.
            /// </summary>
            public static readonly ColorFormat BrightRed;

            /// <summary>
            /// Bright yellow foreground with no background.
            /// </summary>
            public static readonly ColorFormat BrightYellow;

            /// <summary>
            /// Bright magenta foreground with no background.
            /// </summary>
            public static readonly ColorFormat BrightMagenta;

            /// <summary>
            /// Bright blue foreground with no background.
            /// </summary>
            public static readonly ColorFormat BrightBlue;

            /// <summary>
            /// Bright cyan foreground with no background.
            /// </summary>
            public static readonly ColorFormat BrightCyan;

            /// <summary>
            /// Green foreground with no background.
            /// </summary>
            public static readonly ColorFormat Green;

            /// <summary>
            /// Red foreground with no background.
            /// </summary>
            public static readonly ColorFormat Red;

            /// <summary>
            /// Yellow foreground with no background.
            /// </summary>
            public static readonly ColorFormat Yellow;

            /// <summary>
            /// Magenta foreground with no background.
            /// </summary>
            public static readonly ColorFormat Magenta;

            /// <summary>
            /// Blue foreground with no background.
            /// </summary>
            public static readonly ColorFormat Blue;

            /// <summary>
            /// Cyan foreground with no background.
            /// </summary>
            public static readonly ColorFormat Cyan;

            /// <summary>
            /// Light gray foreground with no background.
            /// </summary>
            public static readonly ColorFormat LightGray;

            /// <summary>
            /// Dark gray foreground with no background.
            /// </summary>
            public static readonly ColorFormat DarkGray;

            /// <summary>
            /// White foreground with no background.
            /// </summary>
            public static readonly ColorFormat White;

            /// <summary>
            /// Black foreground with no background.
            /// </summary>
            public static readonly ColorFormat Black;

            /// <summary>
            /// Initializes static members of the <see cref="Colors"/> class.
            /// </summary>
            static Colors()
            {
                Debug = new ColorFormat(ColorCode.BrightYellow);
                Success = new ColorFormat(ColorCode.Black, ColorCode.BrightGreen);
                Progress = new ColorFormat(ColorCode.BrightCyan);
                Error = new ColorFormat(ColorCode.BrightRed);
                Exception = new ColorFormat(ColorCode.BrightRed);
                Subtle = new ColorFormat(ColorCode.DarkGray);
                Warning = new ColorFormat(ColorCode.White, ColorCode.Red);
                Notice = new ColorFormat(ColorCode.Black, ColorCode.Yellow);
                DumpPropertyName = new ColorFormat(ColorCode.BrightCyan);
                DumpPropertyValue = new ColorFormat(ColorCode.BrightMagenta);
                DumpObjectDetail = new ColorFormat(ColorCode.BrightGreen);
                DumpObjectChrome = new ColorFormat(ColorCode.Yellow);
                BrightGreen = new ColorFormat(ColorCode.BrightGreen);
                BrightRed = new ColorFormat(ColorCode.BrightRed);
                BrightYellow = new ColorFormat(ColorCode.BrightYellow);
                BrightMagenta = new ColorFormat(ColorCode.BrightMagenta);
                BrightBlue = new ColorFormat(ColorCode.BrightBlue);
                BrightCyan = new ColorFormat(ColorCode.BrightCyan);
                Green = new ColorFormat(ColorCode.Green);
                Red = new ColorFormat(ColorCode.Red);
                Yellow = new ColorFormat(ColorCode.Yellow);
                Magenta = new ColorFormat(ColorCode.Magenta);
                Blue = new ColorFormat(ColorCode.Blue);
                Cyan = new ColorFormat(ColorCode.Cyan);
                LightGray = new ColorFormat(ColorCode.LightGray);
                DarkGray = new ColorFormat(ColorCode.DarkGray);
                White = new ColorFormat(ColorCode.White);
                Black = new ColorFormat(ColorCode.Black);
            }
            
            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Debugging.
            /// </summary>
            public static ColorFormat Debug { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Success messages.
            /// </summary>
            public static ColorFormat Success { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Progress.
            /// </summary>
            public static ColorFormat Progress { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Errors.
            /// </summary>
            public static ColorFormat Error { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Exceptions.
            /// </summary>
            public static ColorFormat Exception { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Subtle messages.
            /// </summary>
            public static ColorFormat Subtle { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Warnings.
            /// </summary>
            public static ColorFormat Warning { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for Notices.
            /// </summary>
            public static ColorFormat Notice { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for printing property names when dumping objects to the console.
            /// </summary>
            public static ColorFormat DumpPropertyName { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for printing property values when dumping objects to the console.
            /// </summary>
            public static ColorFormat DumpPropertyValue { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for printing object details when dumping objects to the console.
            /// </summary>
            public static ColorFormat DumpObjectDetail { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ColorFormat"/> to use for printing the chrome when dumping objects to the console.
            /// </summary>
            public static ColorFormat DumpObjectChrome { get; set; }
        }
    }
}