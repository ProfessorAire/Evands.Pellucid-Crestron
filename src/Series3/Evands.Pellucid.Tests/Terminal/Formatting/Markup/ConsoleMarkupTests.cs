using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Markup
{
    [TestClass]
    public class ConsoleMarkupTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Options.Instance.EnableMarkup = true;
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        public ConsoleMarkupTests()
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
        public void GetColor_With_InvalidColor_Gets_EmptyString()
        {
            Assert.IsTrue(string.IsNullOrEmpty(ConsoleMarkup.GetColor("lxox", false)));
        }

        #region GeneralFormat

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_All_Closure_Inserts_ClearFormat()
        {
            var expected = "Test\x1b[0m";
            var actual = "Test[:/all]".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Bold_Inserts_Expected()
        {
            var expected = "\x1b[1mTest";
            var actual = "[:b]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Italic_Inserts_Expected()
        {
            var expected = "\x1b[3mTest";
            var actual = "[:i]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Underline_Inserts_Expected()
        {
            var expected = "\x1b[4mTest";
            var actual = "[:u]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Blinking_Inserts_Expected()
        {
            var expected = "\x1b[5mTest";
            var actual = "[:sb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Strikethrough_Inserts_Expected()
        {
            var expected = "\x1b[9mTest";
            var actual = "[:st]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Reverse_Inserts_Expected()
        {
            var expected = "\x1b[7mTest";
            var actual = "[:rv]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Frame_Inserts_Expected()
        {
            var expected = "\x1b[51mTest";
            var actual = "[:fr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Encircle_Inserts_Expected()
        {
            var expected = "\x1b[52mTest";
            var actual = "[:en]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Overline_Inserts_Expected()
        {
            var expected = "\x1b[53mTest";
            var actual = "[:ov]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Bold_Closure_Inserts_Expected()
        {
            var expected = "\x1b[22mTest";
            var actual = "[:/b]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Italic_Closure_Inserts_Expected()
        {
            var expected = "\x1b[23mTest";
            var actual = "[:/i]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Underline_Closure_Inserts_Expected()
        {
            var expected = "\x1b[24mTest";
            var actual = "[:/u]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Blinking_Closure_Inserts_Expected()
        {
            var expected = "\x1b[25mTest";
            var actual = "[:/sb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Strikethrough_Closure_Inserts_Expected()
        {
            var expected = "\x1b[29mTest";
            var actual = "[:/st]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Reverse_Closure_Inserts_Expected()
        {
            var expected = "\x1b[27mTest";
            var actual = "[:/rv]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Frame_Closure_Inserts_Expected()
        {
            var expected = "\x1b[54mTest";
            var actual = "[:/fr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Encircle_Closure_Inserts_Expected()
        {
            var expected = "\x1b[54mTest";
            var actual = "[:/en]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Overline_Closure_Inserts_Expected()
        {
            var expected = "\x1b[55mTest";
            var actual = "[:/ov]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_All_Formats_Inserts_Expected()
        {
            var expected = "\x1b[1;3;4;5;7;9;51;52;53mTest\x1b[0m";
            var actual = "[:biusbstrvfrenov]Test[:/all]".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Mixed_Formats_And_Color_Inserts_Expected()
        {
            var expected = "\x1b[1;3;7;38;2;128;128;128;101mTest\x1b[22;39m";
            var actual = "[:rv i b cf:128,128,128 cb:br]Test[:/b /cf]".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_HideCursor_Inserts_Expected()
        {
            var expected = "\x1b[?25lTest";
            var actual = "[:hc]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_HideCursor_Closure_Inserts_Expected()
        {
            var expected = "\x1b[?25hTest";
            var actual = "[:/hc]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_HideCursor_AndOther_Inserts_Only_HideCursor()
        {
            var expected = "\x1b[?25lTest";
            var actual = "[:hcibcf:lr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Rgb

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_Rgb_Inserts_AnsiColor()
        {
            var expected = "\x1b[38;2;255;255;255mTest";
            var actual = "[:cf:255,255,255]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_Rgb_Inserts_AnsiColor()
        {
            var expected = "\x1b[48;2;255;255;255mTest";
            var actual = "[:cb:255,255,255]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ForegroundStandard

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[30mTest";
            var actual = "[:cf:k]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[30mTest";
            var actual = "[:cf:black]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[31mTest";
            var actual = "[:cf:r]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[31mTest";
            var actual = "[:cf:red]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[32mTest";
            var actual = "[:cf:g]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[32mTest";
            var actual = "[:cf:green]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[33mTest";
            var actual = "[:cf:y]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[33mTest";
            var actual = "[:cf:yellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[34mTest";
            var actual = "[:cf:b]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[34mTest";
            var actual = "[:cf:blue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[35mTest";
            var actual = "[:cf:m]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[35mTest";
            var actual = "[:cf:magenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[36mTest";
            var actual = "[:cf:cyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[36mTest";
            var actual = "[:cf:cyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[37mTest";
            var actual = "[:cf:w]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[37mTest";
            var actual = "[:cf:white]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromColorMarkup_With_Foreground_Closure_Inserts_Expected()
        {
            var expected = "\x1b[91mTest\x1b[39m";
            var actual = "[:cf:lr]Test[:/cf]".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ForegroundBrightLetter

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:bk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:bblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:br]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:bred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:bg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:bgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:by]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:byellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:bb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:bblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:bm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:bmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:bcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:bcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:bw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:bwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ForegroundBrightName

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:brightk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:brightblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:brightr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:brightred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:brightg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:brightgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:brighty]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:brightyellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:brightb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:brightblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:brightm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:brightmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:brightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:brightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:brightw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:brightwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ForegroundLightLetter

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:ly]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:lyellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ForegroundLightName

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lightk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lightblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lightr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lightred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lightg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lightgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:lighty]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:lightyellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lightb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lightblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lightm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lightmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lightw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lightwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BackgroundStandard

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[40mTest";
            var actual = "[:cb:k]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[40mTest";
            var actual = "[:cb:black]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[41mTest";
            var actual = "[:cb:r]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[41mTest";
            var actual = "[:cb:red]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[42mTest";
            var actual = "[:cb:g]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[42mTest";
            var actual = "[:cb:green]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[43mTest";
            var actual = "[:cb:y]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[43mTest";
            var actual = "[:cb:yellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[44mTest";
            var actual = "[:cb:b]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[44mTest";
            var actual = "[:cb:blue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[45mTest";
            var actual = "[:cb:m]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[45mTest";
            var actual = "[:cb:magenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[46mTest";
            var actual = "[:cb:cyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[46mTest";
            var actual = "[:cb:cyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[47mTest";
            var actual = "[:cb:w]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[47mTest";
            var actual = "[:cb:white]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromColorMarkup_With_Background_Closure_Inserts_Expected()
        {
            var expected = "\x1b[101mTest\x1b[49m";
            var actual = "[:cb:lr]Test[:/cb]".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BackgroundBrightLetter

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:bk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:bblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:br]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:bred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:bg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:bgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:by]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:byellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:bb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:bblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:bm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:bmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:bcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:bcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:bw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:bwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BackgroundBrightName

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:brightk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:brightblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:brightr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:brightred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:brightg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:brightgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:brighty]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:brightyellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:brightb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:brightblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:brightm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:brightmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:brightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:brightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:brightw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:brightwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BackgroundLightLetter

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:ly]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:lyellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region BackgroundLightName

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lightk]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lightblack]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lightr]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lightred]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lightg]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lightgreen]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:lighty]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:lightyellow]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lightb]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lightblue]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lightm]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lightmagenta]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lightcyan]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lightw]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lightwhite]Test".ToAnsiFromConsoleMarkup();
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
