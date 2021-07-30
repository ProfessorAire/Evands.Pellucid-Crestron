using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Diagnostics
{
    [TestClass]
    public class LoggerTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();
        private TestLogWriter logger = new TestLogWriter();

        [TestInitialize]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
            Logger.RegisterLogWriter(logger);
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.LogLevels = LogLevels.All;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            logger.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            Logger.UnregisterLogWriter(logger);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.LogLevels = LogLevels.None;
        }

        private TestContext testContextInstance;

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

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region LogMessageMethodsTests

        [TestMethod]
        public void LogMessage_WithoutLevel_NoArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Logger.LogMessage(this, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[31m{0}\x1b[0m", linesToTest[i])));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Notice][LoggerTests] {0}", linesToTest[i]));
            }
        }

        [TestMethod]
        public void LogMessage_WithoutLevel_WithArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                Logger.LogMessage(this, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[32m{0}\x1b[0m", content)));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Notice][LoggerTests] {0}", content));
            }
        }

        [TestMethod]
        public void LogMessage_NoticeLevel_NoArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Notice, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[31m{0}\x1b[0m", linesToTest[i])));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Notice][LoggerTests] {0}", linesToTest[i]));
            }
        }

        [TestMethod]
        public void LogMessage_NoticeLevel_WithArgs_DebugsColor_LogsNotice()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Notice, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[32m{0}\x1b[0m", content)));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Notice][LoggerTests] {0}", content));
            }
        }

        [TestMethod]
        public void LogMessage_DebugLevel_NoArgs_DebugsColor_LogsDebug()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Debug, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[31m{0}\x1b[0m", linesToTest[i])));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Debug][LoggerTests] {0}", linesToTest[i]));
            }
        }

        [TestMethod]
        public void LogMessage_DebugLevel_WithArgs_DebugsColor_LogsDebug()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Debug, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[32m{0}\x1b[0m", content)));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Debug][LoggerTests] {0}", content));
            }
        }

        [TestMethod]
        public void LogMessage_WarningLevel_NoArgs_DebugsColor_LogsWarning()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Warning, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[31m{0}\x1b[0m", linesToTest[i])));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Warning][LoggerTests] {0}", linesToTest[i]));
            }
        }

        [TestMethod]
        public void LogMessage_WarningLevel_WithArgs_DebugsColor_LogsWarning()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Warning, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[32m{0}\x1b[0m", content)));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Warning][LoggerTests] {0}", content));
            }
        }

        [TestMethod]
        public void LogMessage_ErrorLevel_NoArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Error, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[31m{0}\x1b[0m", linesToTest[i])));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Error][LoggerTests] {0}", linesToTest[i]));
            }
        }

        [TestMethod]
        public void LogMessage_ErrorLevel_WithArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Error, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[32m{0}\x1b[0m", content)));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Error][LoggerTests] {0}", content));
            }
        }

        [TestMethod]
        public void LogMessage_ExceptionLevel_NoArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Exception, ConsoleBase.Colors.Red, linesToTest[i]);
            }

            for (var i = 0; i < linesToTest.Length; i++)
            {
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[31m{0}\x1b[0m", linesToTest[i])));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Error][LoggerTests] {0}", linesToTest[i]));
            }
        }

        [TestMethod]
        public void LogMessage_ExceptionLevel_WithArgs_DebugsColor_LogsError()
        {
            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                Logger.LogMessage(this, LogLevels.Exception, ConsoleBase.Colors.Green, formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
            }

            for (var i = 0; i < formatLinesToTest.Length; i++)
            {
                var content = string.Format(formatLinesToTest[i][0], formatLinesToTest[i].Skip(1).ToArray());
                Assert.IsTrue(writer.Messages[i].Contains(string.Format("\x1b[32m{0}\x1b[0m", content)));
                Assert.IsTrue(logger.Messages[i] == string.Format("[Error][LoggerTests] {0}", content));
            }
        }

        #endregion
    }
}
