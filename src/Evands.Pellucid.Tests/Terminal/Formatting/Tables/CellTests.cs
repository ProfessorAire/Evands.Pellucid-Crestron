using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for CellTests
    /// </summary>
    [TestClass]
    public class CellTests
    {
        public CellTests()
        {
        }

        private TestContext testContextInstance;

        [TestInitialize]
        public void TestSetup()
        {
            UnderTest = new Cell();
        }

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

        public Cell UnderTest { get; set; }

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
        public void DefaultCtor_Creates_Empty_Cell()
        {
            Assert.IsNotNull(UnderTest);
            Assert.IsTrue(UnderTest.Contents == string.Empty);
            Assert.IsTrue(UnderTest.Color == ColorFormat.None);
        }

        [TestMethod]
        public void Contents_Ctor_Creates_Cell_WithContents_NoColor()
        {
            var expected = "Test Value";
            UnderTest = new Cell(expected);
            Assert.IsNotNull(UnderTest);
            Assert.IsTrue(UnderTest.Contents == expected);
            Assert.IsTrue(UnderTest.Color == ColorFormat.None);
        }

        [TestMethod]
        public void ContentsAndColor_Ctor_Creates_Cell_WithContentsAndColor()
        {
            var expectedContents = "Test Value";
            var expectedColorFormat = ConsoleBase.Colors.BrightGreen;
            UnderTest = new Cell(expectedContents, expectedColorFormat);
            Assert.IsNotNull(UnderTest);
            Assert.IsTrue(UnderTest.Contents == expectedContents);
            Assert.IsTrue(UnderTest.Color == expectedColorFormat);
        }

        [TestMethod]
        public void ColorFormat_GetsSets_CorrectValue()
        {
            var expectedColorFormat = ConsoleBase.Colors.BrightGreen;
            UnderTest.Color = expectedColorFormat;
            Assert.IsTrue(UnderTest.Color == expectedColorFormat);
        }

        [TestMethod]
        public void HorizontalAlignment_GetsSets_CorrectValue()
        {
            var expected = HorizontalAlignment.Center;
            UnderTest.HorizontalAlignment = expected;
            Assert.IsTrue(UnderTest.HorizontalAlignment == expected);
            expected = HorizontalAlignment.Left;
            UnderTest.HorizontalAlignment = expected;
            Assert.IsTrue(UnderTest.HorizontalAlignment == expected);
            expected = HorizontalAlignment.Right;
            UnderTest.HorizontalAlignment = expected;
            Assert.IsTrue(UnderTest.HorizontalAlignment == expected);
        }

        [TestMethod]
        public void Contents_GetsSets_CorrectValue()
        {
            var expected = "This is a test.";
            UnderTest.Contents = expected;
            Assert.IsTrue(UnderTest.Contents == expected);
        }

        [TestMethod]
        public void GetTotalWidth_Returns_AccurateValue_WhenTextHasColorFormatting()
        {
            var text = "This is just some text.";
            var content = ConsoleBase.Colors.BrightRed.FormatText(text);
            UnderTest.Contents = content;
            Assert.IsTrue(UnderTest.GetTotalWidth() == text.Length);
        }

        [TestMethod]
        public void GetTotalWidth_Returns_AccurateValue_WhenTextHasNoColorFormatting()
        {
            var expected = "This is just a text test.";
            UnderTest.Contents = expected;
            Assert.IsTrue(UnderTest.GetTotalWidth() == expected.Length);
        }

        [TestMethod]
        public void GetNumberOfLines_IsAccurate()
        {
            var text = "Line1\nLine2\nLine3";
            UnderTest.Contents = text;
            Assert.IsTrue(UnderTest.GetNumberOfLines(100) == 3);
            text = "Line1Line2Line3Line4";
            UnderTest.Contents = text;
            Assert.IsTrue(UnderTest.GetNumberOfLines(5) == 4);
            Assert.IsTrue(UnderTest.GetNumberOfLines(10) == 2);
            text = "Some text.";
            UnderTest.Contents = text;
            Assert.IsTrue(UnderTest.GetNumberOfLines(100) == 1);
        }

        [TestMethod]
        public void GetLine_Returns_CorrectValue_WithoutColor()
        {
            var text = "Line1Line2Line3Line4";
            var items = new string[] { "Line1", "Line2", "Line3", "Line4" };

            UnderTest.Contents = text;

            for (var i = 0; i < items.Length; i++)
            {
                Assert.IsTrue(UnderTest.GetLine(i, 5, false) == items[i]);
            }
        }

        [TestMethod]
        public void GetLine_Returns_CorrectValue_WithColor()
        {
            var text = "Line1\nLine2\nLine3\nLine4";
            UnderTest.Contents = text;
            UnderTest.Color = ConsoleBase.Colors.BrightRed;

            var items = text.Split('\n');

            for (var i = 0; i < items.Length; i++)
            {
                var line = UnderTest.GetLine(i, 5, true);
                var expected = UnderTest.Color.FormatText(items[i]);
                Assert.IsTrue(line == expected, "Index '{0}' failed.", i);
            }            
        }

        [TestMethod]
        public void GetLine_Returns_LeftAlignedValue()
        {
            var text = "This is some long text.";
            var width = text.Length + 10;
            UnderTest.Contents = text;
            UnderTest.HorizontalAlignment = HorizontalAlignment.Left;

            Assert.IsTrue(UnderTest.GetLine(0, width, false) == text.PadRight(width));
        }

        [TestMethod]
        public void GetLine_Returns_CenterAlignedValue()
        {
            var text = "This is some long text.";
            var width = text.Length + 10;
            UnderTest.Contents = text;
            UnderTest.HorizontalAlignment = HorizontalAlignment.Center;

            Assert.IsTrue(UnderTest.GetLine(0, width, false) == text.Pad(width));
        }

        [TestMethod]
        public void GetLine_Returns_RightAlignedValue()
        {
            var text = "This is some long text.";
            var width = text.Length + 10;
            UnderTest.Contents = text;
            UnderTest.HorizontalAlignment = HorizontalAlignment.Right;

            Assert.IsTrue(UnderTest.GetLine(0, width, false) == text.PadLeft(width));
        }
    }
}
