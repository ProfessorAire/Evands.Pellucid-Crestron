using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    [TestClass]
    public class DumpNodeTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [TestInitialize]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
            Options.Instance.ColorizeConsoleOutput = false;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.ColorizeConsoleOutput = true;
        }

        public DumpNodeTests()
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

        [TestMethod]
        public void ToString_BasicValue_NoParameters_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_BasicValue_MaxDepth0_And_CurrentDepthNegative_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(0, -1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_BasicValue_MaxDepthTwo_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_BasicValue_FullNamesFalse_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_BasicValue_MaxDepthOne_And_FullNamesFalse_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(1, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_BasicValue_NoName_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", string.Empty);
            var expected = "\"TestValue\"";
            var actual = underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_NullValue_NoName_Writes_Correct()
        {
            var underTest = new DumpNode(null, string.Empty);
            var expected = "<null>";
            var actual = underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_NullValue_WithName_Writes_Correct()
        {
            var underTest = new DumpNode(null, "TestName");
            var expected = "TestName = <null>";
            var actual = underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CTor_WithNullName_Creates_WithEmptyName()
        {
            var underTest = new DumpNode("TestValue", null);
            Assert.AreEqual(string.Empty, underTest.Name);
        }

        [TestMethod]
        public void CTor_AssignsName_Correctly()
        {
            var expected = "TestName";
            var underTest = new DumpNode("TestValue", expected);
            Assert.AreEqual(expected, underTest.Name);
        }

        [TestMethod]
        public void CTor_AssignsStringValue_WithQuotes()
        {
            var expected = "TestValue";
            var underTest = new DumpNode(expected, "TestName");
            Assert.AreEqual("\"" + expected + "\"", underTest.Value);
        }

        [TestMethod]
        public void CTor_AssignsNonStringValue_Directly()
        {
            var expected = 1234;
            var underTest = new DumpNode(expected, "TestName");
            Assert.AreEqual(expected, underTest.Value);
        }
    }
}
