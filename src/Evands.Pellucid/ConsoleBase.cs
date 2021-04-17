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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using Evands.Pellucid.Terminal;
using Evands.Pellucid.Terminal.Formatting;

namespace Evands.Pellucid
{
    /// <summary>
    /// Provides methods for interacting with the Crestron Console in a more simplified manner.
    /// <para>
    /// One way to do this is to create your own static Console class in the root of your project namespace,
    /// which inherits from this class. This will allow you to call these methods by simply entering 'Console.{MethodName}'
    /// since a Console class in your root namespace will supersede the <see cref="System.Console"/> class.
    /// </para>
    /// <para>
    /// Alternatively you can use this class directly, as all the methods required are static methods.
    /// </para>
    /// </summary>
    public abstract class ConsoleBase
    {
        /// <summary>
        /// Holds the writers registered with the <see cref="ConsoleBase"/>.
        /// </summary>
        private static List<IConsoleWriter> writers;

        /// <summary>
        /// Holds the application number header text.
        /// </summary>
        private static string headerText;

        /// <summary>
        /// Tracks whether the last item written included a line terminator.
        /// </summary>
        private static bool isLastWriteALine = true;

        /// <summary>
        /// Initializes static members of the <see cref="ConsoleBase"/> class.
        /// </summary>
        static ConsoleBase()
        {
            writers = new List<IConsoleWriter>();
            NewLine = "\r\n";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleBase"/> class.
        /// </summary>
        protected ConsoleBase()
        {
        }

        /// <summary>
        /// Gets or sets the string to use for NewLine operations.
        /// </summary>
        public static string NewLine { get; set; }

        /// <summary>
        /// Gets or sets the text used for the first header value. If null or an empty string the first header value is omitted.
        /// <para>Will be enclosed with brackets, such as [HeaderText].</para>
        /// </summary>
        public static string OptionalHeader
        {
            get { return headerText; }

            set
            {
                headerText = string.Format("{0}{1}{2}",
                    value.StartsWith("[") ? string.Empty : "[",
                    value,
                    value.EndsWith("]") ? string.Empty : "]");
            }
        }

        /// <summary>
        /// Configures the console to use the current program's slot as the first header value.
        /// </summary>
        public static void UseProgramSlotAsHeader()
        {
            OptionalHeader = InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// Registers a <see cref="IConsoleWriter"/> to write messages to.
        /// </summary>
        /// <param name="writer">The <see cref="IConsoleWriter"/> to register.</param>
        public static void RegisterConsoleWriter(IConsoleWriter writer)
        {
            if (!writers.Contains(writer))
            {
                writers.Add(writer);
            }
        }

        /// <summary>
        /// Unregisters a <see cref="IConsoleWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IConsoleWriter"/> to unregister.</param>
        public static void UnregisterConsoleWriter(IConsoleWriter writer)
        {
            writers.Remove(writer);
        }

        /// <summary>
        /// Gets the <see cref="IConsoleWriter"/>s registered.
        /// </summary>
        /// <returns>A collection of <see cref="IConsoleWriter"/> objects registered.</returns>
        public static ReadOnlyCollection<IConsoleWriter> GetRegisteredConsoleWriters()
        {
            return writers.AsReadOnly();
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
                if (isLastWriteALine)
                {
                    WriteToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteToConsoles(message);
                }
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
                if (isLastWriteALine)
                {
                    WriteToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteToConsoles(message);
                }
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
                if (isLastWriteALine)
                {
                    WriteToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteToConsoles(message);
                }
            }
        }

        /// <summary>
        /// Writes colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(IConsoleColor foreground, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(new ColorFormat(foreground), message, args);

            if (!string.IsNullOrEmpty(message))
            {
                if (isLastWriteALine)
                {
                    WriteToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteToConsoles(message);
                }
            }
        }

        /// <summary>
        /// Writes colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">The background color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(IConsoleColor foreground, IConsoleColor background, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(new ColorFormat(foreground, background), message, args);

            if (!string.IsNullOrEmpty(message))
            {
                if (isLastWriteALine)
                {
                    WriteToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteToConsoles(message);
                }
            }
        }

        /// <summary>
        /// Writes uncolored text to the console output.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void Write(string message, params object[] args)
        {
            message = message.OptionalFormat(message, args);

            if (!string.IsNullOrEmpty(message))
            {
                if (isLastWriteALine)
                {
                    WriteToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteToConsoles(message);
                }
            }
        }

