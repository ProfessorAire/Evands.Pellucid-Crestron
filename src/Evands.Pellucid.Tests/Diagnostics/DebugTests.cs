using System;
namespace Evands.Pellucid.Diagnostics
{
    public class DebugTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(this.writer);
            this.writer.Messages.Clear();
        }

        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            this.writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(this.writer);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.Suppressed.Clear();
            Options.Instance.Allowed.Clear();
        }

        [Test]
        public async Task RegisterHeaderObject_AsString_AddsValueToHeaders_WithColor()
        {
            var value = "TestValue4321";
            var expected = ConsoleBase.Colors.Cyan;
            Debug.RegisterHeaderObject(value, expected);

            var actual = Debug.RegisteredClasses[value];

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task RegisterHeaderObject_AsObject_AddsValueToHeaders_WithColor()
        {
            var value = new ThrowAway1();
            var name = value.GetType().FullName;
            var expected = ConsoleBase.Colors.Cyan;
            Debug.RegisterHeaderObject(value, expected);

            var actual = Debug.RegisteredClasses[name];

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task RegisterHeaderObject_WhenAlreadyExisting_ReplacesColor()
        {
            var value = new ThrowAway2();
            var name = value.GetType().FullName;
            var expected = ConsoleBase.Colors.Cyan;

            Debug.RegisterHeaderObject(value, expected);
            var actual = Debug.RegisteredClasses[name];
            await Assert.That(actual).IsEqualTo(expected);
            expected = ConsoleBase.Colors.BrightYellow;

            Debug.RegisterHeaderObject(value, expected);
            actual = Debug.RegisteredClasses[name];
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task AddSuppression_AddsItem_To_SuppressionList()
        {
            var suppression = "Suppression";
            await Assert.That(Debug.AddSuppression(suppression)).IsTrue();
            await Assert.That(Options.Instance.Suppressed.Contains(suppression)).IsTrue();
        }

        [Test]
        public async Task AddAllowed_AddsItem_To_AllowedList()
        {
            var allowed = "Allowed";
            await Assert.That(Debug.AddAllowed(allowed)).IsTrue();
            await Assert.That(Options.Instance.Allowed.Contains(allowed)).IsTrue();
        }

        [Test]
        public async Task RemoveSuppression_RemovesItem_From_SuppressionList()
        {
            var suppression = "SuppressionToRemove";
            await Assert.That(Debug.AddSuppression(suppression)).IsTrue();
            await Assert.That(Options.Instance.Suppressed.Contains(suppression)).IsTrue();
            await Assert.That(Debug.RemoveSuppression(suppression)).IsTrue();
            await Assert.That(Options.Instance.Suppressed.Contains(suppression)).IsFalse();
        }

        [Test]
        public async Task RemoveAllowed_RemovesItem_From_AllowedList()
        {
            var allowed = "AllowedToRemove";
            await Assert.That(Debug.AddAllowed(allowed)).IsTrue();
            await Assert.That(Options.Instance.Allowed.Contains(allowed)).IsTrue();
            await Assert.That(Debug.RemoveAllowed(allowed)).IsTrue();
            await Assert.That(Options.Instance.Allowed.Contains(allowed)).IsFalse();
        }

        [Test]
        public async Task RemoveSuppression_WhenNotAdded_ReturnsFalse()
        {
            var suppression = "SuppressionToRemove";
            await Assert.That(Debug.RemoveSuppression(suppression)).IsFalse();
        }

        [Test]
        public async Task RemoveAllowed_WhenNotAdded_ReturnsFalse()
        {
            var allowed = "AllowedToRemove";
            await Assert.That(Debug.RemoveAllowed(allowed)).IsFalse();
        }

        [Test]
        public async Task WriteLine_WithSuppressed_DoesNotWrite()
        {
            var suppressed = "Suppressed";
            await Assert.That(Debug.AddSuppression(suppressed)).IsTrue();
            var notExpected = "This message should be missing.";
            Debug.WriteLine((object)suppressed, Evands.Pellucid.Terminal.ColorCode.Blue, notExpected);
            await Assert.That(this.writer.Contains(notExpected)).IsFalse();
            Debug.RemoveSuppression(suppressed);
        }

        [Test]
        public async Task GetMessageHeader_ReturnsColoredHeader_WithoutTrailingSpaceColored()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            var headerText = "HeaderTest";
            var expected = ConsoleBase.Colors.Subtle.FormatText(string.Format("[{0}]", headerText)) + " ";
            var actual = Debug.GetMessageHeader(headerText, false, true);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task WriteLineWithLevelWritesExpectedLevel()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = false;
            Debug.WriteLine(string.Empty);
            this.writer.Messages.Clear();

            var expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Debug.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Debug, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Notice.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Notice, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[1]).IsEqualTo(expected);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Error.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Error, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[2]).IsEqualTo(expected);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Progress.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Progress, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[3]).IsEqualTo(expected);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Success.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Success, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[4]).IsEqualTo(expected);

            expected = string.Format(
                "{0} {1}\r\n",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Warning.FormatText(true, "Test Message with Argument."));
            Debug.WriteLine(DebugLevels.Warning, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[5]).IsEqualTo(expected);
        }

        [Test]
        public async Task WriteWithLevelWritesExpectedLevel()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = false;

            Debug.Write("Test");
            this.writer.Messages.Clear();

            var expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Debug.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Debug, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Notice.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Notice, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[1]).IsEqualTo(expected);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Error.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Error, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[2]).IsEqualTo(expected);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Progress.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Progress, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[3]).IsEqualTo(expected);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Success.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Success, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[4]).IsEqualTo(expected);

            expected = string.Format(
                "{0}",
                ConsoleBase.Colors.Warning.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Warning, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[5]).IsEqualTo(expected);
        }

        [Test]
        public async Task WriteWithLevelWritesExpectedLevelWithHeader()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = false;

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            var expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Debug.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Debug, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Notice.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Notice, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Error.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Error, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Progress.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Progress, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Success.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Success, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

            Debug.WriteLine("Test");
            this.writer.Messages.Clear();

            expected = string.Format(
                "{0} {1}",
                ConsoleBase.Colors.Subtle.FormatText(true, "[DebugTests]"),
                ConsoleBase.Colors.Warning.FormatText(true, "Test Message with Argument."));
            Debug.Write(DebugLevels.Warning, "DebugTests", "Test Message with {0}.", "Argument");
            await Assert.That(this.writer.Messages[0]).IsEqualTo(expected);

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
