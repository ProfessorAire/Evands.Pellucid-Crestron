using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Terminal.Commands
{
    [Command("TestCommand", 4, "Test command help.")]
    public class TestCommand : TerminalCommandBase
    {
        [Verb("VerbOne", 3, "Does some thing.")]
        public void VerbOne()
        {
            ConsoleBase.WriteLine("Does some thing.");
        }

        [Verb("VerbTwo", "v2", "Does some thing2.")]
        public void VerbTwo()
        {
            ConsoleBase.WriteLine("Does some thing2.");
        }

        [DefaultVerb]
        [Verb("VerbThree", "v3", "Does some default thing.")]
        public void VerbThree()
        {
            ConsoleBase.WriteLine("Does some default thing.");
        }
    }

    [Command("TestCommand2", "TC2", "Test command 2 help.")]
    public class TestCommand2 : TerminalCommandBase
    {
        [DefaultVerb]
        [Verb("Test", 1, "Test default command.")]
        public void Test()
        {
            ConsoleBase.WriteLine("Default test command executed.");
        }
    }
}
