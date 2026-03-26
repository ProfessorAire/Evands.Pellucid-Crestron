using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Evands.Pellucid
{
    /// <summary>
    /// Summary description for ConsoleBaseTests
    /// </summary>
    public class ConsoleBaseTests
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
            ConsoleBase.OptionalHeader = string.Empty;
        }

        public ConsoleBaseTests()
        {
        }

        [Test]
        public async Task HeaderTextIsEmptyByDefault()
        {
            await Assert.That(string.IsNullOrEmpty(ConsoleBase.OptionalHeader)).IsTrue();
        }

        [Test]
        public async Task HeaderText_GetSet_Functions()
        {
            var expected = "01";
            ConsoleBase.OptionalHeader = expected;
            await Assert.That(ConsoleBase.OptionalHeader == string.Format("[{0}]", expected)).IsTrue();

            ConsoleBase.OptionalHeader = string.Empty;
            await Assert.That(ConsoleBase.OptionalHeader == string.Empty).IsTrue();
        }

        [Test]
        public async Task WriteLine_WithEmptyHeader_WritesLineWithNoHeader()
        {
            var invalidStart = "[01]";
            var expectedEnd = "Test Message";
            ConsoleBase.WriteLine(expectedEnd);
            var msg = writer.Messages.Last();
            await Assert.That(!msg.StartsWith(invalidStart) && msg.Contains(expectedEnd)).IsTrue();
        }

        [Test]
        public async Task WriteLine_WithNonEmptyHeader_WritesLineWithHeader()
        {
            ConsoleBase.OptionalHeader = "01";
            var expectedStart = "[01]";
            var expectedEnd = "Test Message";
            ConsoleBase.WriteLine(expectedEnd);
            var msg = writer.Messages.Last();
            await Assert.That(msg.StartsWith(expectedStart) && msg.Contains(expectedEnd)).IsTrue();
        }
    }
}
