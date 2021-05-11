using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Summary description for DebugWriteException
    /// </summary>
    [TestClass]
    public class DebugWriteExceptionTests
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
            Options.Instance.ColorizeConsoleOutput = true;
            Options.Instance.UseTimestamps = true;
            ConsoleBase.NewLine = "\r\n";
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

        private class TestException : Exception
        {
            public TestException(string message, bool value, Exception innerException)
                : base(message, innerException)
            {
                Value = value;
            }

            public bool Value { get; private set; }
        }

        [TestMethod]
        public void WriteException_UsesConsoleNewLine()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            Options.Instance.UseTimestamps = false;
            var inner = new TestException("Inner Exception", false, null);
            var ex = new TestException("Simple Message", true, inner);
            Debug.WriteException("Test", ex, "Exception encountered when doing test stuff.");

            var expected = "[Test] Exception encountered when doing test stuff.\r\n--------Exception 1--------\r\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Simple Message ---> Evands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\r\n   --- End of inner exception stack trace ---\r\n-----------------------------\r\n--------Exception 2--------\r\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\r\n-----------------------------\r\n";

            Assert.AreEqual(expected, writer.Last());
        }

        [TestMethod]
        public void WriteException_UsesDifferentConsoleNewLine()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            Options.Instance.UseTimestamps = false;
            ConsoleBase.NewLine = "\n";
            var inner = new TestException("Inner Exception", false, null);
            var ex = new TestException("Simple Message", true, inner);
            Debug.WriteException("Test", ex, "Exception encountered when doing test stuff.");

            var expected = "[Test] Exception encountered when doing test stuff.\n--------Exception 1--------\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Simple Message ---> Evands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\n   --- End of inner exception stack trace ---\n-----------------------------\n--------Exception 2--------\nEvands.Pellucid.Diagnostics.DebugWriteExceptionTests+TestException: Inner Exception\n-----------------------------\n";

            Assert.AreEqual(expected, writer.Last());
        }
    }
}
