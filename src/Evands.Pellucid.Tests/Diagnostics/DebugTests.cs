using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evands.Pellucid;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Summary description for DebugTests
    /// </summary>
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

        [TestMethod]
        public void When_ConsoleBaseOptionalHeader_IsEmpty_NoPrefixWritten()
        {
            var invalidStart = "[01]";
            var expectedContents = "Test Message";
            Debug.WriteLine(expectedContents);
            var msg = writer.Messages.Last();
            Assert.IsTrue(!msg.StartsWith(invalidStart) && msg.Contains(expectedContents));
        }

        [TestMethod]
        public void When_ConsoleBaseOptionalHeader_IsNotEmpty_PrefixWritten()
        {
            var expectedStart = "[OptionalHeader]";
            ConsoleBase.OptionalHeader = expectedStart;
            var expectedContents = "Test Message";
            Debug.WriteLine(expectedContents);
            var msg = writer.Messages.Last();

            Assert.IsTrue(msg.StartsWith(expectedStart) && msg.Contains(expectedContents));
        }
    }
}
