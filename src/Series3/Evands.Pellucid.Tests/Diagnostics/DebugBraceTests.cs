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
    public class DebugBraceTests
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

        private string[] linesWithBraces = new string[]
        {
            "{ \"Message\": \"MessageContents\", \"Item\": { \"ItemName\": \"Name\" } }",
            "Just a simple string with {0} a brace inside it.",
            "A string with {0} formatting braces and {other braces}"
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
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteDebugLine(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteDebugLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteDebugLine(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteProgressLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteProgressLine(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteProgressLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteProgressLine(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteWarningLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteWarningLine(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteWarningLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteWarningLine(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteErrorLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteErrorLine(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteErrorLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteErrorLine(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteNoticeLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteNoticeLine(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteNoticeLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteNoticeLine(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteSuccessLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteSuccessLine(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteSuccessLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteSuccessLine(this, linesWithBraces[2], "Something");
        }

        #endregion

        #region Writes

        [TestMethod]
        public void WriteDebug_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteDebug(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteDebug_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteDebug(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteProgress_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteProgress(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteProgress_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteProgress(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteWarning_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteWarning(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteWarning_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteWarning(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteError_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteError(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteError_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteError(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteNotice_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteNotice(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteNotice_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteNotice(this, linesWithBraces[2], "Something");
        }

        [TestMethod]
        public void WriteSuccess_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteSuccess(this, linesWithBraces[i]);
                Assert.IsTrue(writer.Messages.Last().Contains(linesWithBraces[i]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WriteSuccess_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Debug.WriteSuccess(this, linesWithBraces[2], "Something");
        }

        #endregion
    }
}
