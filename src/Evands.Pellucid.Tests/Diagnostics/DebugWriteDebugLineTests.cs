using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Evands.Pellucid;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Summary description for DebugTests
    /// </summary>
    public class DebugWriteDebugLineTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
        }

        [After(Test)]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.MaxDebugMessageLength = -1;
        }
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
#region WriteLines

        [Test]
        public async Task WriteDebugLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Debug.WriteDebugLine(this, linesToTest[i]);
                await Assert.That(writer.Messages.Last().Contains(linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteDebugLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteDebugLine(this, linesToTest[2], "Something");
        
            });
        }

        #endregion

        #region Writes

        [Test]
        public async Task WriteDebug_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesToTest.Length; i++)
            {
                Debug.WriteDebug(this, linesToTest[i]);
                await Assert.That(writer.Messages.Last().Contains(linesToTest[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteDebug_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteDebug(this, linesToTest[2], "Something");
        
            });
        }

        #endregion

        #region WithLimits

        [Test]
        public async Task Write_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.Write((object)null, ConsoleBase.Colors.Green, "12345678900987654321ABC");
            Console.Write(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("123456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.Write((object)null, ConsoleBase.Colors.Green, "22345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("223456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteDebug_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteDebug(null, "32345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("323456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteDebugLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteDebugLine(null, "42345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("4234567890098<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteSuccess_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteSuccess(null, "52345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("523456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteSuccessLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteSuccessLine(null, "62345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("6234567890098<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteProgress_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteProgress(null, "72345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("723456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteProgressLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteProgressLine(null, "82345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("8234567890098<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteNotice_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteNotice(null, "92345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("923456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteNoticeLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteNoticeLine(null, "02345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("0234567890098<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteWarning_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteWarning(null, "82345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("823456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteWarningLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteWarningLine(null, "72345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("7234567890098<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteError_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 20;
            Debug.WriteError(null, "62345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("623456789009<...>1ABC")).IsTrue();
        }

        [Test]
        public async Task WriteErrorLine_With_Limit_Prints_Expected()
        {
            Options.Instance.MaxDebugMessageLength = 21;
            Debug.WriteErrorLine(null, "52345678900987654321ABC");
            Console.WriteLine(writer.Messages.Last());
            await Assert.That(writer.Messages.Last().Contains("5234567890098<...>1ABC")).IsTrue();
        }

        #endregion
    }
}
