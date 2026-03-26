using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid
{
    public class OptionsTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
        }

        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
        }

        [Test]
        public async Task UseDefault_CreatesDefaults_WithAutoSave_Disabled()
        {
            Options.UseDefault();
            await Assert.That(Options.Instance.ColorizeConsoleOutput).IsTrue();
            await Assert.That(Options.Instance.UseTimestamps).IsTrue();
            await Assert.That(Options.Instance.Use24HourTime).IsTrue();
            await Assert.That(Evands.Pellucid.Diagnostics.LogLevels.None).IsEqualTo(Options.Instance.LogLevels);
            await Assert.That(Evands.Pellucid.Diagnostics.DebugLevels.All).IsEqualTo(Options.Instance.DebugLevels);
            await Assert.That(Options.Instance.Suppressed.Count == 0).IsTrue();
            await Assert.That(Options.Instance.Allowed.Count == 0).IsTrue();
            await Assert.That(Options.Instance.AutoSave).IsFalse();
            await Assert.That(Options.Instance.UseFullTypeNamesWhenDumping).IsFalse();
        }

        [Test]
        public async Task DefaultLogTimestampFormat_Gets_Expected_Default()
        {
            const string expected = "yy/MM/dd HH:mm:ss";
            var actual = Options.Instance.DefaultLogTimestampFormat;

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task DefaultLogTimestampFormat_Sets_Gets_Expected()
        {
            const string expected = "ss:mm:HH dd/MM/yy";
            Options.Instance.DefaultLogTimestampFormat = expected;
            var actual = Options.Instance.DefaultLogTimestampFormat;
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task MaxDebugMessageLength_Get_Returns_Expected_Default()
        {
            const int expected = -1;
            var actual = Options.Instance.MaxDebugMessageLength;

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task MaxDebugMessageLength_Sets_Gets_Expected()
        {
            const int expected = 120;
            Options.Instance.MaxDebugMessageLength = expected;
            var actual = Options.Instance.MaxDebugMessageLength;

            await Assert.That(actual).IsEqualTo(expected);
        }
    }
}
