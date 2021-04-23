using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid
{
    /// <summary>
    /// Summary description for ConsoleBaseTests
    /// </summary>
    [TestClass]
    public class ConsoleBaseTests
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

        public ConsoleBaseTests()
        {
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
        public void HeaderTextIsEmptyByDefault()
        {
            Assert.IsTrue(string.IsNullOrEmpty(ConsoleBase.OptionalHeader));
        }

        [TestMethod]
        public void HeaderText_GetSet_Functions()
        {
            var expected = "01";
            ConsoleBase.OptionalHeader = expected;
            Assert.IsTrue(ConsoleBase.OptionalHeader == string.Format("[{0}]", expected));

            ConsoleBase.OptionalHeader = string.Empty;
            Assert.IsTrue(ConsoleBase.OptionalHeader == string.Empty);
        }

        [TestMethod]
        public void WriteLine_WithEmptyHeader_WritesLineWithNoHeader()
        {
            var invalidStart = "[01]";
            var expectedEnd = "Test Message";
            ConsoleBase.WriteLine(expectedEnd);
            var msg = writer.Messages.Last();
            Assert.IsTrue(!msg.StartsWith(invalidStart) && msg.Contains(expectedEnd));
        }

        [TestMethod]
        public void WriteLine_WithNonEmptyHeader_WritesLineWithHeader()
        {
            ConsoleBase.OptionalHeader = "01";
            var expectedStart = "[01]";
            var expectedEnd = "Test Message";
            ConsoleBase.WriteLine(expectedEnd);
            var msg = writer.Messages.Last();
            Assert.IsTrue(msg.StartsWith(expectedStart) && msg.Contains(expectedEnd));
        }
    }
}
