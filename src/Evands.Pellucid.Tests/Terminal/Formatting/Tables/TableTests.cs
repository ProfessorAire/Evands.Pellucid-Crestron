using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    public class TableTests
    {
        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
        }

        public TableTests()
        {
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

        [Test]
        public async Task Table_Create_ReturnsNewTable()
        {
            var t = Table.Create();
            await Assert.That(t).IsNotNull();
        }

        [Test]
        public async Task Table_Ctors_CreateMinimumColumnWidthOfZero()
        {
            var t = new Table();
            await Assert.That(t.MinimumColumnWidth == 0).IsTrue();
        }

        [Test]
        public async Task Table_NumberOfRows_IsAccurate()
        {
            var t = Table.Create();
            await Assert.That(t.NumberOfRows == 0).IsTrue();
            t.AddRow(new string[] { "Value 1", "Value 2" });
            await Assert.That(t.NumberOfRows == 1).IsTrue();
            t.AddRow(new string[] { "Value 3", "Value 4" });
            await Assert.That(t.NumberOfRows == 2).IsTrue();
            t.AddRow(new Cell[] { new Cell("Value 5"), new Cell("Value 6") });
            await Assert.That(t.NumberOfRows == 3).IsTrue();
        }

        [Test]
        public async Task Table_NumberOfColumns_IsAccurate_WhenAddingColumns()
        {
            var t = Table.Create();
            await Assert.That(t.NumberOfColumns == 0).IsTrue();
            for (var i = 1; i < 5; i++)
            {
                t.AddColumn("Cell", "Cell");
                await Assert.That(t.NumberOfColumns == i).IsTrue();
            }
        }

        [Test]
        public async Task Table_MinimumColumWidth_IsAccurate()
        {
            var t = Table.Create();
            await Assert.That(t.MinimumColumnWidth == 0).IsTrue();
            t.MinimumColumnWidth = 10;
            await Assert.That(t.MinimumColumnWidth == 10).IsTrue();
        }

        [Test]
        public async Task Table_SetMinimumColumWidth_SetsMinimumColumnWidth()
        {
            var t = Table.Create();
            await Assert.That(t.MinimumColumnWidth == 0).IsTrue();
            t.SetMinimumColumnWidth(4);
            await Assert.That(t.MinimumColumnWidth == 4).IsTrue();
        }

        [Test]
        public async Task Table_HorizontalBorder_GetsSets()
        {
            var t = Table.Create();
            t.HorizontalBorder = 'a';
            await Assert.That(t.HorizontalBorder == 'a').IsTrue();
        }

        [Test]
        public async Task Table_VerticalBorder_GetsSets()
        {
            var t = Table.Create();
            t.VerticalBorder = 'b';
            await Assert.That(t.VerticalBorder == 'b').IsTrue();
        }

        [Test]
        public async Task Table_HeaderBottomBorder_GetsSets()
        {
            var t = Table.Create();
            t.HeaderBottomBorder = 'c';
            await Assert.That(t.HeaderBottomBorder == 'c').IsTrue();
        }

        [Test]
        public async Task Table_WithHeaders_StringArray_AddsHeaders()
        {
            var expectedValues = new string[] { "Header1", "Header2", "Header3" };

            var t = Table.Create()
                .WithHeaders(expectedValues);

            for (var i = 0; i < t.NumberOfColumns; i++)
            {
                await Assert.That(t.Headers[i].Contents == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_WithHeaders_CellArray_AddsHeaders()
        {
            var expectedValues = new Cell[]
            {
                new Cell("Header1"),
                new Cell("Header2")
            };

            var t = Table.Create().WithHeaders(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Headers[i] == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_AddHeader_String_AddsHeader()
        {
            var expectedValue = "New Header";

            var t = Table
                .Create()
                .WithHeaders("Header1", "Header2")
                .AddHeader(expectedValue);

            await Assert.That(t.Headers[2].Contents == expectedValue).IsTrue();
        }

        [Test]
        public async Task Table_AddHeader_Cell_AddsHeader()
        {
            var expectedValue = new Cell("New Header");
            var t = Table
                .Create()
                .WithHeaders("Header1", "Header2")
                .AddHeader(expectedValue);

            await Assert.That(t.Headers[2] == expectedValue).IsTrue();
        }

        [Test]
        public async Task Table_AddRow_Strings_AddsRow()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value3" };
            var t = Table
                .Create()
                .AddRow(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Rows[0][i].Contents == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_AddRow_Cells_AddsRow()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2") };
            var t = Table
                .Create()
                .AddRow(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Rows[0][i] == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_AddColumn_Strings_AddsColumn_WithEmptyHeader()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value3", "Value4" };
            var t = Table
                .Create()
                .AddColumn(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Columns[0][i].Contents == expectedValues[i]).IsTrue();
            }

            await Assert.That(t.Headers[0].Contents == string.Empty).IsTrue();
        }

        [Test]
        public async Task Table_AddColumn_Cells_AddsColumn_WithEmptyHeader()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2"), new Cell("Value") };
            var t = Table
                .Create()
                .AddColumn(expectedValues);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Columns[0][i] == expectedValues[i]).IsTrue();
            }

            await Assert.That(string.IsNullOrEmpty(t.Headers[0].Contents)).IsTrue();
        }

        [Test]
        public async Task Table_AddColumnWithHeader_StringCells_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2"), new Cell("Value") };
            var expectedHeader = "Header Value";

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            await Assert.That(t.Headers[0].Contents == expectedHeader).IsTrue();

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Columns[0][i] == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_AddColumnWithHeader_CellStrings_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value" };
            var expectedHeader = new Cell("Header Value");

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            await Assert.That(t.Headers[0] == expectedHeader).IsTrue();

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Columns[0][i].Contents == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_AddColumnWithHeader_StringStrings_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new string[] { "Value1", "Value2", "Value" };
            var expectedHeader = "Header Value";

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            await Assert.That(t.Headers[0].Contents == expectedHeader).IsTrue();

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Columns[0][i].Contents == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_AddColumnWithHeader_CellCells_AddsColumn_WithExpectedHeader()
        {
            var expectedValues = new Cell[] { new Cell("Value1"), new Cell("Value2"), new Cell("Value") };
            var expectedHeader = new Cell("Header Value");

            var t = Table
                .Create()
                .AddColumnWithHeader(expectedHeader, expectedValues);

            await Assert.That(t.Headers[0] == expectedHeader).IsTrue();

            for (var i = 0; i < expectedValues.Length; i++)
            {
                await Assert.That(t.Columns[0][i] == expectedValues[i]).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatHeaders_AlignmentColor()
        {
            var expectedAlign = HorizontalAlignment.Center;
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .WithHeaders("H1", "H2", "H3", "H4")
                .FormatHeaders(expectedColor, expectedAlign);

            for (var i = 0; i < t.Headers.Count; i++)
            {
                await Assert.That(t.Headers[i].HorizontalAlignment == expectedAlign).IsTrue();
                await Assert.That(t.Headers[i].Color == expectedColor).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatHeaders_Alignment()
        {
            var expectedAlign = HorizontalAlignment.Center;

            var t = Table
                .Create()
                .WithHeaders("H1", "H2", "H3", "H4")
                .FormatHeaders(expectedAlign);

            for (var i = 0; i < t.Headers.Count; i++)
            {
                await Assert.That(t.Headers[i].HorizontalAlignment == expectedAlign).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatHeaders_Color()
        {
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .WithHeaders("H1", "H2", "H3", "H4")
                .FormatHeaders(expectedColor);

            for (var i = 0; i < t.Headers.Count; i++)
            {
                await Assert.That(t.Headers[i].Color == expectedColor).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatColumn_AlignmentColor()
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
                await Assert.That(t.Columns[1][i].HorizontalAlignment == expectedAlign).IsTrue();
                await Assert.That(t.Columns[1][i].Color == expectedColor).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatColumn_Alignment()
        {
            var expectedAlign = HorizontalAlignment.Center;

            var t = Table
                .Create()
                .AddColumn("H1", "H2", "H3", "H4")
                .AddColumn("H5", "H6", "H7", "H8")
                .FormatColumn(1, expectedAlign);

            for (var i = 0; i < t.Columns.Count; i++)
            {
                await Assert.That(t.Columns[1][i].HorizontalAlignment == expectedAlign).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatColumn_Color()
        {
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .AddColumn("H1", "H2", "H3", "H4")
                .AddColumn("H5", "H6", "H7", "H8")
                .FormatColumn(1, expectedColor);

            for (var i = 0; i < t.Columns.Count; i++)
            {
                await Assert.That(t.Columns[1][i].Color == expectedColor).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatRow_AlignmentColor()
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
                await Assert.That(t.Rows[1][i].HorizontalAlignment == expectedAlign).IsTrue();
                await Assert.That(t.Rows[1][i].Color == expectedColor).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatRow_Alignment()
        {
            var expectedAlign = HorizontalAlignment.Center;

            var t = Table
                .Create()
                .AddRow("H1", "H2", "H3", "H4")
                .AddRow("H5", "H6", "H7", "H8")
                .FormatRow(1, expectedAlign);

            for (var i = 0; i < t.Rows.Count; i++)
            {
                await Assert.That(t.Rows[1][i].HorizontalAlignment == expectedAlign).IsTrue();
            }
        }

        [Test]
        public async Task Table_FormatRow_Color()
        {
            var expectedColor = ConsoleBase.Colors.Warning;

            var t = Table
                .Create()
                .AddRow("H1", "H2", "H3", "H4")
                .AddRow("H5", "H6", "H7", "H8")
                .FormatRow(1, expectedColor);

            for (var i = 0; i < t.Rows.Count; i++)
            {
                await Assert.That(t.Rows[1][i].Color == expectedColor).IsTrue();
            }
        }

        [Test]
        public async Task ToString_Formats_Left_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_Formats_Center_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_Formats_Right_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithMax_Formats_Left_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithMax_Formats_Center_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithMax_Formats_Right_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithBrokenText_Formats_Left_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithBrokenText_Formats_Center_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithBrokenText_Formats_Right_Correctly()
        {
            Options.Instance.ColorizeConsoleOutput = false;
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
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithNullCellContent_Prints_Correctly()
        {
            var t = Table.Create().AddRow((string)null, (string)null);
            var expected = @"-------
|  |  |
-------
".Replace("\r\n", "\n").Replace("\n", "\r\n");
            var actual = t.ToString();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithEmptyCellContent_Prints_Correctly()
        {
            var t = Table.Create().AddRow(string.Empty, string.Empty);
            var expected = @"-------
|  |  |
-------
".Replace("\r\n", "\n").Replace("\n", "\r\n");
            var actual = t.ToString();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithNullCellContents_AndRoundedChrome_Prints_Correctly()
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

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithMultiRowCell_WithNonStandardCharacterEndings_PrintsCorrect()
        {
            var expected =
@"-------------
| Test (1)  |
| Test3 (3) |
|-----------|
| Test2 (2) |
-------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var t = new Table().AddColumn("Test (1)\r\nTest3 (3)", "Test2 (2)");

            await Assert.That(t.ToString()).IsEqualTo(expected);
        }

        [Test]
        [Arguments(null)]
        [Arguments("")]
        [Arguments("x")]
        [Arguments("abc")]
        public async Task ToString_WithHeadersAndCellContentShorterThanHeader_AutoSizesColumnToHeaderWidth(string cellContent)
        {
            var t = Table.Create()
                .WithHeaders("room-01", "room-02", "room-03")
                .AddRow(cellContent, cellContent, cellContent);

            var paddedContent = (cellContent ?? string.Empty).PadRight(7);

            var sb = new StringBuilder();
            sb.Append('-', 31);
            sb.Append(ConsoleBase.NewLine);
            sb.Append("| room-01 | room-02 | room-03 |");
            sb.Append(ConsoleBase.NewLine);
            sb.Append('-', 31);
            sb.Append(ConsoleBase.NewLine);
            sb.AppendFormat("| {0} | {1} | {2} |", paddedContent, paddedContent, paddedContent);
            sb.Append(ConsoleBase.NewLine);
            sb.Append('-', 31);
            sb.Append(ConsoleBase.NewLine);

            var expected = sb.ToString();
            var actual = t.ToString(false);

            await Assert.That(actual).IsEqualTo(expected);
        }
    }
}
