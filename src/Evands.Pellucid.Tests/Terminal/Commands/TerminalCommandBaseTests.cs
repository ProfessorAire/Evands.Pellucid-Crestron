using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    public class TerminalCommandBaseTests
    {
        private class TestBaseCommand : TerminalCommandBase
        {
        }

        [Test]
        public async Task CTor_Names_MatchConstructor()
        {
            var suffix = "value";
            var tc2 = new TestCommand2(suffix);
            await Assert.That(tc2.Name == "TestCommand2" + suffix).IsTrue();
            await Assert.That(tc2.Alias == "TC2" + suffix).IsTrue();
        }

        [Test]
        public async Task RegisterCommand_Returns_NoCommandAttributeFound_When_NonePresent()
        {
            var t = new TestBaseCommand();
            var result = t.RegisterCommand("test");

            await Assert.That(result == RegisterResult.NoCommandAttributeFound).IsTrue();
        }

        [Test]
        public async Task UnregisterCommand_ReturnsValue()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var t = new TestCommand();
            var gc = new GlobalCommand("ValueTest", "help", Access.Administrator);
            var added = gc.AddToConsole();
            await Assert.That(added).IsTrue();
            var reg = t.RegisterCommand("ValueTest");
            await Assert.That(reg == RegisterResult.Success).IsTrue();
            var result = t.UnregisterCommand("ValueTest");
            await Assert.That(result).IsTrue();
            gc.RemoveFromConsole();
            gc.Dispose();
        }

        [Test]
        public async Task SetName_Throws_InvalidOperationException_When_Registered()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var t = new TestCommand2();
            var gc = new GlobalCommand("SomethingTest", "t", Access.Administrator);
            var added = gc.AddToConsole();
            await Assert.That(added).IsTrue();
            var reg = t.RegisterCommand("SomethingTest");
            await Assert.That(reg == RegisterResult.Success).IsTrue();
            var threw = false;
            try
            {
                t.Name = "Throws Exception";
            }
            catch (InvalidOperationException)
            {
                threw = true;
            }

            gc.RemoveFromConsole();
            gc.Dispose();

            await Assert.That(threw).IsTrue();
        }

        [Test]
        public async Task SetName_SetsName_When_NotRegistered()
        {
            var t = new TestCommand2();
            await Assert.That(t.Name == "TestCommand2").IsTrue();
            var expected = "NewName";
            t.Name = expected;
            await Assert.That(t.Name == expected).IsTrue();
        }

        [Test]
        public async Task CommandExecute_When_NameSetManually()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var t = new TestCommand2();
            t.Name = "NewName";
            var gc = new GlobalCommand("SomethingTest", "t", Access.Administrator);
            var added = gc.AddToConsole();
            await Assert.That(added).IsTrue();
            var reg = t.RegisterCommand("SomethingTest");
            await Assert.That(reg == RegisterResult.Success).IsTrue();
            Crestron.SimplSharp.CrestronConsole.Messages = new StringBuilder();
            gc.ExecuteCommand("NewName Test");
            await Assert.That(Crestron.SimplSharp.CrestronConsole.Messages.ToString().Contains("Default test command executed.")).IsTrue();

            gc.RemoveFromConsole();
            gc.Dispose();
        }

        [Test]
        public async Task SetAlias_Throws_InvalidOperationException_When_Registered()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var t = new TestCommand2();
            var gc = new GlobalCommand("SomethingTest", "t", Access.Administrator);
            var added = gc.AddToConsole();
            await Assert.That(added).IsTrue();
            var reg = t.RegisterCommand("SomethingTest");
            await Assert.That(reg == RegisterResult.Success).IsTrue();
            var threw = false;
            try
            {
                t.Alias = "Throws Exception";
            }
            catch (InvalidOperationException)
            {
                threw = true;
            }

            gc.RemoveFromConsole();
            gc.Dispose();

            await Assert.That(threw).IsTrue();
        }

        [Test]
        public async Task SetAlias_SetsAlias_When_NotRegistered()
        {
            var t = new TestCommand2();
            await Assert.That(t.Name == "TestCommand2").IsTrue();
            var expected = "NewAlias";
            t.Alias = expected;
            await Assert.That(t.Alias == expected).IsTrue();
        }

        [Test]
        public async Task CommandExecute_When_AliasSetManually()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var t = new TestCommand2();
            t.Alias = "NN";
            var gc = new GlobalCommand("SomethingTest", "t", Access.Administrator);
            var added = gc.AddToConsole();
            await Assert.That(added).IsTrue();
            var reg = t.RegisterCommand("SomethingTest");
            await Assert.That(reg == RegisterResult.Success).IsTrue();
            Crestron.SimplSharp.CrestronConsole.Messages = new StringBuilder();
            gc.ExecuteCommand("NN Test");
            await Assert.That(Crestron.SimplSharp.CrestronConsole.Messages.ToString().Contains("Default test command executed.")).IsTrue();

            gc.RemoveFromConsole();
            gc.Dispose();
        }
    }
}
