using System;
namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Summary description for DebugWriteException
    /// </summary>
    public class DebugWriteExceptionTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(this.writer);
        }

        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            this.writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(this.writer);
            ConsoleBase.OptionalHeader = string.Empty;
        }
        private class TestException : Exception
        {
            public TestException(string message, bool value, Exception innerException)
                : base(message, innerException)
            {
                this.Value = value;
            }

            public bool Value { get; private set; }
        }

        [Test]
        public async Task WriteException_UsesConsoleNewLine()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            Options.Instance.UseTimestamps = false;
            ConsoleBase.NewLine = "\r\n";
            var inner = new TestException("Inner Exception", false, null);
            var ex = new TestException("Simple Message", true, inner);
            Debug.WriteException("Test", ex, "Exception encountered when doing test stuff.");
            var temp = ex.ToString();
#if NET6_0_OR_GREATER
            var expected = "[Test] Exception encountered when doing test stuff.\r\n--------Exception 1--------\r\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Simple Message\r\n ---> Evands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\r\n   --- End of inner exception stack trace ---\r\n-----------------------------\r\n--------Exception 2--------\r\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\r\n-----------------------------\r\n";
#else
            var expected = "[Test] Exception encountered when doing test stuff.\r\n--------Exception 1--------\r\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Simple Message ---> Evands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\r\n   --- End of inner exception stack trace ---\r\n-----------------------------\r\n--------Exception 2--------\r\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\r\n-----------------------------\r\n";
#endif
            await Assert.That(this.writer.Last()).IsEqualTo(expected);
        }

        [Test]
        public async Task WriteException_UsesDifferentConsoleNewLine()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            Options.Instance.UseTimestamps = false;
            ConsoleBase.NewLine = "\n";
            var inner = new TestException("Inner Exception", false, null);
            var ex = new TestException("Simple Message", true, inner);
            Debug.WriteException("Test", ex, "Exception encountered when doing test stuff.");

#if NET6_0_OR_GREATER
            var expected = "[Test] Exception encountered when doing test stuff.\n--------Exception 1--------\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Simple Message\n ---> Evands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\n   --- End of inner exception stack trace ---\n-----------------------------\n--------Exception 2--------\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\n-----------------------------\n";
#else
            var expected = "[Test] Exception encountered when doing test stuff.\n--------Exception 1--------\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Simple Message ---> Evands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\n   --- End of inner exception stack trace ---\n-----------------------------\n--------Exception 2--------\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\n-----------------------------\n";
#endif

            await Assert.That(this.writer.Last()).IsEqualTo(expected);
        }
    }
}
