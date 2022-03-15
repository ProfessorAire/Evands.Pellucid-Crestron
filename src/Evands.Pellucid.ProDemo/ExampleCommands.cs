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

using System;
using Evands.Pellucid;
using Evands.Pellucid.Terminal.Commands;
using Evands.Pellucid.Terminal.Commands.Attributes;
using Evands.Pellucid.Terminal.Formatting;
using Evands.Pellucid.Terminal.Formatting.Tables;

namespace Evands.Pellucid.ProDemo
{
    /// <summary>
    /// Example commands.
    /// </summary>
    [Command("example", 2, "Example console command.")]
    public class ExampleCommands : TerminalCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleCommands"/> class.
        /// </summary>        
        public ExampleCommands()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleCommands"/> class.
        /// </summary>
        /// <param name="suffix">The suffix to append to the command name.</param>        
        public ExampleCommands(string suffix)
            : base(suffix)
        {
        }

        /// <summary>
        /// Echoes a message to the console.
        /// </summary>
        /// <param name="message">The message to echo.</param>
        /// <param name="caps">When true will print the message in all caps.</param>
        [Verb("echo", 2, "Echoes the message provided back to the console.")]
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
        [Verb("echo", 2, "Echoes the message provided back to the console in a specified color.")]
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
        [Verb("echo", 2, "Echoes the message provided back to the console in a specified color.")]
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
        [Verb("echo-green", 6, "Echoes the message provided back to the console in green.")]
        [Sample("example echo-green --message \"Just a test.\"", "Prints the message \"Just a test.\" to the console in green.")]
        public void EchoGreenMessage(
            [Operand("message", "The message to echo.")] string message,
            [Flag("caps", 'c', "When present prints the message in all caps.", true)] bool caps)
        {
            ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightGreen, caps ? message.ToUpper() : message);
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        [Verb("table", 3, "Prints a sample table to the console.")]
        [Sample("table", "Prints a sample table with automatic cell widths.")]
        [DefaultVerb]
        public void SampleTable()
        {
            SampleTable(0, 0);
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        /// <param name="minWidth">The minimum width the table can print to.</param>
        [Verb("table", 3, "Prints a sample table to the console.")]
        [Sample("table --min 15", "Prints a sample table with a minimum cell width of 15.")]
        public void SampleTable(
            [Operand("min", "Sets the minimum width the cells can be.")] int minWidth)
        {
            this.SampleTable(minWidth, 0);
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        /// <param name="minWidth">The minimum width the table can print to.</param>
        [Verb("table", 3, "Prints a sample table to the console.")]
        [Sample("table --max 15", "Prints a sample table with a maximum cell width of 15.")]
        public void SampleTableMax(
            [Operand("max", "Sets the maximum width the cells can be.")] int maxWidth)
        {
            this.SampleTable(0, maxWidth);
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        /// <param name="minWidth">The minimum width the table can print to.</param>
        [Verb("table", 3, "Prints a sample table to the console.")]
        [Sample("table --min 10 --max 20", "Prints a sample table with a minimum cell width of 10 and a maxium of 20.")]
        public void SampleTable(
            [Operand("min", "Sets the minimum width the cells can be.")] int minWidth,
            [Operand("max", "Sets the maximum width the cells can be.")] int maxWidth)
        {
            ConsoleBase.WriteLine();
            ConsoleBase.WriteLineNoHeader(Table.Create()
                .SetMinimumColumnWidth(minWidth)
                .AddColumnWithHeader("Device", "Touchpanel", "DSP", "Codec", "Display 1", "Display 2", "Device with a really long name.")
                .AddColumnWithHeader("Status", "Online", "Online", "Offline", "Offline", "Online", "Offline")
                .FormatHeaders(ConsoleBase.Colors.BrightYellow, HorizontalAlignment.Center)
                .FormatColumn(0, ConsoleBase.Colors.BrightCyan, HorizontalAlignment.Right)
                .ForEachCellInColumn(1, c => c.Color = c.Contents == "Offline" ? ConsoleBase.Colors.BrightRed : ConsoleBase.Colors.BrightGreen)
                .ToString(maxWidth));
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        /// <param name="minWidth">The minimum width the table can print to.</param>
        [Verb("table-mix", "tm", "Prints a sample table to the console.")]
        [Sample("table --min 15", "Prints a sample table with a minimum cell width of 15.")]
        public void SampleTableMixed(
            [Operand("min", "Sets the minimum width the cells can be.")] int minWidth)
        {
            this.SampleTableMixed(minWidth, 0);
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        /// <param name="minWidth">The minimum width the table can print to.</param>
        [Verb("table-mix", "tm", "Prints a sample table to the console.")]
        [Sample("table --max 15", "Prints a sample table with a maximum cell width of 15.")]
        public void SampleTableMixedMax(
            [Operand("max", "Sets the maximum width the cells can be.")] int maxWidth)
        {
            this.SampleTableMixed(0, maxWidth);
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        /// <param name="minWidth">The minimum width the table can print to.</param>
        [Verb("table-mix", "tm", "Prints a sample table to the console.")]
        [Sample("table --min 10 --max 20", "Prints a sample table with a minimum cell width of 10 and a maxium of 20.")]
        public void SampleTableMixed(
            [Operand("min", "Sets the minimum width the cells can be.")] int minWidth,
            [Operand("max", "Sets the maximum width the cells can be.")] int maxWidth)
        {
            var longName = string.Format("{0} {1} {2} really {3} {4} {5}",
                ConsoleBase.Colors.BrightGreen.FormatText("Device"),
                ConsoleBase.Colors.BrightMagenta.FormatText("with"),
                ConsoleBase.Colors.BrightRed.FormatText("a"),
                ConsoleBase.Colors.BrightYellow.FormatText("colorful"),
                ConsoleBase.Colors.White.FormatText("long"),
                ConsoleBase.Colors.BrightBlue.FormatText("name"));

            ConsoleBase.WriteLine();
            ConsoleBase.WriteLineNoHeader(Table.Create()
                .SetMinimumColumnWidth(minWidth)
                .AddColumnWithHeader("\x1b[4mDevice\x1b[0m", "\x1b[5mTouchpanel\x1b[0m", "\x1b[3mDSP\x1b[0m", "\x1b[1mCodec\x1b[0m", "\x1b[2mDisplay \x1b[0m\x1b[7m1\x1b[0m", "\x1b[9mDisplay 2\x1b[0m", longName)
                .AddColumnWithHeader("Status", "Online", "Online", "Offline", "Offline", "Online", "Offline")
                .FormatHeaders(ConsoleBase.Colors.BrightYellow, HorizontalAlignment.Center)
                .FormatColumn(0, ConsoleBase.Colors.BrightCyan, HorizontalAlignment.Right)
                .ForEachCellInColumn(1, c => c.Color = c.Contents == "Offline" ? ConsoleBase.Colors.BrightRed : ConsoleBase.Colors.BrightGreen)
                .ToString(maxWidth));
        }

        /// <summary>
        /// Prints a sample table to the console.
        /// </summary>
        [DefaultVerb]
        [Verb("table-mix", "tm", "Prints a sample table to the console.")]
        [Sample("table", "Prints a sample table with automatic cell widths.")]
        public void SampleTableMixed()
        {
            SampleTableMixed(0, 0);
        }

        /// <summary>
        /// Dumps the <see cref="Evands.Pellucid.Options.Instance"/> value to the console.
        /// </summary>
        /// <param name="full">Optional parameter specifying whether to use full type names.</param>
        [Verb("DumpOptions", 4, "Dumps the Evands.Pellucid.Options class to the console.")]
        public void DumpOptions(
            [Flag("full", 'f', "Writes type names with their full name.", true)] bool full)
        {
            ConsoleBase.WriteLine();
            Evands.Pellucid.Options.Instance.Dump(full);
        }

        /// <summary>
        /// Dumps a list of objects to the console.
        /// </summary>
        /// <param name="max">The max number of times an item can recurse.</param>
        /// <param name="full">Optional parameter specifying whether to use full type names.</param>
        [Verb("DumpClassListPartial", "dump-partial", "Dump a list of class objects to the console.")]
        public void DumpObjectList(
            [Operand("max", "When provided, indicates the maximum number of times the object being dumped should be recursed.")] int max,
            [Flag("full", 'f', "Writes type names with their full name.", true)] bool full)
        {
            var t = new System.Collections.Generic.List<Evands.Pellucid.ProDemo.Sample.SampleItem>();
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(1, "Item A", true, "IA"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(2, "Item B", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(3, "Item C", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(4, "Item 4", true, "Four", "Item Four"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(5, "Item 5", true, "Five", "Item Five", "Item 5"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(6, "Item F", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(7, "Item G", true, "Item G", "Item Gee", "Item Jee"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(8, "Item Name H", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(9, "Eye", true, "Item I", "I", "Item Eye"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(10, "Jay", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(11, null, false, null));

            var nc = new Evands.Pellucid.ProDemo.Sample.NestedSample("Nested Container", t);
            nc.Dump(max, full);
        }

        /// <summary>
        /// Dumps a list of objects to the console.
        /// </summary>
        /// <param name="full">Optional parameter specifying whether to use full type names.</param>
        /// <param name="extended">When true use the <see cref="RoundedChrome"/> chrome collection.</param>
        [Verb("DumpClassList", 5, "Dump a list of class objects to the console.")]
        public void DumpObjectList(
            [Flag("full", 'f', "Writes type names with their full name.", true)] bool full)
        {
            var t = new System.Collections.Generic.List<Evands.Pellucid.ProDemo.Sample.SampleItem>();
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(1, "Item A", true, "IA"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(2, "Item B", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(3, "Item C", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(4, "Item 4", true, "Four", "Item Four"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(5, "Item 5", true, "Five", "Item Five", "Item 5"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(6, "Item F", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(7, "Item G", true, "Item G", "Item Gee", "Item Jee"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(8, "Item Name H", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(9, "Eye", true, "Item I", "I", "Item Eye"));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(10, "Jay", true));
            t.Add(new Evands.Pellucid.ProDemo.Sample.SampleItem(11, null, false, null));

            var nc = new Evands.Pellucid.ProDemo.Sample.NestedSample("Nested Container", t);
            nc.Dump(full);
        }

        /// <summary>
        /// Writes an exception to the console.
        /// </summary>        
        [Verb("WriteEx", "wex", "Writes an exception to the console.")]
        public void WriteEx()
        {
            var exceptionString = "Formatted {0} string.";
            try
            {
                Evands.Pellucid.Diagnostics.Debug.WriteLine(string.Format(exceptionString));
            }
            catch (Exception ex)
            {
                Evands.Pellucid.Diagnostics.Debug.WriteException(this, ex, "Exception encountered.");
            }
        }

        [Verb("Blinky", "bl", "Writes a blinking line of dots to the console.")]
        public void Blinky(
            [Operand("duration", "The number of seconds to run for, printing one dot every 3 seconds. Minimum of 12.")]int duration)
        {
            duration = Math.Min(duration, 12);
            var stop = DateTime.Now.AddSeconds(duration);

            while (DateTime.Now < stop)
            {
                ConsoleBase.WriteCommandResponse("[:sbcf:lr]>>>...[:/all]");
                Crestron.SimplSharpPro.CrestronThread.Thread.Sleep(3000);
                ConsoleBase.WriteCommandResponse("\x08\x08\x08\x08\x08\x08[:cf:lg].[:/all]");
            }

            ConsoleBase.WriteCommandResponse("[:cf:lg]Processing Complete[:/all]");
        }
    }
}