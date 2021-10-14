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
        public void GetMessageHeader_ReturnsColoredHeader_WithoutTrailingSpaceColored()
        {
            Options.Instance.ColorizeConsoleOutput = true;
            var headerText = "HeaderTest";
            var expected = ConsoleBase.Colors.Subtle.FormatText(string.Format("[{0}]", headerText)) + " ";
            var actual = Debug.GetMessageHeader(headerText, false, true);
            Assert.AreEqual(expected, actual);
        }
    }
}
