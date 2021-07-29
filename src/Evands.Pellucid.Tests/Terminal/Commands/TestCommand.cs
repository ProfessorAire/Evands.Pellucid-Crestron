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
        [Verb("", "v3", "Does some default thing.")]
        public void VerbThree()
        {
            ConsoleBase.WriteLine("Does some default thing.");
        }

        [Verb("test", "t", "Used for sample help.")]
        [Sample("TestCommand test --fluff", "Sample One")]
        [Sample("TestCommand t -f", "Sample Two")]
        public void TestStuff(
            [Flag("fluff", 'f', "Just a fluff flag.")] bool fluff)
        {
            ConsoleBase.WriteLine("Does Fluffy Stuff.");
        }

        [Verb("test2", "t2", "Used for operand help.")]
        public void TestOperandHelp(
            [Operand("fluff", "Provides fluff.")] string fluff,
            [Operand("stuff", "Provides stuff.")] string stuff)
        {
            ConsoleBase.WriteLine("Fluff: '{0}' and Stuff: '{1}'.", fluff, stuff);
        }

        [Verb("test3", "t3", "Used for flag help.")]
        public void TestFlagHelp(
            [Flag("fluff", "Indicates fluff.")] string fluff,
            [Flag("stuff", 's', "Indicates stuff.", true)] string stuff)
        {
            ConsoleBase.WriteLine("Fluff: '{0}' and Stuff: '{1}'.", fluff, stuff);
        }
    }

    [Command("TestCommand2", "TC2", "Test command 2 help.")]
    public class TestCommand2 : TerminalCommandBase
    {
        public TestCommand2()
            : base()
        {
        }

        public TestCommand2(string suffix)
            : base(suffix)
        {
        }

        [DefaultVerb]
        [Verb("Test", 1, "Test default command.")]
        public void Test()
        {
            ConsoleBase.WriteLine("Default test command executed.");
        }
    }

    [Command("TestCommand3", "TC3", "Test command 3 help.")]
    public class TestCommand3 : TerminalCommandBase<string>
    {
        public TestCommand3(string value)
            : base(value)
        {
        }

        public TestCommand3(string value, string suffix)
            : base(value, suffix)
        {
        }

        [Verb("Test", 1, "Help")]
        public void Test()
        {
        }
    }

    [Command("TestCommand4", "Test command 4 help.")]
    public class TestCommand4 : TerminalCommandBase
    {
        public TestCommand4(string name)
        {
            this.Name = name;
        }

        [Verb("Test", 2, "Help")]
        public void Test()
        {
        }
    }
}
