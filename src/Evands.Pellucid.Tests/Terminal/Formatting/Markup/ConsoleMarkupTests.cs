using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Formatting.Markup
{
    public class ConsoleMarkupTests
    {
        [Before(Test)]
        public void Initialize()
        {
            Options.Instance.EnableMarkup = true;
        }
        public ConsoleMarkupTests()
        {
        }


        [Test]
        public async Task GetColor_With_InvalidColor_Gets_EmptyString()
        {
            await Assert.That(string.IsNullOrEmpty(ConsoleMarkup.GetColor("lxox", false))).IsTrue();
        }

        #region GeneralFormat

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_All_Closure_Inserts_ClearFormat()
        {
            var expected = "Test\x1b[0m";
            var actual = "Test[:/all]".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Bold_Inserts_Expected()
        {
            var expected = "\x1b[1mTest";
            var actual = "[:b]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Italic_Inserts_Expected()
        {
            var expected = "\x1b[3mTest";
            var actual = "[:i]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Underline_Inserts_Expected()
        {
            var expected = "\x1b[4mTest";
            var actual = "[:u]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Blinking_Inserts_Expected()
        {
            var expected = "\x1b[5mTest";
            var actual = "[:sb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Strikethrough_Inserts_Expected()
        {
            var expected = "\x1b[9mTest";
            var actual = "[:st]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Reverse_Inserts_Expected()
        {
            var expected = "\x1b[7mTest";
            var actual = "[:rv]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Frame_Inserts_Expected()
        {
            var expected = "\x1b[51mTest";
            var actual = "[:fr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Encircle_Inserts_Expected()
        {
            var expected = "\x1b[52mTest";
            var actual = "[:en]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Overline_Inserts_Expected()
        {
            var expected = "\x1b[53mTest";
            var actual = "[:ov]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Bold_Closure_Inserts_Expected()
        {
            var expected = "\x1b[22mTest";
            var actual = "[:/b]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Italic_Closure_Inserts_Expected()
        {
            var expected = "\x1b[23mTest";
            var actual = "[:/i]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Underline_Closure_Inserts_Expected()
        {
            var expected = "\x1b[24mTest";
            var actual = "[:/u]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Blinking_Closure_Inserts_Expected()
        {
            var expected = "\x1b[25mTest";
            var actual = "[:/sb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Strikethrough_Closure_Inserts_Expected()
        {
            var expected = "\x1b[29mTest";
            var actual = "[:/st]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Reverse_Closure_Inserts_Expected()
        {
            var expected = "\x1b[27mTest";
            var actual = "[:/rv]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Frame_Closure_Inserts_Expected()
        {
            var expected = "\x1b[54mTest";
            var actual = "[:/fr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Encircle_Closure_Inserts_Expected()
        {
            var expected = "\x1b[54mTest";
            var actual = "[:/en]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Overline_Closure_Inserts_Expected()
        {
            var expected = "\x1b[55mTest";
            var actual = "[:/ov]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_All_Formats_Inserts_Expected()
        {
            var expected = "\x1b[1;3;4;5;7;9;51;52;53mTest\x1b[0m";
            var actual = "[:biusbstrvfrenov]Test[:/all]".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Mixed_Formats_And_Color_Inserts_Expected()
        {
            var expected = "\x1b[1;3;7;38;2;128;128;128;101mTest\x1b[22;39m";
            var actual = "[:rv i b cf:128,128,128 cb:br]Test[:/b /cf]".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_HideCursor_Inserts_Expected()
        {
            var expected = "\x1b[?25lTest";
            var actual = "[:hc]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_HideCursor_Closure_Inserts_Expected()
        {
            var expected = "\x1b[?25hTest";
            var actual = "[:/hc]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_HideCursor_AndOther_Inserts_Only_HideCursor()
        {
            var expected = "\x1b[?25lTest";
            var actual = "[:hcibcf:lr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region Rgb

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_Rgb_Inserts_AnsiColor()
        {
            var expected = "\x1b[38;2;255;255;255mTest";
            var actual = "[:cf:255,255,255]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_Rgb_Inserts_AnsiColor()
        {
            var expected = "\x1b[48;2;255;255;255mTest";
            var actual = "[:cb:255,255,255]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region ForegroundStandard

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[30mTest";
            var actual = "[:cf:k]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[30mTest";
            var actual = "[:cf:black]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[31mTest";
            var actual = "[:cf:r]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[31mTest";
            var actual = "[:cf:red]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[32mTest";
            var actual = "[:cf:g]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[32mTest";
            var actual = "[:cf:green]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[33mTest";
            var actual = "[:cf:y]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[33mTest";
            var actual = "[:cf:yellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[34mTest";
            var actual = "[:cf:b]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[34mTest";
            var actual = "[:cf:blue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[35mTest";
            var actual = "[:cf:m]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[35mTest";
            var actual = "[:cf:magenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[36mTest";
            var actual = "[:cf:cyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[36mTest";
            var actual = "[:cf:cyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[37mTest";
            var actual = "[:cf:w]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[37mTest";
            var actual = "[:cf:white]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromColorMarkup_With_Foreground_Closure_Inserts_Expected()
        {
            var expected = "\x1b[91mTest\x1b[39m";
            var actual = "[:cf:lr]Test[:/cf]".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region ForegroundBrightLetter

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:bk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:bblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:br]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:bred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:bg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:bgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:by]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:byellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:bb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:bblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:bm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:bmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:bcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:bcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:bw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:bwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region ForegroundBrightName

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:brightk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:brightblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:brightr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:brightred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:brightg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:brightgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:brighty]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:brightyellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:brightb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:brightblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:brightm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:brightmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:brightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:brightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:brightw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:brightwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region ForegroundLightLetter

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:ly]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:lyellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region ForegroundLightName

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lightk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[90mTest";
            var actual = "[:cf:lightblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lightr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[91mTest";
            var actual = "[:cf:lightred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lightg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[92mTest";
            var actual = "[:cf:lightgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:lighty]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[93mTest";
            var actual = "[:cf:lightyellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lightb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[94mTest";
            var actual = "[:cf:lightblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lightm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[95mTest";
            var actual = "[:cf:lightmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[96mTest";
            var actual = "[:cf:lightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lightw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Foreground_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[97mTest";
            var actual = "[:cf:lightwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region BackgroundStandard

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[40mTest";
            var actual = "[:cb:k]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[40mTest";
            var actual = "[:cb:black]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[41mTest";
            var actual = "[:cb:r]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[41mTest";
            var actual = "[:cb:red]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[42mTest";
            var actual = "[:cb:g]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[42mTest";
            var actual = "[:cb:green]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[43mTest";
            var actual = "[:cb:y]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[43mTest";
            var actual = "[:cb:yellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[44mTest";
            var actual = "[:cb:b]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[44mTest";
            var actual = "[:cb:blue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[45mTest";
            var actual = "[:cb:m]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[45mTest";
            var actual = "[:cb:magenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[46mTest";
            var actual = "[:cb:cyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[46mTest";
            var actual = "[:cb:cyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[47mTest";
            var actual = "[:cb:w]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[47mTest";
            var actual = "[:cb:white]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromColorMarkup_With_Background_Closure_Inserts_Expected()
        {
            var expected = "\x1b[101mTest\x1b[49m";
            var actual = "[:cb:lr]Test[:/cb]".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region BackgroundBrightLetter

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:bk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:bblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:br]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:bred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:bg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:bgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:by]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:byellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:bb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:bblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:bm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:bmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:bcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:bcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:bw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightLetter_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:bwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region BackgroundBrightName

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:brightk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:brightblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:brightr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:brightred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:brightg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:brightgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:brighty]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:brightyellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:brightb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:brightblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:brightm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:brightmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:brightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:brightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:brightw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_BrightName_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:brightwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region BackgroundLightLetter

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:ly]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:lyellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightLetter_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion

        #region BackgroundLightName

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lightk]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Black_Inserts_AnsiColor()
        {
            var expected = "\x1b[100mTest";
            var actual = "[:cb:lightblack]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lightr]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Red_Inserts_AnsiColor()
        {
            var expected = "\x1b[101mTest";
            var actual = "[:cb:lightred]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lightg]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Green_Inserts_AnsiColor()
        {
            var expected = "\x1b[102mTest";
            var actual = "[:cb:lightgreen]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:lighty]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Yellow_Inserts_AnsiColor()
        {
            var expected = "\x1b[103mTest";
            var actual = "[:cb:lightyellow]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lightb]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Blue_Inserts_AnsiColor()
        {
            var expected = "\x1b[104mTest";
            var actual = "[:cb:lightblue]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lightm]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Magenta_Inserts_AnsiColor()
        {
            var expected = "\x1b[105mTest";
            var actual = "[:cb:lightmagenta]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_Cyan_Inserts_AnsiColor()
        {
            var expected = "\x1b[106mTest";
            var actual = "[:cb:lightcyan]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorLetter_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lightw]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToAnsiFromConsoleMarkup_With_LightName_Background_ColorName_White_Inserts_AnsiColor()
        {
            var expected = "\x1b[107mTest";
            var actual = "[:cb:lightwhite]Test".ToAnsiFromConsoleMarkup();
            await Assert.That(actual).IsEqualTo(expected);
        }

        #endregion
    }
}
