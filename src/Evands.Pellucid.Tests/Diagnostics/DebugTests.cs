using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Diagnostics
{
    [TestClass]
    public class DebugTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [TestInitialize]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.Suppressed.Clear();
            Options.Instance.Allowed.Clear();
            Options.Instance.UseTimestamps = true;
        }

        private TestContext testContextInstance;

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

        [TestMethod]
        public void RegisterHeaderObject_AsString_AddsValueToHeaders_WithColor()
        {
            var value = "TestValue4321";
            var expected = ConsoleBase.Colors.Cyan;
            Debug.RegisterHeaderObject(value, expected);

            var actual = Debug.RegisteredClasses[value];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterHeaderObject_AsObject_AddsValueToHeaders_WithColor()
        {
            var value = new ThrowAway1();
            var name = value.GetType().FullName;
            var expected = ConsoleBase.Colors.Cyan;
            Debug.RegisterHeaderObject(value, expected);

            var actual = Debug.RegisteredClasses[name];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterHeaderObject_WhenAlreadyExisting_ReplacesColor()
        {
            var value = new ThrowAway2();
            var name = value.GetType().FullName;
            var expected = ConsoleBase.Colors.Cyan;

            Debug.RegisterHeaderObject(value, expected);
            var actual = Debug.RegisteredClasses[name];
            Assert.AreEqual(expected, actual);
            expected = ConsoleBase.Colors.BrightYellow;

            Debug.RegisterHeaderObject(value, expected);
            actual = Debug.RegisteredClasses[name];
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddSuppression_AddsItem_To_SuppressionList()
        {
            var suppression = "Suppression";
            Assert.IsTrue(Debug.AddSuppression(suppression));
            Assert.IsTrue(Options.Instance.Suppressed.Contains(suppression));
        }

        [TestMethod]
        public void AddAllowed_AddsItem_To_AllowedList()
        {
            var allowed = "Allowed";
            Assert.IsTrue(Debug.AddAllowed(allowed));
            Assert.IsTrue(Options.Instance.Allowed.Contains(allowed));
        }

        [TestMethod]
        public void RemoveSuppression_RemovesItem_From_SuppressionList()
        {
            var suppression = "SuppressionToRemove";
            Assert.IsTrue(Debug.AddSuppression(suppression));
            Assert.IsTrue(Options.Instance.Suppressed.Contains(suppression));
            Assert.IsTrue(Debug.RemoveSuppression(suppression));
            Assert.IsFalse(Options.Instance.Suppressed.Contains(suppression));
        }

        [TestMethod]
        public void RemoveAllowed_RemovesItem_From_AllowedList()
        {
            var allowed = "AllowedToRemove";
            Assert.IsTrue(Debug.AddAllowed(allowed));
            Assert.IsTrue(Options.Instance.Allowed.Contains(allowed));
            Assert.IsTrue(Debug.RemoveAllowed(allowed));
            Assert.IsFalse(Options.Instance.Allowed.Contains(allowed));
        }

        [TestMethod]
        public void RemoveSuppression_WhenNotAdded_ReturnsFalse()
        {
            var suppression = "SuppressionToRemove";
            Assert.IsFalse(Debug.RemoveSuppression(suppression));
        }

        [TestMethod]
        public void RemoveAllowed_WhenNotAdded_ReturnsFalse()
        {
            var allowed = "AllowedToRemove";
            Assert.IsFalse(Debug.RemoveAllowed(allowed));
        }

        [TestMethod]
        public void WriteLine_WithSuppressed_DoesNotWrite()
        {
            var suppressed = "Suppressed";
            Assert.IsTrue(Debug.AddSuppression(suppressed), "Suppression failed to add.");
            var notExpected = "This message should be missing.";
            Debug.WriteLine((object)suppressed, Evands.Pellucid.Terminal.ColorCode.Blue, notExpected);
            Assert.IsFalse(this.writer.Contains(notExpected));
            Debug.RemoveSuppression(suppressed);
        }

        [TestMethod]
        public void GetMessageHeader_ReturnsColoredHeader_WithoutTrailingSpaceColored()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            var headerText = "HeaderTest";
            var expected = ConsoleBase.Colors.Subtle.FormatText(string.Format("[{0}]", headerText)) + " ";
            var actual = Debug.GetMessageHeader(headerText, false, true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteLineWithLevelWritesExpectedLevel()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = false;

            var expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Debug.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Debug, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Notice.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Notice, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[1]);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Error.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Error, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[2]);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Progress.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Progress, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[3]);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Success.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Success, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[4]);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Warning.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Warning, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[5]);
        }

        [TestMethod]
        public void WriteWithLevelWritesExpectedLevel()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = false;

            Debug.Write("Test");
            this.writer.Messages.Clear();

            var expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Debug.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Debug, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Notice.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Notice, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[1]);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Error.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Error, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[2]);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Progress.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Progress, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[3]);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Success.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Success, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[4]);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Warning.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Warning, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[5]);
        }

        [TestMethod]
        public void WriteWithLevelWritesExpectedLevelWithHeader()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = false;

            var expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Debug.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Debug, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Notice.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Notice, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Error.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Error, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Progress.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Progress, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Success.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Success, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Warning.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Warning, "DebugTests", "Test Message with {0}.", "Argument");
            Assert.AreEqual(expected, this.writer.Messages[0]);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();
        }

        private class ThrowAway1
        {
        }

        private class ThrowAway2
        {
        }
    }
}
