using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for CellTests
    /// </summary>
    public class CellTests
    {
        public CellTests()
        {
        }

        [Before(Test)]
        public void TestSetup()
        {
            UnderTest = new Cell();
        }

        public Cell UnderTest { get; set; }

        [Test]
        public async Task DefaultCtor_Creates_Empty_Cell()
        {
            await Assert.That(UnderTest).IsNotNull();
            await Assert.That(UnderTest.Contents == string.Empty).IsTrue();
            await Assert.That(UnderTest.Color == ColorFormat.None).IsTrue();
        }

        [Test]
        public async Task Contents_Ctor_Creates_Cell_WithContents_NoColor()
        {
            var expected = "Test Value";
            UnderTest = new Cell(expected);
            await Assert.That(UnderTest).IsNotNull();
            await Assert.That(UnderTest.Contents == expected).IsTrue();
            await Assert.That(UnderTest.Color == ColorFormat.None).IsTrue();
        }

        [Test]
        public async Task ContentsAndColor_Ctor_Creates_Cell_WithContentsAndColor()
        {
            var expectedContents = "Test Value";
            var expectedColorFormat = ConsoleBase.Colors.BrightGreen;
            UnderTest = new Cell(expectedContents, expectedColorFormat);
            await Assert.That(UnderTest).IsNotNull();
            await Assert.That(UnderTest.Contents == expectedContents).IsTrue();
            await Assert.That(UnderTest.Color == expectedColorFormat).IsTrue();
        }

        [Test]
        public async Task ColorFormat_GetsSets_CorrectValue()
        {
            var expectedColorFormat = ConsoleBase.Colors.BrightGreen;
            UnderTest.Color = expectedColorFormat;
            await Assert.That(UnderTest.Color == expectedColorFormat).IsTrue();
        }

        [Test]
        public async Task HorizontalAlignment_GetsSets_CorrectValue()
        {
            var expected = HorizontalAlignment.Center;
            UnderTest.HorizontalAlignment = expected;
            await Assert.That(UnderTest.HorizontalAlignment == expected).IsTrue();
            expected = HorizontalAlignment.Left;
            UnderTest.HorizontalAlignment = expected;
            await Assert.That(UnderTest.HorizontalAlignment == expected).IsTrue();
            expected = HorizontalAlignment.Right;
            UnderTest.HorizontalAlignment = expected;
            await Assert.That(UnderTest.HorizontalAlignment == expected).IsTrue();
        }

        [Test]
        public async Task Contents_GetsSets_CorrectValue()
        {
            var expected = "This is a test.";
            UnderTest.Contents = expected;
            await Assert.That(UnderTest.Contents == expected).IsTrue();
        }

        [Test]
        public async Task GetTotalWidth_Returns_AccurateValue_WhenTextHasColorFormatting()
        {
            var text = "This is just some text.";
            var content = ConsoleBase.Colors.BrightRed.FormatText(text);
            UnderTest.Contents = content;
            await Assert.That(UnderTest.GetTotalWidth() == text.Length).IsTrue();
        }

        [Test]
        public async Task GetTotalWidth_Returns_AccurateValue_WhenTextHasNoColorFormatting()
        {
            var expected = "This is just a text test.";
            UnderTest.Contents = expected;
            await Assert.That(UnderTest.GetTotalWidth() == expected.Length).IsTrue();
        }

        [Test]
        public async Task GetTotalWidth_Returns_AccurateValue_WhenLineBreaksIncluded()
        {
            var lineToTest = "This is line 1.\r\nThis is line two.\r\nThis is line three.";
            UnderTest.Contents = lineToTest;
            var expected = lineToTest.Replace("\r\n", "\n").Split('\n').Max(l => l.Length);

            await Assert.That(UnderTest.GetTotalWidth()).IsEqualTo(expected);
        }

        [Test]
        public async Task GetNumberOfLines_IsAccurate()
        {
            var text = "Line1\nLine2\nLine3";
            UnderTest.Contents = text;
            await Assert.That(UnderTest.GetNumberOfLines(100)).IsEqualTo(3);
            text = "Line1Line2Line3Line4";
            UnderTest.Contents = text;
            await Assert.That(UnderTest.GetNumberOfLines(5)).IsEqualTo(4);
            await Assert.That(UnderTest.GetNumberOfLines(10)).IsEqualTo(2);
            text = "Some text.";
            UnderTest.Contents = text;
            await Assert.That(UnderTest.GetNumberOfLines(100)).IsEqualTo(1);
        }

        [Test]
        public async Task GetNumberOfLines_IsAccurate_WhenColorIncluded()
        {
            var text = ConsoleBase.Colors.Blue.FormatText("Line1\nLine2\nLine3");
            UnderTest.Contents = text;
            await Assert.That(UnderTest.GetNumberOfLines(100)).IsEqualTo(3);
            text = ConsoleBase.Colors.Blue.FormatText("Line1Line2Line3Line4");
            UnderTest.Contents = text;
            await Assert.That(UnderTest.GetNumberOfLines(5)).IsEqualTo(4);
            await Assert.That(UnderTest.GetNumberOfLines(10)).IsEqualTo(2);
            text = ConsoleBase.Colors.Blue.FormatText("Some text.");
            UnderTest.Contents = text;
            await Assert.That(UnderTest.GetNumberOfLines(100)).IsEqualTo(1);
        }

        [Test]
        public async Task GetLine_Returns_CorrectValue_WithoutColor()
        {
            var text = "Line1Line2Line3Line4";
            var items = new string[] { "Line1", "Line2", "Line3", "Line4" };

            UnderTest.Contents = text;

            for (var i = 0; i < items.Length; i++)
            {
                await Assert.That(UnderTest.GetLine(i, 5, false) == items[i]).IsTrue();
            }
        }

        [Test]
        public async Task GetLine_Returns_CorrectValue_WithColor()
        {
            var text = "Line1\nLine2\nLine3\nLine4";
            UnderTest.Contents = text;
            UnderTest.Color = ConsoleBase.Colors.BrightRed;

            var items = text.Split('\n');

            for (var i = 0; i < items.Length; i++)
            {
                var line = UnderTest.GetLine(i, 5, true);
                var expected = UnderTest.Color.FormatText(items[i]);
                await Assert.That(line == expected).IsTrue();
            }            
        }

        [Test]
        public async Task GetLine_WithTerminatingParenthesis_Returns_CorrectValue_WithoutColor()
        {
            var text = "Line1 (1)\r\nLine2 (2)\r\nLine3 (1)\r\nLine 5 (123)";
            UnderTest.Contents = text;

            var items = text.Replace("\r", string.Empty).Split('\n');

            for (var i = 0; i < items.Length; i++)
            {
                await Assert.That(UnderTest.GetLine(i, 12, false).TrimEnd()).IsEqualTo(items[i]);
            }
        }

        [Test]
        public async Task GetLine_WithTerminatingBracket_Returns_CorrectValue_WithoutColor()
        {
            var text = "Line1 [1]\nLine2 [2]\nLine3 [1]\nLine5 [4]";
            UnderTest.Contents = text;

            var items = text.Split('\n');

            for (var i = 0; i < items.Length; i++)
            {
                await Assert.That(UnderTest.GetLine(i, 9, false)).IsEqualTo(items[i]);
            }
        }

        [Test]
        public async Task GetLine_Returns_LeftAlignedValue()
        {
            var text = "This is some long text.";
            var width = text.Length + 10;
            UnderTest.Contents = text;
            UnderTest.HorizontalAlignment = HorizontalAlignment.Left;

            await Assert.That(UnderTest.GetLine(0, width, false) == text.PadRight(width)).IsTrue();
        }

        [Test]
        public async Task GetLine_Returns_CenterAlignedValue()
        {
            var text = "This is some long text.";
            var width = text.Length + 10;
            UnderTest.Contents = text;
            UnderTest.HorizontalAlignment = HorizontalAlignment.Center;

            await Assert.That(UnderTest.GetLine(0, width, false) == text.Pad(width)).IsTrue();
        }

        [Test]
        public async Task GetLine_Returns_RightAlignedValue()
        {
            var text = "This is some long text.";
            var width = text.Length + 10;
            UnderTest.Contents = text;
            UnderTest.HorizontalAlignment = HorizontalAlignment.Right;

            await Assert.That(UnderTest.GetLine(0, width, false) == text.PadLeft(width)).IsTrue();
        }

        [Test]
        public async Task SetNullValue_SetsCellContents_ToEmptyString()
        {
            UnderTest.Contents = null;
            await Assert.That(UnderTest.Contents == string.Empty).IsTrue();
        }
    }
}
