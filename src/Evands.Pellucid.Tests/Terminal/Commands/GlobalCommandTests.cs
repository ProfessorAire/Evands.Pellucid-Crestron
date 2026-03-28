using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Terminal.Commands
{
    public class GlobalCommandTests
    {
        private GlobalCommand underTest;
        private TestConsoleWriter testWriter;

        private class InvalidCommand : TerminalCommandBase
        {
        }

        [Command("cmd", "")]
        private class InvalidInternals : TerminalCommandBase
        {
            [Verb("v", "help")]
            public void DoesNothing([Flag("f", "h")] bool a) { ConsoleBase.WriteLineNoHeader("NOTHING"); }

            [Verb("x", "Exception")]
            public void ThrowsException() { throw new TerminalCommandException(new NullReferenceException(), "TEST MESSAGE"); }

            [Verb("x2", "Exception")]
            public void ThrowsException2() { throw new NullReferenceException("TEST NAME"); }

            [Verb("some", "help")]
            public void Something1(
                [Operand("v", "help")] string value)
            {
                ConsoleBase.WriteLineNoHeader("NOTHING1");
            }

            [Verb("some", "help")]
            public void Something2(
                [Operand("v", "help")] string value,
                [Operand("w", "help")] string value2)
            {
                ConsoleBase.WriteLineNoHeader("NOTHING2");
            }

            [Verb("some", "help")]
            public void Something3(
                [Operand("a", "help")] bool value)
            {
                ConsoleBase.WriteLineNoHeader("NOTHING3");
            }

            [Verb("opt", "help")]
            public void Optional(
                [Flag("opt", "help", true)] bool optional)
            {
                if (!optional)
                {
                    ConsoleBase.WriteLineNoHeader("OPTIONAL");
                }
            }

            [Verb("typeTest", "help")]
            public void TestTypes(
                [Operand("bool1", "")] bool b1,
                [Operand("bool2", "")] bool b2,
                [Operand("bool3", "")] bool b3,
                [Operand("bool4", "")] bool b4,
                [Operand("bool5", "")] bool b5,
                [Operand("bool6", "")] bool b6,
                [Operand("int1", "")] int i1,
                [Operand("int2", "")] ushort i2,
                [Operand("int3", "")] uint i3,
                [Operand("int4", "")] double i4,
                [Operand("int5", "")] long l1
                )
            {
                ConsoleBase.WriteLineNoHeader("SUCCESS");
            }
        }

        [Before(Test)]
        public void MyTestInitialize()
        {
            underTest = new GlobalCommand("app", "app", Access.Administrator);
            underTest.AddToConsole();
            testWriter = new TestConsoleWriter();
            ConsoleBase.RegisterConsoleWriter(testWriter);
        }
        [After(Test)]
        public void MyTestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            underTest.RemoveFromConsole();
            underTest.Dispose();
            underTest = null;
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            Crestron.SimplSharp.CrestronEnvironment.DevicePlatform = Crestron.SimplSharp.eDevicePlatform.Appliance;
            ConsoleBase.UnregisterConsoleWriter(testWriter);
            testWriter = null;
        }

        [Test]
        public async Task Constructor_Throws_ArgumentOutOfRangeException_When_NameTooLong()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {

                var gc = new GlobalCommand("This name is longer than 23 characters.", "Help", Access.Administrator);
        
            });
        }

        [Test]
        public async Task Constructor_Throws_ArgumentOutOfRangeException_When_Name_Null()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {

                var gc = new GlobalCommand(null, "Help", Access.Administrator);
        
            });
        }

        [Test]
        public async Task Constructor_Throws_ArgumentOutOfRangeException_When_Name_Empty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {

                var gc = new GlobalCommand(string.Empty, "Help", Access.Administrator);
        
            });
        }

        [Test]
        public async Task Constructor_Sets_Name()
        {
            var expected = "ex";
            var gc = new GlobalCommand(expected, "help", Access.Administrator);

            await Assert.That(gc.Name == expected).IsTrue();
        }

        [Test]
        public async Task Constructor_Sets_Help_WhenShortHelp()
        {
            var help = "HelpTest";
            var gc = new GlobalCommand("TestTest", help, Access.Administrator);

            await Assert.That(gc.Help == help).IsTrue();
        }

        [Test]
        public async Task Constructor_Sets_Help_WhenEmpty()
        {
            var help = string.Empty;
            var gc = new GlobalCommand("TestTest", help, Access.Administrator);

            await Assert.That(gc.Help == help).IsTrue();
        }

        [Test]
        public async Task Constructor_Truncates_Help_When_TooLong()
        {
            var help = "This help is longer than 79 characters long and is going to be truncated by the constructor.";
            var expected = "This help is longer than 79 characters long and is going to be truncated by ...";
            var gc = new GlobalCommand("TestTest", help, Access.Administrator);

            Trace.WriteLine("Help: '" + gc.Help + "'");

            await Assert.That(gc.Help == expected).IsTrue();
        }

        [Test]
        public async Task Constructor_Sets_CommandAccess_Tests()
        {
            var gc1 = new GlobalCommand("Test", "Help", Access.Operator);
            await Assert.That(gc1.CommandAccess == Access.Operator).IsTrue();

            var gc2 = new GlobalCommand("Test", "Help", Access.Programmer);
            await Assert.That(gc2.CommandAccess == Access.Programmer).IsTrue();

            var gc3 = new GlobalCommand("Test", "Help", Access.Administrator);
            await Assert.That(gc3.CommandAccess == Access.Administrator).IsTrue();
        }

        [Test]
        public async Task WriteErrorMethod_Property_Sets()
        {
            Action<string> expected = (s) => Trace.WriteLine(s);
            underTest.WriteErrorMethod = expected;

            await Assert.That(underTest.WriteErrorMethod == expected).IsTrue();
        }

        [Test]
        public async Task WriteHelpMethod_Property_Sets()
        {
            Action<string> expected = (s) => Trace.WriteLine(s);
            underTest.WriteHelpMethod = expected;

            await Assert.That(underTest.WriteHelpMethod == expected).IsTrue();
        }

        [Test]
        public async Task FormatHelpCommandMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpCommandMethod = expected;

            await Assert.That(underTest.FormatHelpCommandMethod == expected).IsTrue();
        }

        [Test]
        public async Task FormatHelpVerbMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpVerbMethod = expected;

            await Assert.That(underTest.FormatHelpVerbMethod == expected).IsTrue();
        }

        [Test]
        public async Task FormatHelpOperandMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpOperandMethod = expected;

            await Assert.That(underTest.FormatHelpOperandMethod == expected).IsTrue();
        }

        [Test]
        public async Task FormatHelpFlagMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpFlagMethod = expected;

            await Assert.That(underTest.FormatHelpFlagMethod == expected).IsTrue();
        }

        [Test]
        public async Task FormatHelpSampleMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpSampleMethod = expected;

            await Assert.That(underTest.FormatHelpSampleMethod == expected).IsTrue();
        }

        [Test]
        public async Task FormatHelpTextMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpTextMethod = expected;

            await Assert.That(underTest.FormatHelpTextMethod == expected).IsTrue();
        }

        [Test]
        public async Task AddToConsole_ReturnsTrue_WhenNoOtherCommandAdded()
        {
            var expected = true;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;

            var actual = gc.AddToConsole();

            await Assert.That(expected == actual).IsTrue();
            gc.RemoveFromConsole();
        }

        [Test]
        public async Task AddToConsole_ReturnsFalse_WhenCrestronConsole_ReturnsFalse()
        {
            var expected = false;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = false;
            var actual = gc.AddToConsole();
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;

            await Assert.That(expected == actual).IsTrue();
            gc.RemoveFromConsole();
        }

        [Test]
        public async Task AddToConsole_ReturnsFalse_WhenSameCommandAdded()
        {
            var expected = false;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            gc.AddToConsole();

            var gc2 = new GlobalCommand("aps", "help", Access.Administrator);
            var actual = gc2.AddToConsole();

            await Assert.That(expected == actual).IsTrue();
            gc.RemoveFromConsole();
        }

        [Test]
        public async Task AddToConsole_ReturnsTrue_WhenServer_AndConsoleReturnsFalse()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = false;
            Crestron.SimplSharp.CrestronEnvironment.DevicePlatform = Crestron.SimplSharp.eDevicePlatform.Server;

            var gc = new GlobalCommand("spa", "Help", Access.Administrator);
            var actual = gc.AddToConsole();

            await Assert.That(actual).IsTrue();
        }

        [Test]
        public async Task RemoveFromConsole_ReturnsTrue_WhenCommandAdded()
        {
            var expected = true;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            gc.AddToConsole();
            var actual = gc.RemoveFromConsole();

            await Assert.That(actual == expected).IsTrue();
        }

        [Test]
        public async Task RemoveFromConsole_ReturnsFalse_WhenNoCommandAdded()
        {
            var expected = false;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            var actual = gc.RemoveFromConsole();

            await Assert.That(actual == expected).IsTrue();
        }

        [Test]
        public async Task AddCommand_Returns_Success()
        {
            var expected = RegisterResult.Success;
            var c1 = new TestCommand();
            var actual = underTest.AddCommand(c1);

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task AddCommand_Returns_GlobalCommandNotFound_WhenNoGlobalRegistered()
        {
            var expected = RegisterResult.GlobalCommandNotFound;
            var c1 = new TestCommand();
            var actual = c1.RegisterCommand("missing");

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task AddCommand_Returns_NoCommandAttributeFound()
        {
            var expected = RegisterResult.NoCommandAttributeFound;
            var c1 = new InvalidCommand();
            var actual = underTest.AddCommand(c1);

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task AddCommand_AllowsMultiple_CustomName()
        {
            var expected = RegisterResult.Success;
            var c1 = new TestCommand4("CustomName1");
            var c2 = new TestCommand4("ADifferentName");

            await Assert.That(underTest.AddCommand(c1) == expected).IsTrue();
            await Assert.That(underTest.AddCommand(c2) == expected).IsTrue();
        }

        [Test]
        public async Task RemoveCommand_Returns_True_When_CommandRemoved()
        {
            var expected = true;
            var c1 = new TestCommand();
            underTest.AddCommand(c1);
            var actual = underTest.RemoveCommand(c1);

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task RemoveCommand_Returns_False_When_NoCommandRemoved()
        {
            var expected = false;
            var c1 = new TestCommand();
            var actual = underTest.RemoveCommand(c1);

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task IsCommandRegistered_Returns_True_When_CommandRegistered()
        {
            var expected = true;
            var c1 = new TestCommand();
            underTest.AddCommand(c1);

            var actual = underTest.IsCommandRegistered(c1);

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task IsCommandRegistered_Returns_False_When_CommandRegistered()
        {
            var expected = false;
            var c1 = new TestCommand();
            var actual = underTest.IsCommandRegistered(c1);

            await Assert.That(expected == actual).IsTrue();
        }

        [Test]
        public async Task Dispose_RemovesAllCommands_When_Called()
        {
            var c1 = new TestCommand();

            var gc = new GlobalCommand("temp", "help", Access.Administrator);
            gc.AddToConsole();
            gc.AddCommand(c1);

            await Assert.That(gc.IsCommandRegistered(c1)).IsTrue();

            gc.Dispose();

            await Assert.That(gc.IsCommandRegistered(c1)).IsFalse();
        }

        [Test]
        public async Task Dispose_RemovesFromConsole_When_Called()
        {
            var c1 = new TestCommand();

            var gc = new GlobalCommand("temp", "help", Access.Administrator);
            gc.AddToConsole();
            gc.Dispose();

            await Assert.That(gc.RemoveFromConsole()).IsFalse();
        }

        [Test]
        public async Task DuplicateFlag_WritesError()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v -f -f -f -f");

            await Assert.That(testWriter.Contains("Duplicate operand or flag names are not allowed!")).IsTrue();
            await Assert.That(testWriter.Contains("NOTHING")).IsFalse();
        }

        [Test]
        public async Task DuplicateOperand_WritesError()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f temp --f");

            await Assert.That(testWriter.Contains("Duplicate operand or flag names are not allowed!")).IsTrue();
            await Assert.That(testWriter.Contains("NOTHING")).IsFalse();
        }

        [Test]
        public async Task BasicCommand_PerformsAction_Coverage1()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f \"value\"");

            await Assert.That(testWriter.Contains("NOTHING")).IsTrue();
        }

        [Test]
        public async Task BasicCommand_PerformsAction_Coverage2()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f value");

            await Assert.That(testWriter.Contains("NOTHING")).IsTrue();
        }

        [Test]
        public async Task BasicCommand_PerformsAction_Coverage3()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f");

            await Assert.That(testWriter.Contains("NOTHING")).IsTrue();
        }

        [Test]
        public async Task BasicCommand_PerformsAction_Coverage4()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd some -v");

            await Assert.That(testWriter.Contains("NOTHING")).IsTrue();
        }

        [Test]
        public async Task BasicCommand_PerformsAction_Coverage5()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd some -v -x");

            await Assert.That(testWriter.Contains("The verb 'some' requires a different combination of operands than what was provided.")).IsTrue();
        }

        [Test]
        public async Task BasicCommand_WithInvalidParamType_WritesMessage()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd some --a bah");

            await Assert.That(testWriter.Contains("Unable to convert the operand 'a' with the value 'bah' to the expected type value 'System.Boolean'.")).IsTrue();
        }

        [Test]
        public async Task ConvertParameter_ConvertsType_Success()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 \"-123\" --int2 65535 --int3 123 --int4 123.43 --int5 423123");

            await Assert.That(testWriter.Contains("SUCCESS")).IsTrue();
        }

        [Test]
        public async Task ConvertParameter_ConvertsType_Failure1()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 gbg --int2 65535 --int3 123 --int4 123.43 --int5 423123");

            await Assert.That(testWriter.Contains("SUCCESS")).IsFalse();
        }

        [Test]
        public async Task ConvertParameter_ConvertsType_Failure2()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 gbg --int3 123 --int4 123.43 --int5 423123");

            await Assert.That(testWriter.Contains("SUCCESS")).IsFalse();
        }

        [Test]
        public async Task ConvertParameter_ConvertsType_Failure3()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 132 --int3 gbg --int4 123.43 --int5 423123");

            await Assert.That(testWriter.Contains("SUCCESS")).IsFalse();
        }

        [Test]
        public async Task ConvertParameter_ConvertsType_Failure4()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 132 --int3 123 --int4 gbg --int5 423123");

            await Assert.That(testWriter.Contains("SUCCESS")).IsFalse();
        }

        [Test]
        public async Task ConvertParameter_ConvertsType_Failure5()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 132 --int3 123 --int4 123 --int5 gbg");

            await Assert.That(testWriter.Contains("SUCCESS")).IsFalse();
        }

        [Test]
        public async Task ExecuteCommand_WithNoOptionalFlag_ExecutesCommand()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd opt");

            await Assert.That(testWriter.Contains("OPTIONAL")).IsTrue();
        }

        [Test]
        public async Task ExecuteCommand_WithNoCommandName_WritesResponse()
        {
            underTest.ExecuteCommand("-v");
            await Assert.That(testWriter.Contains("You must enter the name of a command.")).IsTrue();
        }

        [Test]
        public async Task ExecuteCommand_WithInvalidCommandName_WritesResponse()
        {
            underTest.ExecuteCommand("coms");
            await Assert.That(testWriter.Contains("No command with the name 'coms' exists.")).IsTrue();
        }

        [Test]
        public async Task ExecuteCommand_WithNoVerb_WritesResponse()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd");
            await Assert.That(testWriter.Contains("The command 'cmd' requires a verb.")).IsTrue();
        }

        [Test]
        public async Task ExecuteCommand_WithInvalidVerb_WritesResponse()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd not");
            await Assert.That(testWriter.Contains("No verb with the specified name 'not' exists.")).IsTrue();
        }

        [Test]
        public async Task ExecuteCommand_WithDefaultValue_Coverage()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);
            var handled = false;
            underTest.CommandExceptionEncountered += (o, a) => handled = true;
            underTest.ExecuteCommand("cmd not value");
            await Assert.That(handled).IsFalse();
        }

        [Test]
        public async Task ExecuteCommand_ProcessesException()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd x");

            await Assert.That(testWriter.Contains("TEST MESSAGE") && testWriter.Contains("System.NullReferenceException")).IsTrue();
        }

        [Test]
        public async Task ExecuteCommand_Handler_ProcessesException()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);
            var handled = false;
            EventHandler<TerminalCommandExceptionEventArgs> handler = (o, a) => handled = true;

            underTest.CommandExceptionEncountered += handler;
            underTest.ExecuteCommand("cmd x2");
            underTest.CommandExceptionEncountered -= handler;
            await Assert.That(handled).IsTrue();
        }

        [Test]
        public async Task GlobalHelp_IsPrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("--help");
            await Assert.That(testWriter.Contains("testCommand (test)") &&
                testWriter.Contains("Test command help.")).IsTrue();
        }

        [Test]
        public async Task GlobalHelp_IsPrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("-h");
            await Assert.That(testWriter.Contains("testCommand (test)") &&
                testWriter.Contains("Test command help.")).IsTrue();
        }

        [Test]
        public async Task GlobalHelp_Prints_CommandNamesWithSuffix_When_SuffixPresent()
        {
            var c1 = new TestCommand3("string", "s1");
            var c2 = new TestCommand3("string", "s2");
            underTest.AddCommand(c1);
            underTest.AddCommand(c2);

            underTest.ExecuteCommand("-h");
            await Assert.That(testWriter.Contains("testCommand3s1 (tc3s1)") &&
                testWriter.Contains("testCommand3s2 (tc3s2)")).IsTrue();
        }

        [Test]
        public async Task GlobalHelp_Prints_CommandNames_Alphabetically()
        {
            var c3 = new TestCommand4("Zed");
            var c1 = new TestCommand();
            var c2 = new TestCommand2();

            underTest.AddCommand(c3);
            underTest.AddCommand(c2);
            underTest.AddCommand(c1);

            Options.Instance.ColorizeConsoleOutput = false;

            underTest.ExecuteCommand("-h");

            var expected = @"Listing the commands available for 'app'
-----
testCommand (test)      Test command help.
testCommand2 (tc2)      Test command 2 help.
zed                     Test command 4 help.
-----
".Replace("\n", "\r\n");

            await Assert.That(testWriter.ToString() == expected).IsTrue();

            Options.Instance.ColorizeConsoleOutput = true;
        }

        [Test]
        public async Task VerbHelp_Sample_IsPrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand test --help");

            await Assert.That(testWriter.Contains("TestCommand test --fluff") &&
                testWriter.Contains("Sample One")).IsTrue();

            await Assert.That(testWriter.Contains("TestCommand t -f") &&
                testWriter.Contains("Sample Two")).IsTrue();
        }

        [Test]
        public async Task VerbHelp_Sample_IsPrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand test -h");

            await Assert.That(testWriter.Contains("TestCommand test --fluff") &&
                testWriter.Contains("Sample One")).IsTrue();

            await Assert.That(testWriter.Contains("TestCommand t -f") &&
                testWriter.Contains("Sample Two")).IsTrue();
        }

        [Test]
        public async Task VerbHelp_Operands_ArePrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t2 --help");

            await Assert.That(testWriter.Contains("--fluff") &&
                testWriter.Contains("Provides fluff.")).IsTrue();

            await Assert.That(testWriter.Contains("--stuff") &&
                testWriter.Contains("Provides stuff.")).IsTrue();
        }

        [Test]
        public async Task VerbHelp_Operands_ArePrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t2 -h");

            await Assert.That(testWriter.Contains("--fluff") &&
                testWriter.Contains("Provides fluff.")).IsTrue();

            await Assert.That(testWriter.Contains("--stuff") &&
                testWriter.Contains("Provides stuff.")).IsTrue();
        }

        [Test]
        public async Task VerbHelp_Flags_ArePrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t3 --help");

            await Assert.That(testWriter.Contains("--fluff") &&
                testWriter.Contains("Indicates fluff.")).IsTrue();

            await Assert.That(testWriter.Contains("--stuff, -s (optional)") &&
                testWriter.Contains("Indicates stuff.")).IsTrue();
        }

        [Test]
        public async Task VerbHelp_Flags_ArePrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t3 -h");

            await Assert.That(testWriter.Contains("--fluff") &&
                testWriter.Contains("Indicates fluff.")).IsTrue();

            await Assert.That(testWriter.Contains("--stuff, -s (optional)") &&
                testWriter.Contains("Indicates stuff.")).IsTrue();
        }

        [Test]
        public async Task CommandHelp_Verbs_WithNoName_Show_FormattedNone()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand --help");

            await Assert.That(testWriter.Contains("<none>")).IsTrue();
        }

        [Test]
        public async Task ValidateWriter_DoesNothing_When_NewValidator_IsNull()
        {
            underTest.WriteErrorMethod = (s) => { ConsoleBase.Write(s); };
            underTest.WriteErrorMethod("temp");
            await Assert.That(testWriter.Contains("temp") && !testWriter.Contains("temp" + ConsoleBase.NewLine)).IsTrue();

            underTest.WriteErrorMethod = null;
            underTest.WriteErrorMethod("Error");
            await Assert.That(testWriter.Contains("Error")).IsTrue();
        }

        [Test]
        public async Task ValidateFormatter_DoesNothing_When_NewValidator_IsNull()
        {
            underTest.FormatHelpCommandMethod = (s) => s + "1";
            var result = underTest.FormatHelpCommandMethod("temp");
            await Assert.That(result == "temp1").IsTrue();

            underTest.FormatHelpCommandMethod = null;
            result = underTest.FormatHelpCommandMethod("temp");
            await Assert.That(result == "temp").IsTrue();
        }

        [Test]
        public async Task GetAllGlobalCommands_ReturnsAllGlobalCommands()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var gc2 = new GlobalCommand("sap", "Test", Access.Administrator);
            gc2.AddToConsole();

            var gc3 = new GlobalCommand("spsa", "Test", Access.Operator);
            gc3.AddToConsole();

            var values = GlobalCommand.GetAllGlobalCommands();

            await Assert.That(values.Contains(gc2)).IsTrue();
            await Assert.That(values.Contains(gc3)).IsTrue();
            await Assert.That(values.Contains(underTest)).IsTrue();
        }
    }
}
