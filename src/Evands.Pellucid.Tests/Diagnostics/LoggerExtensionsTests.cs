using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Diagnostics
{
    public class LoggerExtensionsTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();
        private TestLogWriter logger = new TestLogWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
            Logger.RegisterLogWriter(logger);
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.LogLevels = LogLevels.All;
        }

        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            writer.Messages.Clear();
            logger.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            Logger.UnregisterLogWriter(logger);
            ConsoleBase.OptionalHeader = string.Empty;
        }
        private string[] linesToTest = new string[]
        {
            "Message Number 0",
            "Message Number 1",
            "Message Number 2"
        };

        private string[][] formatLinesToTest = new string[][]
        {
            new string[] { "This is a {0} message.", "format test" },
            new string[] { "This is a {0} message with {1} formats", "format testing", "2" }
        };
#region LogMessageMethodsTests

        [Test]
        public async Task LogMessage_WithoutLevel_NoArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                this.LogMessage(ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                await Assert.That(writer.Messages[i].Contains(ConsoleBase.Colors.Red.FormatText(linesToTest[i]))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Notice][LoggerExtensionsTests] {0}", linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_WithoutLevel_WithArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                this.LogMessage(ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[32;49m{0}\x1b[0m", content))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Notice][LoggerExtensionsTests] {0}", content)).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_NoticeLevel_NoArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Notice, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[31;49m{0}\x1b[0m", linesToTest[i]))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Notice][LoggerExtensionsTests] {0}", linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_NoticeLevel_WithArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Notice, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[32;49m{0}\x1b[0m", content))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Notice][LoggerExtensionsTests] {0}", content)).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_DebugLevel_NoArgs_DebugsColor_LogsDebug()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Debug, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[31;49m{0}\x1b[0m", linesToTest[i]))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Debug][LoggerExtensionsTests] {0}", linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_DebugLevel_WithArgs_DebugsColor_LogsDebug()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Debug, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[32;49m{0}\x1b[0m", content))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Debug][LoggerExtensionsTests] {0}", content)).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_WarningLevel_NoArgs_DebugsColor_LogsWarning()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Warning, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[31;49m{0}\x1b[0m", linesToTest[i]))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Warning][LoggerExtensionsTests] {0}", linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_WarningLevel_WithArgs_DebugsColor_LogsWarning()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Warning, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[32;49m{0}\x1b[0m", content))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Warning][LoggerExtensionsTests] {0}", content)).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_ErrorLevel_NoArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Error, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[31;49m{0}\x1b[0m", linesToTest[i]))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Error][LoggerExtensionsTests] {0}", linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_ErrorLevel_WithArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Error, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[32;49m{0}\x1b[0m", content))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Error][LoggerExtensionsTests] {0}", content)).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_ExceptionLevel_NoArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Exception, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[31;49m{0}\x1b[0m", linesToTest[i]))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Error][LoggerExtensionsTests] {0}", linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task LogMessage_ExceptionLevel_WithArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                this.LogMessage(LogLevels.Exception, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                await Assert.That(writer.Messages[i].Contains(string.Format("\x1b[32;49m{0}\x1b[0m", content))).IsTrue();
                await Assert.That(logger.Messages[i] == string.Format("[Error][LoggerExtensionsTests] {0}", content)).IsTrue();
            }
        }

        #endregion
    }
}
