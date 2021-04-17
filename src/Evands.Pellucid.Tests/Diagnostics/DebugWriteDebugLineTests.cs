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
    public class DebugWriteDebugLineTests
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

        private string[] linesToTest = new string[]
        {
            "{ \"Message\": \"MessageContents\", \"Item\": { \"ItemName\": \"Name\" } }",
            "Just a simple string with {0} a brace inside it.",
            "A string with {0} formatting braces and {other braces}"
        };

        private string[][] formatLinesToTest = new string[][]
        {
            new string[] { "This is a {0} message.", "format test" }
        };

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

        #region WriteLines

        [TestMethod]
        public void WriteDebugLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Debug.WriteDebugLine(this, linesToTest[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesToTest[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteDebugLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteDebugLine(this, linesToTest[2], "Something");
        }

        #endregion

        #region Writes

        [TestMethod]
        public void WriteDebug_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Debug.WriteDebug(this, linesToTest[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesToTest[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteDebug_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteDebug(this, linesToTest[2], "Something");
        }

        #endregion
    }
}
