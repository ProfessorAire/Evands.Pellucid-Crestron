using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evands.Pellucid.Terminal.Commands;
using Crestron.SimplSharp;
using Evands.Pellucid.Helpers;

namespace Evands.Pellucid.Diagnostics
{
    public class LoggerCommandsTests
    {
        private CrestronErrorLogCommands logCommands = new CrestronErrorLogCommands();

        private GlobalCommand global;

        [Before(Test)]
        public void TestInitialize()
        {
            global = new GlobalCommand("app", "", Access.Administrator);
            global.AddCommand(logCommands);

            CrestronConsole.Messages.Length = 0;
            Options.Instance.ColorizeConsoleOutput = true;
        }

        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            CrestronConsole.Messages.Length = 0;
            CrestronConsole.CommandResponse = string.Empty;
        }

        [Test]
        public async Task PrettyPrintLog_With_Nothing_Prints_Message()
        {
            global.ExecuteCommand("log plog -o");
            var result = CrestronConsole.Messages.ToString();

            await Assert.That(result.Contains("No content returned")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_Everything_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("Error")).IsTrue();
            await Assert.That(result.Contains("Notice")).IsTrue();
            await Assert.That(result.Contains("Warning")).IsTrue();
            await Assert.That(result.Contains("\x1b[")).IsFalse();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_Everything_With_Color()
        {
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("Error")).IsTrue();
            await Assert.That(result.Contains("Notice")).IsTrue();
            await Assert.That(result.Contains("Warning")).IsTrue();
            await Assert.That(result.Contains(ConsoleBase.Colors.Error.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains(ConsoleBase.Colors.Warning.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, ""))).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredOrigin_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --origin nk.exe");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("ConsoleServiceCE.exe")).IsFalse();
            await Assert.That(result.Contains("SimplSharpPro.exe")).IsFalse();
            await Assert.That(result.Contains("TLDM.exe")).IsFalse();
            await Assert.That(result.Contains("\x1b[")).IsFalse();
            await Assert.That(result.Contains("nk.exe")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredOrigin_With_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --origin NK.exe");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("ConsoleServiceCE.exe")).IsFalse();
            await Assert.That(result.Contains("SimplSharpPro.exe")).IsFalse();
            await Assert.That(result.Contains("TLDM.exe")).IsFalse();
            await Assert.That(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains("nk.exe")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredMessage_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --message flash");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("Event rcvd")).IsFalse();
            await Assert.That(result.Contains("**Program 10 Stopped**")).IsFalse();
            await Assert.That(result.Contains("SHELL")).IsFalse();
            await Assert.That(result.Contains("\x1b[")).IsFalse();
            await Assert.That(result.Contains("Flash")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredMessage_With_Color()
        {
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --message flash");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("Event rcvd")).IsFalse();
            await Assert.That(result.Contains("**Program 10 Stopped**")).IsFalse();
            await Assert.That(result.Contains("SHELL")).IsFalse();
            await Assert.That(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains("Flash")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredSingleLevel_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --level Notice");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("\x1b[")).IsFalse();
            await Assert.That(result.Contains("Error")).IsFalse();
            await Assert.That(result.Contains("Warning")).IsFalse();
            await Assert.That(result.Contains("Notice")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredMultiLevel_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --level Notice,Warning");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("\x1b[")).IsFalse();
            await Assert.That(result.Contains("Error")).IsFalse();
            await Assert.That(result.Contains("Warning")).IsTrue();
            await Assert.That(result.Contains("Notice")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredSingleLevel_With_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --level Error");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains(ConsoleBase.Colors.Error.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains("Warning")).IsFalse();
            await Assert.That(result.Contains("Notice")).IsFalse();
            await Assert.That(result.Contains("Error")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredMultiLevel_With_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --level Notice,Error");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains(ConsoleBase.Colors.Error.FormatText(false, ""))).IsTrue();
            await Assert.That(result.Contains("Warning")).IsFalse();
            await Assert.That(result.Contains("Notice")).IsTrue();
            await Assert.That(result.Contains("Error")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredEverything_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --origin nk.exe --level Notice --message User");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("nk.exe")).IsTrue();
            await Assert.That(result.Contains("User")).IsTrue();
            await Assert.That(result.Contains("Timeout")).IsFalse();
            await Assert.That(result.Contains("error message")).IsFalse();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredOriginMessage_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLogForFilterTests();
            global.ExecuteCommand("log plog --origin nk.exe --message User");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("nk.exe")).IsTrue();
            await Assert.That(result.Contains("User")).IsTrue();
            await Assert.That(result.Contains("Timeout")).IsFalse();
            await Assert.That(result.Contains("error message")).IsFalse();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredOriginLevel_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLogForFilterTests();
            global.ExecuteCommand("log plog --origin nk.exe --level Error");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("nk.exe")).IsTrue();
            await Assert.That(result.Contains("User")).IsFalse();
            await Assert.That(result.Contains("Timeout")).IsFalse();
            await Assert.That(result.Contains("error message")).IsTrue();
        }

        [Test]
        public async Task PrettyPrintLog_Prints_WithFilteredLevelMessage_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLogForFilterTests();
            global.ExecuteCommand("log plog --level Notice --message User");
            var result = CrestronConsole.Messages.ToString();
            await Assert.That(result.Contains("nk.exe")).IsTrue();
            await Assert.That(result.Contains("User")).IsTrue();
            await Assert.That(result.Contains("Timeout")).IsFalse();
            await Assert.That(result.Contains("error message")).IsFalse();
        }
    }
}
