using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evands.Pellucid.Terminal.Commands;
using Crestron.SimplSharp;
using Evands.Pellucid.Helpers;

namespace Evands.Pellucid.Diagnostics
{
    [TestClass]
    public class LoggerCommandsTests
    {
        private LoggerCommands logCommands = new LoggerCommands();

        private GlobalCommand global;

        [TestInitialize]
        public void TestInitialize()
        {
            global = new GlobalCommand("app", "", Access.Administrator);
            global.AddCommand(logCommands);

            CrestronConsole.Messages.Length = 0;
            Options.Instance.ColorizeConsoleOutput = true;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CrestronConsole.Messages.Length = 0;
            CrestronConsole.CommandResponse = string.Empty;
        }

        [TestMethod]
        public void PrettyPrintLog_With_Nothing_Prints_Message()
        {
            global.ExecuteCommand("log plog -o");
            var result = CrestronConsole.Messages.ToString();

            Assert.IsTrue(result.Contains("No content returned"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_Everything_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains("Error"));
            Assert.IsTrue(result.Contains("Notice"));
            Assert.IsTrue(result.Contains("Warning"));
            Assert.IsFalse(result.Contains("\x1b["));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_Everything_With_Color()
        {
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains("Error"));
            Assert.IsTrue(result.Contains("Notice"));
            Assert.IsTrue(result.Contains("Warning"));
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Error.FormatText(false, "")));
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Warning.FormatText(false, "")));
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, "")));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredOrigin_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --origin nk.exe");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsFalse(result.Contains("ConsoleServiceCE.exe"));
            Assert.IsFalse(result.Contains("SimplSharpPro.exe"));
            Assert.IsFalse(result.Contains("TLDM.exe"));
            Assert.IsFalse(result.Contains("\x1b["));
            Assert.IsTrue(result.Contains("nk.exe"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredOrigin_With_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --origin NK.exe");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsFalse(result.Contains("ConsoleServiceCE.exe"));
            Assert.IsFalse(result.Contains("SimplSharpPro.exe"));
            Assert.IsFalse(result.Contains("TLDM.exe"));
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, "")));
            Assert.IsTrue(result.Contains("nk.exe"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredMessage_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --message flash");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsFalse(result.Contains("Event rcvd"));
            Assert.IsFalse(result.Contains("**Program 10 Stopped**"));
            Assert.IsFalse(result.Contains("SHELL"));
            Assert.IsFalse(result.Contains("\x1b["));
            Assert.IsTrue(result.Contains("Flash"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredMessage_With_Color()
        {
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --message flash");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsFalse(result.Contains("Event rcvd"));
            Assert.IsFalse(result.Contains("**Program 10 Stopped**"));
            Assert.IsFalse(result.Contains("SHELL"));
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, "")));
            Assert.IsTrue(result.Contains("Flash"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredSingleLevel_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --level Notice");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsFalse(result.Contains("\x1b["));
            Assert.IsFalse(result.Contains("Error"));
            Assert.IsFalse(result.Contains("Warning"));
            Assert.IsTrue(result.Contains("Notice"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredMultiLevel_Without_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog -o --level Notice,Warning");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsFalse(result.Contains("\x1b["));
            Assert.IsFalse(result.Contains("Error"));
            Assert.IsTrue(result.Contains("Warning"));
            Assert.IsTrue(result.Contains("Notice"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredSingleLevel_With_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --level Error");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Error.FormatText(false, "")));
            Assert.IsFalse(result.Contains("Warning"));
            Assert.IsFalse(result.Contains("Notice"));
            Assert.IsTrue(result.Contains("Error"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredMultiLevel_With_Color()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --level Notice,Error");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Notice.FormatText(false, "")));
            Assert.IsTrue(result.Contains(ConsoleBase.Colors.Error.FormatText(false, "")));
            Assert.IsFalse(result.Contains("Warning"));
            Assert.IsTrue(result.Contains("Notice"));
            Assert.IsTrue(result.Contains("Error"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredEverything_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLog();
            global.ExecuteCommand("log plog --origin nk.exe --level Notice --message User");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains("nk.exe"));
            Assert.IsTrue(result.Contains("User"));
            Assert.IsFalse(result.Contains("Timeout"));
            Assert.IsFalse(result.Contains("error message"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredOriginMessage_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLogForFilterTests();
            global.ExecuteCommand("log plog --origin nk.exe --message User");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains("nk.exe"));
            Assert.IsTrue(result.Contains("User"));
            Assert.IsFalse(result.Contains("Timeout"));
            Assert.IsFalse(result.Contains("error message"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredOriginLevel_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLogForFilterTests();
            global.ExecuteCommand("log plog --origin nk.exe --level Error");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains("nk.exe"));
            Assert.IsFalse(result.Contains("User"));
            Assert.IsFalse(result.Contains("Timeout"));
            Assert.IsTrue(result.Contains("error message"));
        }

        [TestMethod]
        public void PrettyPrintLog_Prints_WithFilteredLevelMessage_Returns_Expected()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            CrestronConsole.CommandResponse = ErrorLogData.GetThreeSeriesLogForFilterTests();
            global.ExecuteCommand("log plog --level Notice --message User");
            var result = CrestronConsole.Messages.ToString();
            Assert.IsTrue(result.Contains("nk.exe"));
            Assert.IsTrue(result.Contains("User"));
            Assert.IsFalse(result.Contains("Timeout"));
            Assert.IsFalse(result.Contains("error message"));
        }
    }
}
