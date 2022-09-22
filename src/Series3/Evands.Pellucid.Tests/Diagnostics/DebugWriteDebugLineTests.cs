using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evands.Pellucid;
using System.Diagnostics;

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
            Options.Instance.MaxDebugMessageLength = -1;
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

        #region WithLimits

        [TestMethod]
        public void Write_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.Write((object)null, ConsoleBase.Colors.Green, "12345678900987654321ABC");
            Console.Write(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("123456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.Write((object)null, ConsoleBase.Colors.Green, "22345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("223456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteDebug_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteDebug(null, "32345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("323456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteDebugLine_With_Limit_Prints_Expected()
        {
            const string expected = "4234567890098<...>1ABC";
            Options.Instance.MaxDebugMessageLength = 21;
            Options.Instance.ColorizeConsoleOutput = false;
            Debug.WriteDebugLine(null, "42345678900987654321ABC");
            System.Diagnostics.Debug.WriteLine(String.Format("Expected: {0}", expected));
            System.Diagnostics.Debug.WriteLine(String.Format("  Actual: {0}", writer.Messages.Last()));
            Assert.IsTrue(writer.Messages.Last().Contains(expected));
        }

        [TestMethod]
        public void WriteSuccess_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteSuccess(null, "52345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("523456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteSuccessLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteSuccessLine(null, "62345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("6234567890098<...>1ABC"));
        }

        [TestMethod]
        public void WriteProgress_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteProgress(null, "72345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("723456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteProgressLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteProgressLine(null, "82345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("8234567890098<...>1ABC"));
        }

        [TestMethod]
        public void WriteNotice_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteNotice(null, "92345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("923456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteNoticeLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteNoticeLine(null, "02345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("0234567890098<...>1ABC"));
        }

        [TestMethod]
        public void WriteWarning_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteWarning(null, "82345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("823456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteWarningLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteWarningLine(null, "72345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("7234567890098<...>1ABC"));
        }

        [TestMethod]
        public void WriteError_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteError(null, "62345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("623456789009<...>1ABC"));
        }

        [TestMethod]
        public void WriteErrorLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteErrorLine(null, "52345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            Assert.IsTrue(writer.Messages.Last().Contains("5234567890098<...>1ABC"));
        }

        #endregion
    }
}
