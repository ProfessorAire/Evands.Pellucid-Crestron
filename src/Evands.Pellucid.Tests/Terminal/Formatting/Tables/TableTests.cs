using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TableTests
    {
        public TableTests()
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

        private string[] row1 = new string[] { "R1C1", "R1C2", "R1C3" };
        private string[] row2 = new string[] { "R2C1", "R2C2", "R2C3" };
        private string[] head = new string[] { "H1", "H2", "H3" };

        private string[] brokenRow = new string[] { "This is a long string that spans three lines.", "This is a long string that spans three lines.", "This is a long string that spans three lines." };

        private string GetExpectedTable(HorizontalAlignment a, int maxCellWidth)
        {
            var sb = new StringBuilder();

            if (maxCellWidth == 0)
            {
                maxCellWidth = 4;
            }

            var totalWidth = 10 + maxCellWidth * 3;

            sb.Append('-', totalWidth);
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", head[0].Align(a, maxCellWidth), head[1].Align(a, maxCellWidth), head[2].Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.Append('-', totalWidth);
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", row1[0].Align(a, maxCellWidth), row1[1].Align(a, maxCellWidth), row1[2].Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.Append('|');

            for (var i = 0; i < 3; i++)
            {
                sb.Append('-', maxCellWidth + 2);
                if (i < 2)
                {
                    sb.Append('+');
                }
            }

            sb.Append("|\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", row2[0].Align(a, maxCellWidth), row2[1].Align(a, maxCellWidth), row2[2].Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.Append('-', totalWidth);
            sb.Append("\r\n");
            return sb.ToString();
        }

        private string GetExpectedBrokenTable(HorizontalAlignment a, int maxCellWidth)
        {
            var sb = new StringBuilder();

            if (maxCellWidth == 0)
            {
                maxCellWidth = 4;
            }

            var split1 = "This is a long";
            var split2 = "string that spans";
            var split3 = "three lines.";

            var totalWidth = 10 + maxCellWidth * 3;

            sb.Append('-', totalWidth);
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split1.Align(a, maxCellWidth), split1.Align(a, maxCellWidth), split1.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split2.Align(a, maxCellWidth), split2.Align(a, maxCellWidth), split2.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split3.Align(a, maxCellWidth), split3.Align(a, maxCellWidth), split3.Align(a, maxCellWidth));
            sb.Append("\r\n").Append('-', totalWidth).Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split1.Align(a, maxCellWidth), split1.Align(a, maxCellWidth), split1.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split2.Align(a, maxCellWidth), split2.Align(a, maxCellWidth), split2.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split3.Align(a, maxCellWidth), split3.Align(a, maxCellWidth), split3.Align(a, maxCellWidth));
            sb.Append("\r\n|");
            for (var i = 0; i < 3; i++)
            {
                sb.Append('-', maxCellWidth + 2);
                if (i < 2)
                {
                    sb.Append('+');
                }
            }

            sb.Append("|\r\n");

            sb.AppendFormat("| {0} | {1} | {2} |", split1.Align(a, maxCellWidth), split1.Align(a, maxCellWidth), split1.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split2.Align(a, maxCellWidth), split2.Align(a, maxCellWidth), split2.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.AppendFormat("| {0} | {1} | {2} |", split3.Align(a, maxCellWidth), split3.Align(a, maxCellWidth), split3.Align(a, maxCellWidth));
            sb.Append("\r\n");
            sb.Append('-', totalWidth);
            sb.Append("\r\n");
            return sb.ToString();
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
        public void Table_Create_ReturnsNewTable()
        {
            var t = Table.Create();
            Assert.IsNotNull(t);
        }

        [TestMethod]
        public void Table_Ctors_CreateMinimumColumnWidthOfZero()
        {
            var t = new Table();
            Assert.IsTrue(t.MinimumColumnWidth == 0, "Empty ctor failed.");
        }

        [TestMethod]
        public void Table_NumberOfRows_IsAccurate()
        {
            var t = Table.Create();
            Assert.IsTrue(t.NumberOfRows == 0);
            t.AddRow(new string[] { "Value 1", "Value 2" });
            Assert.IsTrue(t.NumberOfRows == 1);
            t.AddRow(new string[] { "Value 3", "Value 4" });
            Assert.IsTrue(t.NumberOfRows == 2);
            t.AddRow(new Cell[] { new Cell("Value 5"), new Cell("Value 6") });
            Assert.IsTrue(t.NumberOfRows == 3);
        }

        [TestMethod]
        public void Table_NumberOfColumns_IsAccurate_WhenAddingColumns()
        {
            var t = Table.Create();
            Assert.IsTrue(t.NumberOfColumns == 0);
            for (var i = 1; i < 5; i++)
            {
                t.AddColumn("Cell", "Cell");
                Assert.IsTrue(t.NumberOfColumns == i);
            }
        }

        [TestMethod]
        public void Table_MinimumColumWidth_IsAccurate()
        {
            var t = Table.Create();
            Assert.IsTrue(t.MinimumColumnWidth == 0);
            t.MinimumColumnWidth = 10;
            Assert.IsTrue(t.MinimumColumnWidth == 10);
        }

        [TestMethod]
        public void Table_SetMinimumColumWidth_SetsMinimumColumnWidth()
        {
            var t = Table.Create();
            Assert.IsTrue(t.MinimumColumnWidth == 0);
            t.SetMinimumColumnWidth(4);
            Assert.IsTrue(t.MinimumColumnWidth == 4);
        }

        [TestMethod]
        public void Table_HorizontalBorder_GetsSets()
        {
            var t = Table.Create();
            t.HorizontalBorder = 'a';
            Assert.IsTrue(t.HorizontalBorder == 'a');
        }

        [TestMethod]
        public void Table_VerticalBorder_GetsSets()
        {
            var t = Table.Create();
            t.VerticalBorder = 'b';
            Assert.IsTrue(t.VerticalBorder == 'b');
        }

        [TestMethod]
        public void Table_HeaderBottomBorder_GetsSets()
        {
            var t = Table.Create();
            t.HeaderBottomBorder = 'c';
            Assert.IsTrue(t.HeaderBottomBorder == 'c');
        }

        [TestMethod]
        public void Table_WithHeaders_StringArray_AddsHeaders()
        {
            var expectedValues = new string[] { "Header1", "Header2", "Header3" };

            var t = Table.Create()
                .WithHeaders(expectedValues);

            for (var i = 0; i < t.NumberOfColumns; i++)
            {
                Assert.IsTrue(t.Headers[i].Contents == expectedValues[i], "Index {0}", i);
            }
        }

        [TestMethod]
        public void Table_WithHeaders_CellArray_AddsHeaders()
        {
            var expectedValues = new Cell[]
            {
                new Cell("Header1"),
                new Cell("Header2")
            };

            var t = Table.Create().WithHeaders(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Headers[i] == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_AddHeader_String_AddsHeader()
        {
            var expectedValue = "New Header";

            var t = Table
                .Create()
                .WithHeaders("Header1", "Header2")
                .AddHeader(expectedValue);

            Assert.IsTrue(t.Headers[2].Contents == expectedValue);
        }

        [TestMethod]
        public void Table_AddHeader_Cell_AddsHeader()
        {
            var expectedValue = new Cell("New Header");
            var t = Table
                .Create()
                .WithHeaders("Header1", "Header2")
                .AddHeader(expectedValue);

            Assert.IsTrue(t.Headers[2] == expectedValue);
        }

        [TestMethod]
        public void Table_AddRow_Strings_AddsRow()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value3" };
            var t = Table
                .Create()
                .AddRow(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Rows[0][i].Contents == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_AddRow_Cells_AddsRow()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2") };
            var t = Table
                .Create()
                .AddRow(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Rows[0][i] == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_AddColumn_Strings_AddsColumn_WithEmptyHeader()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value3", "Value4" };
            var t = Table
                .Create()
                .AddColumn(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Columns[0][i].Contents == expectedValues[i]);
            }

            Assert.IsTrue(t.Headers[0].Contents == string.Empty);
        }

        [TestMethod]
        public void Table_AddColumn_Cells_AddsColumn_WithEmptyHeader()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2"), new Cell("Value") };
            var t = Table
                .Create()
                .AddColumn(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Columns[0][i] == expectedValues[i]);
            }

            Assert.IsTrue(string.IsNullOrEmpty(t.Headers[0].Contents));
        }

        [TestMethod]
        public void Table_AddColumnWithHeader_StringCells_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2"), new Cell("Value") };
            var expectedHeader = "Header Value";

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            Assert.IsTrue(t.Headers[0].Contents == expectedHeader);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Columns[0][i] == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_AddColumnWithHeader_CellStrings_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value" };
            var expectedHeader = new Cell("Header Value");

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            Assert.IsTrue(t.Headers[0] == expectedHeader);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Columns[0][i].Contents == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_AddColumnWithHeader_StringStrings_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value" };
            var expectedHeader = "Header Value";

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            Assert.IsTrue(t.Headers[0].Contents == expectedHeader);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Columns[0][i].Contents == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_AddColumnWithHeader_CellCells_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2"), new Cell("Value") };
            var expectedHeader = new Cell("Header Value");

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            Assert.IsTrue(t.Headers[0] == expectedHeader);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(t.Columns[0][i] == expectedValues[i]);
            }
        }

        [TestMethod]
        public void Table_FormatHeaders_AlignmentColor()
        {
            var expectedAlign = HorizontalAlignment.Center;
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .WithHeaders("H1", "H2", "H3", "H4")
                .FormatHeaders(expectedColor, expectedAlign);

            for (var i = 0; i < t.Headers.Count; i++)
            {
                Assert.IsTrue(t.Headers[i].HorizontalAlignment == expectedAlign);
                Assert.IsTrue(t.Headers[i].Color == expectedColor);
            }
        }

        [TestMethod]
        public void Table_FormatHeaders_Alignment()
        {
            var expectedAlign = HorizontalAlignment.Center;

            var t = Table
                .Create()
                .WithHeaders("H1", "H2", "H3", "H4")
                .FormatHeaders(expectedAlign);

            for (var i = 0; i < t.Headers.Count; i++)
            {
                Assert.IsTrue(t.Headers[i].HorizontalAlignment == expectedAlign);
            }
        }

        [TestMethod]
        public void Table_FormatHeaders_Color()
        {
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .WithHeaders("H1", "H2", "H3", "H4")
                .FormatHeaders(expectedColor);

            for (var i = 0; i < t.Headers.Count; i++)
            {
                Assert.IsTrue(t.Headers[i].Color == expectedColor);
            }
        }

        [TestMethod]
        public void Table_FormatColumn_AlignmentColor()
        {
            var expectedAlign = HorizontalAlignment.Center;
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .AddColumn("H1", "H2", "H3", "H4")
                .AddColumn("H5", "H6", "H7", "H8")
                .FormatColumn(1, expectedColor, expectedAlign);

            for (var i = 0; i < t.Columns.Count; i++)
            {
                Assert.IsTrue(t.Columns[1][i].HorizontalAlignment == expectedAlign);
                Assert.IsTrue(t.Columns[1][i].Color == expectedColor);
            }
        }

        [TestMethod]
        public void Table_FormatColumn_Alignment()
        {
            var expectedAlign = HorizontalAlignment.Center;

            var t = Table
                .Create()
                .AddColumn("H1", "H2", "H3", "H4")
                .AddColumn("H5", "H6", "H7", "H8")
                .FormatColumn(1, expectedAlign);

            for (var i = 0; i < t.Columns.Count; i++)
            {
                Assert.IsTrue(t.Columns[1][i].HorizontalAlignment == expectedAlign);
            }
        }

        [TestMethod]
        public void Table_FormatColumn_Color()
        {
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .AddColumn("H1", "H2", "H3", "H4")
                .AddColumn("H5", "H6", "H7", "H8")
                .FormatColumn(1, expectedColor);

            for (var i = 0; i < t.Columns.Count; i++)
            {
                Assert.IsTrue(t.Columns[1][i].Color == expectedColor);
            }
        }

        [TestMethod]
        public void Table_FormatRow_AlignmentColor()
        {
            var expectedAlign = HorizontalAlignment.Center;
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .AddRow("H1", "H2", "H3", "H4")
                .AddRow("H5", "H6", "H7", "H8")
                .FormatRow(1, expectedColor, expectedAlign);

            for (var i = 0; i < t.Rows.Count; i++)
            {
                Assert.IsTrue(t.Rows[1][i].HorizontalAlignment == expectedAlign);
                Assert.IsTrue(t.Rows[1][i].Color == expectedColor);
            }
        }

        [TestMethod]
        public void Table_FormatRow_Alignment()
        {
            var expectedAlign = HorizontalAlignment.Center;

            var t = Table
                .Create()
                .AddRow("H1", "H2", "H3", "H4")
                .AddRow("H5", "H6", "H7", "H8")
                .FormatRow(1, expectedAlign);

            for (var i = 0; i < t.Rows.Count; i++)
            {
                Assert.IsTrue(t.Rows[1][i].HorizontalAlignment == expectedAlign);
            }
        }

        [TestMethod]
        public void Table_FormatRow_Color()
        {
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .AddRow("H1", "H2", "H3", "H4")
                .AddRow("H5", "H6", "H7", "H8")
                .FormatRow(1, expectedColor);

            for (var i = 0; i < t.Rows.Count; i++)
            {
                Assert.IsTrue(t.Rows[1][i].Color == expectedColor);
            }
        }

        [TestMethod]
        public void ToString_Formats_Left_Correctly()
        {
            var align = HorizontalAlignment.Left;
            var width = 16;
            var expected = GetExpectedTable(align, width);
            var t = Table.Create()
                .WithHeaders(head)
                .FormatHeaders(align)
                .AddRow(row1)
                .AddRow(row2)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Formats_Center_Correctly()
        {
            var align = HorizontalAlignment.Center;
            var width = 22;
            var expected = GetExpectedTable(align, width);
            var t = Table.Create()
                .WithHeaders(head)
                .FormatHeaders(align)
                .AddRow(row1)
                .AddRow(row2)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Formats_Right_Correctly()
        {
            var align = HorizontalAlignment.Right;
            var width = 12;
            var expected = GetExpectedTable(align, width);
            var t = Table.Create()
                .WithHeaders(head)
                .FormatHeaders(align)
                .AddRow(row1)
                .AddRow(row2)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithMax_Formats_Left_Correctly()
        {
            var align = HorizontalAlignment.Left;
            var width = 0;
            var expected = GetExpectedTable(align, width);
            var t = Table.Create()
                .WithHeaders(head)
                .FormatHeaders(align)
                .AddRow(row1)
                .AddRow(row2)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString(4);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithMax_Formats_Center_Correctly()
        {
            var align = HorizontalAlignment.Center;
            var width = 0;
            var expected = GetExpectedTable(align, width);
            var t = Table.Create()
                .WithHeaders(head)
                .FormatHeaders(align)
                .AddRow(row1)
                .AddRow(row2)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString(4);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithMax_Formats_Right_Correctly()
        {
            var align = HorizontalAlignment.Right;
            var width = 0;
            var expected = GetExpectedTable(align, width);
            var t = Table.Create()
                .WithHeaders(head)
                .FormatHeaders(align)
                .AddRow(row1)
                .AddRow(row2)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString(4);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithBrokenText_Formats_Left_Correctly()
        {
            var align = HorizontalAlignment.Left;
            var width = 0;
            var expected = GetExpectedBrokenTable(align, 17);
            var t = Table.Create()
                .WithHeaders(brokenRow)
                .FormatHeaders(align)
                .AddRow(brokenRow)
                .AddRow(brokenRow)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString(17);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithBrokenText_Formats_Center_Correctly()
        {
            var align = HorizontalAlignment.Center;
            var width = 0;
            var expected = GetExpectedBrokenTable(align, 17);
            var t = Table.Create()
                .WithHeaders(brokenRow)
                .FormatHeaders(align)
                .AddRow(brokenRow)
                .AddRow(brokenRow)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString(17);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithBrokenText_Formats_Right_Correctly()
        {
            var align = HorizontalAlignment.Right;
            var width = 0;
            var expected = GetExpectedBrokenTable(align, 17);
            var t = Table.Create()
                .WithHeaders(brokenRow)
                .FormatHeaders(align)
                .AddRow(brokenRow)
                .AddRow(brokenRow)
                .FormatRow(0, align)
                .FormatRow(1, align);

            t.MinimumColumnWidth = width;

            var actual = t.ToString(17);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNullCellContent_Prints_Correctly()
        {
            var t = Table.Create().AddRow((string)null, (string)null);
            var expected = @"-------
|  |  |
-------
";
            var actual = t.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithEmptyCellContent_Prints_Correctly()
        {
            var t = Table.Create().AddRow(string.Empty, string.Empty);
            var expected = @"-------
|  |  |
-------
";
            var actual = t.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNullCellContents_AndRoundedChrome_Prints_Correctly()
        {
            var t = Table.Create().AddRow((string)null, (string)null);
            var c = new RoundedChrome();
            var sb = new StringBuilder();
            sb.Append(c.BodyTopLeft);
            sb.Append(c.BodyTop);
            sb.Append(c.BodyTop);
            sb.Append(c.BodyTopJoin);
            sb.Append(c.BodyTop);
            sb.Append(c.BodyTop);
            sb.Append(c.BodyTopRight);
            sb.Append(ConsoleBase.NewLine);
            sb.Append(c.BodyLeft);
            sb.Append(' ', 2);
            sb.Append(c.BodyInteriorVertical);
            sb.Append(' ', 2);
            sb.Append(c.BodyRight);
            sb.Append(ConsoleBase.NewLine);
            sb.Append(c.BodyBottomLeft);
            sb.Append(c.BodyBottom);
            sb.Append(c.BodyBottom);
            sb.Append(c.BodyBottomJoin);
            sb.Append(c.BodyBottom);
            sb.Append(c.BodyBottom);
            sb.Append(c.BodyBottomRight);
            sb.Append("\r\n");

            var expected = sb.ToString();

            var actual = t.ToString(c);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithMultiRowCell_WithNonStandardCharacterEndings_PrintsCorrect()
        {
            var expected =
@"-------------
| Test (1)  |
| Test3 (3) |
|-----------|
| Test2 (2) |
-------------
";

            var t = new Table().AddColumn("Test (1)\r\nTest3 (3)", "Test2 (2)");

            Assert.AreEqual(expected, t.ToString());
        }
    }
}
