using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evands.Pellucid.Terminal.Commands;

namespace Evands.Pellucid
{
    public class ConsoleCommandsTests
    {
        private class TestBaseCommand : TerminalCommandBase
        {
        }

        [Test]
        public async Task Enable_EnablesColorizingConsole()
        {
            Options.Instance.ColorizeConsoleOutput = false;

            var cc = new ConsoleCommands();
            cc.Enable(true);

            await Assert.That(Options.Instance.ColorizeConsoleOutput).IsTrue();
        }

        [Test]
        public async Task Disable_DisablesColorizingConsole()
        {
            Options.Instance.ColorizeConsoleOutput = true;

            var cc = new ConsoleCommands();
            cc.Disable(true);

            await Assert.That(Options.Instance.ColorizeConsoleOutput).IsFalse();
        }
    }
}