        /// <summary>
        /// Writes text to the console output, without any header appended.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        public static void WriteNoHeader(string message, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                WriteToConsoles(message, args);
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
                if (isLastWriteALine)
                {
                    WriteLineToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteLineToConsoles(message);
                }
            }
            else
            {
                WriteLine();
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
                if (isLastWriteALine)
                {
                    WriteLineToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteLineToConsoles(message);
                }
            }
            else
            {
                WriteLine();
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
                if (isLastWriteALine)
                {
                    WriteLineToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteLineToConsoles(message);
                }
            }
            else
            {
                WriteLine();
            }
        }

        /// <summary>
        /// Writes a line of colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(IConsoleColor foreground, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(new ColorFormat(foreground), message, args);
            if (!string.IsNullOrEmpty(message))
            {
                if (isLastWriteALine)
                {
                    WriteLineToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteLineToConsoles(message);
                }
            }
            else
            {
                WriteLine();
            }
        }

        /// <summary>
        /// Writes a line of colored text to the console output.
        /// </summary>
        /// <param name="foreground">The foreground color of the text.</param>
        /// <param name="background">The background color of the text.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(IConsoleColor foreground, IConsoleColor background, string message, params object[] args)
        {
            message = Formatters.GetColorFormattedString(new ColorFormat(foreground, background), message, args);
            if (!string.IsNullOrEmpty(message))
            {
                if (isLastWriteALine)
                {
                    WriteLineToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteLineToConsoles(message);
                }
            }
            else
            {
                WriteLine();
            }
        }

        /// <summary>
        /// Writes a line of text to the console output.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="args">Optional arguments to use when formatting the message.</param>
        public static void WriteLine(string message, params object[] args)
        {
            message = message.OptionalFormat(message, args);
            if (!string.IsNullOrEmpty(message))
            {
                if (isLastWriteALine)
                {
                    WriteLineToConsoles("{0}{1}", headerText, message);
                }
                else
                {
                    WriteLineToConsoles(message);
                }
            }
            else
            {
                WriteLine();
            }
        }

        /// <summary>
        /// Writes a line of text to the console output, without any header prepended.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        public static void WriteLineNoHeader(string message, params object[] args)
        {
            if (!string.IsNullOrEmpty(message))
            {
                WriteLineToConsoles(message, args);
            }
            else
            {
                WriteLine();
            }
        }

        /// <summary>
        /// Writes an empty line to the console output. This line will have no header.
        /// </summary>
        public static void WriteLine()
        {
            if (writers.Count == 0)
            {
                writers.Add(new CrestronConsoleWriter());
            }

            writers.ForEach(w => w.WriteLine());
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
            WriteResponseToConsoles(Formatters.GetColorFormattedString(colors, message, args));
        }

        /// <summary>
        /// Writes a command response to the console using the colors specified.
        /// </summary>
        /// <param name="foreground">The <see cref="ColorCode"/> to print the response foreground using.</param>
        /// <param name="background">The <see cref="ColorCode"/> to print the response background using.</param>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(ColorCode foreground, ColorCode background, string message, params object[] args)
        {
            WriteResponseToConsoles(Formatters.GetColorFormattedString(foreground, background, message, args));
        }

        /// <summary>
        /// Writes a command response to the console using the colors specified.
        /// </summary>
        /// <param name="foreground">The <see cref="ColorCode"/> to print the response foreground using.</param>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(ColorCode foreground, string message, params object[] args)
        {
            WriteResponseToConsoles(Formatters.GetColorFormattedString(foreground, ColorCode.None, message, args));
        }

        /// <summary>
        /// Writes a command response to the console.
        /// </summary>
        /// <param name="message">The response message.</param>
        /// <param name="args">Optional arguments to use when formatting the response.</param>
        public static void WriteCommandResponse(string message, params object[] args)
        {
            WriteResponseToConsoles(message, args);
        }

        /// <summary>
        /// Writes to all registered consoles.
        /// </summary>
        /// <param name="message">The message to writer.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        private static void WriteToConsoles(string message, params object[] args)
        {
            if (writers.Count == 0)
            {
                RegisterConsoleWriter(new CrestronConsoleWriter());
            }

            writers.ForEach(w => w.Write(message.OptionalFormat(args)));
            isLastWriteALine = false;
        }

        /// <summary>
        /// Writes to all registered consoles.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        private static void WriteLineToConsoles(string message, params object[] args)
        {
            if (writers.Count == 0)
            {
                RegisterConsoleWriter(new CrestronConsoleWriter());
            }

            writers.ForEach(w => w.WriteLine(message.OptionalFormat(args)));
            isLastWriteALine = true;
        }

        /// <summary>
        /// Writes to all registered consoles.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Optional array of arguments to format the message using.</param>
        private static void WriteResponseToConsoles(string message, params object[] args)
        {
            if (writers.Count == 0)
            {
                RegisterConsoleWriter(new CrestronConsoleWriter());
            }

            writers.ForEach(w => w.WriteCommandResponse(message.OptionalFormat(args)));
            isLastWriteALine = true;
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
                Success = new ColorFormat(ColorCode.BrightGreen);
                Progress = new ColorFormat(ColorCode.BrightCyan);
                Error = new ColorFormat(ColorCode.BrightRed);
                Exception = new ColorFormat(ColorCode.BrightRed);
                Subtle = new ColorFormat(ColorCode.DarkGray);
                Warning = new ColorFormat(new RgbColor(254, 163, 110));
                Notice = new ColorFormat(ColorCode.DarkGray);
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