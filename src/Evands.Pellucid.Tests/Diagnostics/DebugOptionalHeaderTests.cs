using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Evands.Pellucid;
using System.Diagnostics;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Summary description for DebugTests
    /// </summary>
    public class DebugOptionalHeaderTests
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
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
        }

        [Test]
        public async Task When_ConsoleBaseOptionalHeader_IsEmpty_NoPrefixWritten()
        {
            var invalidStart = "[01]";
            var expectedContents = "Test Message";
            Debug.WriteLine(expectedContents);
            var msg = writer.Messages.Last();
            await Assert.That(!msg.StartsWith(invalidStart) && msg.Contains(expectedContents)).IsTrue();
        }

        [Test]
        public async Task When_ConsoleBaseOptionalHeader_IsNotEmpty_PrefixWritten()
        {
            ConsoleBase.WriteLine();
            var expectedHeader = "[OptionalHeader]";
            ConsoleBase.OptionalHeader = expectedHeader;
            var expectedContents = "Test Message";
            Debug.WriteLine("");
            Debug.WriteLine(this, Evands.Pellucid.Terminal.ColorCode.None, expectedContents);
            var msg = writer.Messages.Last();
            Trace.WriteLine(msg);
            await Assert.That(msg.Contains(expectedHeader)).IsTrue();
            await Assert.That(msg.Contains(expectedContents)).IsTrue();
        }
    }
}
