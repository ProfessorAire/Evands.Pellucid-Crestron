using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Summary description for TerminalCommandTests
    /// </summary>
    public class TerminalCommandTests
    {
        private GlobalCommand global;

        private TestCommand command;

        private TestConsoleWriter writer;

        public TerminalCommandTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }
         [Before(Test)]
         public void MyTestInitialize()
         {
             global = new GlobalCommand("app", "App Command", Access.Administrator);
             global.AddToConsole();
             command = new TestCommand();
             global.AddCommand(command);
             global.AddCommand(new TestCommand2());
         }
         [After(Test)]
         public void MyTestCleanup()
         {
             Options.UseDefault();
             ConsoleBase.NewLine = Environment.NewLine;
             global.RemoveCommand(command);
             global = null;
             command = null;
             CrestronConsole.Messages.Length = 0;
             if (writer != null)
             {
                 ConsoleBase.UnregisterConsoleWriter(writer);
             }
         }

        [Test]
        public async Task Command_WithAlias_CalledByAlias_Functions()
        {
            var expected = "Does some thing.";
            global.ExecuteCommand("test verbone");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task Command_WithAlias_AskedForHelp_PrintsAliasCorrectly()
        {
            var expected = "testCommand (test)";
            global.ExecuteCommand("test -h");
            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task Verb_WithSubstringAlias_CalledByAlias_Functions()
        {
            var expected = "Does some thing.";
            global.ExecuteCommand("test ver");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task Verb_WithSubstringAlias_AskedForHelp_PrintsAliasCorrectly()
        {
            var expected = "VerbOne (Ver)";
            global.ExecuteCommand("test ver -h");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task Verb_WithStringAlias_CalledByAlias_Functions()
        {
            var expected = "Does some thing2.";
            global.ExecuteCommand("test v2");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task Verb_WithStringAlias_AskedForHelp_PrintsAliasCorrectly()
        {
            var expected = "(v2)";
            global.ExecuteCommand("test v2 -h");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task DefaultVerb_Functions()
        {
            var expected = "Does some default thing.";
            global.ExecuteCommand("test");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task CommandAlias_With_DefaultVerb_Functions()
        {
            var expected = "Default test command executed.";
            global.ExecuteCommand("tc2");

            await Assert.That(CrestronConsole.Messages.ToString().Contains(expected)).IsTrue();
        }

        [Test]
        public async Task CommandAlias_Shows_InTopLevelHelp()
        {
            var expected = "(test)";
            Options.Instance.ColorizeConsoleOutput = false;
            writer = new TestConsoleWriter();
            ConsoleBase.RegisterConsoleWriter(writer);
            global.ExecuteCommand("-h");
            await Assert.That(writer.Contains(expected)).IsTrue();
            Options.Instance.ColorizeConsoleOutput = true;
        }

        [Test]
        public async Task RegisterCommand_WithExistingName_Returns_CommandInUse()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var expected = RegisterResult.CommandNameAlreadyExists;
            var gc = new GlobalCommand("temp", "temp", Access.Administrator);
            gc.AddToConsole();
            var c1 = new TestCommand();
            var c2 = new TestCommand();

            c1.RegisterCommand("temp");
            var actual = c2.RegisterCommand("temp");
            System.Diagnostics.Trace.WriteLine("Register result: '" + actual + "'");
            await Assert.That(expected == actual).IsTrue();

            c1.UnregisterCommand();
            gc = null;
        }
    }
}
