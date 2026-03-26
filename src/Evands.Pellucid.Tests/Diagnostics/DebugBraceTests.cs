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
    public class DebugBraceTests
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
        }
        private string[] linesWithBraces = new string[]
        {
            "{ \"Message\": \"MessageContents\", \"Item\": { \"ItemName\": \"Name\" } }",
            "Just a simple string with {0} a brace inside it.",
            "A string with {0} formatting braces and {other braces}"
        };
#region WriteLines

        [Test]
        public async Task WriteDebugLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteDebugLine(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteDebugLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteDebugLine(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteProgressLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteProgressLine(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteProgressLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteProgressLine(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteWarningLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteWarningLine(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteWarningLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteWarningLine(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteErrorLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteErrorLine(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteErrorLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteErrorLine(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteNoticeLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteNoticeLine(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteNoticeLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteNoticeLine(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteSuccessLine_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteSuccessLine(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteSuccessLine_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteSuccessLine(this, linesWithBraces[2], "Something");
        
            });
        }

        #endregion

        #region Writes

        [Test]
        public async Task WriteDebug_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteDebug(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteDebug_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteDebug(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteProgress_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteProgress(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteProgress_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteProgress(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteWarning_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteWarning(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteWarning_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteWarning(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteError_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteError(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteError_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteError(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteNotice_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteNotice(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteNotice_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteNotice(this, linesWithBraces[2], "Something");
        
            });
        }

        [Test]
        public async Task WriteSuccess_Contents_ContainsBraces_NoFormatting_PrintsText()
        {
            for (var i = 0; i < linesWithBraces.Length; i++)
            {
                Debug.WriteSuccess(this, linesWithBraces[i]);
                await Assert.That(writer.Messages.Last().Contains(linesWithBraces[i])).IsTrue();
            }
        }

        [Test]
        public async Task WriteSuccess_Contents_ContainsBraces_WithFormatting_ThrowsException()
        {
            Assert.Throws<FormatException>(() =>
            {

                Debug.WriteSuccess(this, linesWithBraces[2], "Something");
        
            });
        }

        #endregion
    }
}
