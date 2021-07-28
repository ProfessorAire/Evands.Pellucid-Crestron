using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Terminal.Commands
{
    [TestClass]
    public class GlobalCommandTests
    {
        private GlobalCommand underTest;

        private TestContext testContextInstance;

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

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            underTest = new GlobalCommand("app", "app", Access.Administrator);
            underTest.AddToConsole();
            testWriter = new TestConsoleWriter();
            ConsoleBase.RegisterConsoleWriter(testWriter);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            underTest.RemoveFromConsole();
            underTest.Dispose();
            underTest = null;
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            Crestron.SimplSharp.CrestronEnvironment.DevicePlatform = Crestron.SimplSharp.eDevicePlatform.Appliance;
            ConsoleBase.UnregisterConsoleWriter(testWriter);
            testWriter = null;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_Throws_ArgumentOutOfRangeException_When_NameTooLong()
        {
            var gc = new GlobalCommand("This name is longer than 23 characters.", "Help", Access.Administrator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_Throws_ArgumentOutOfRangeException_When_Name_Null()
        {
            var gc = new GlobalCommand(null, "Help", Access.Administrator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_Throws_ArgumentOutOfRangeException_When_Name_Empty()
        {
            var gc = new GlobalCommand(string.Empty, "Help", Access.Administrator);
        }

        [TestMethod]
        public void Constructor_Sets_Name()
        {
            var expected = "ex";
            var gc = new GlobalCommand(expected, "help", Access.Administrator);

            Assert.IsTrue(gc.Name == expected);
        }

        [TestMethod]
        public void Constructor_Sets_Help_WhenShortHelp()
        {
            var help = "HelpTest";
            var gc = new GlobalCommand("TestTest", help, Access.Administrator);

            Assert.IsTrue(gc.Help == help);
        }

        [TestMethod]
        public void Constructor_Sets_Help_WhenEmpty()
        {
            var help = string.Empty;
            var gc = new GlobalCommand("TestTest", help, Access.Administrator);

            Assert.IsTrue(gc.Help == help);
        }

        [TestMethod]
        public void Constructor_Truncates_Help_When_TooLong()
        {
            var help = "This help is longer than 79 characters long and is going to be truncated by the constructor.";
            var expected = "This help is longer than 79 characters long and is going to be truncated by ...";
            var gc = new GlobalCommand("TestTest", help, Access.Administrator);

            Trace.WriteLine("Help: '" + gc.Help + "'");

            Assert.IsTrue(gc.Help == expected);
        }

        [TestMethod]
        public void Constructor_Sets_CommandAccess_Tests()
        {
            var gc1 = new GlobalCommand("Test", "Help", Access.Operator);
            Assert.IsTrue(gc1.CommandAccess == Access.Operator);

            var gc2 = new GlobalCommand("Test", "Help", Access.Programmer);
            Assert.IsTrue(gc2.CommandAccess == Access.Programmer);

            var gc3 = new GlobalCommand("Test", "Help", Access.Administrator);
            Assert.IsTrue(gc3.CommandAccess == Access.Administrator);
        }

        [TestMethod]
        public void WriteErrorMethod_Property_Sets()
        {
            Action<string> expected = (s) => Trace.WriteLine(s);
            underTest.WriteErrorMethod = expected;

            Assert.IsTrue(underTest.WriteErrorMethod == expected);
        }

        [TestMethod]
        public void WriteHelpMethod_Property_Sets()
        {
            Action<string> expected = (s) => Trace.WriteLine(s);
            underTest.WriteHelpMethod = expected;

            Assert.IsTrue(underTest.WriteHelpMethod == expected);
        }

        [TestMethod]
        public void FormatHelpCommandMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpCommandMethod = expected;

            Assert.IsTrue(underTest.FormatHelpCommandMethod == expected);
        }

        [TestMethod]
        public void FormatHelpVerbMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpVerbMethod = expected;

            Assert.IsTrue(underTest.FormatHelpVerbMethod == expected);
        }

        [TestMethod]
        public void FormatHelpOperandMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpOperandMethod = expected;

            Assert.IsTrue(underTest.FormatHelpOperandMethod == expected);
        }

        [TestMethod]
        public void FormatHelpFlagMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpFlagMethod = expected;

            Assert.IsTrue(underTest.FormatHelpFlagMethod == expected);
        }

        [TestMethod]
        public void FormatHelpSampleMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpSampleMethod = expected;

            Assert.IsTrue(underTest.FormatHelpSampleMethod == expected);
        }

        [TestMethod]
        public void FormatHelpTextMethod_Property_Sets()
        {
            Func<string, string> expected = (s) => s;
            underTest.FormatHelpTextMethod = expected;

            Assert.IsTrue(underTest.FormatHelpTextMethod == expected);
        }

        [TestMethod]
        public void AddToConsole_ReturnsTrue_WhenNoOtherCommandAdded()
        {
            var expected = true;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;

            var actual = gc.AddToConsole();

            Assert.IsTrue(expected == actual);
            gc.RemoveFromConsole();
        }

        [TestMethod]
        public void AddToConsole_ReturnsFalse_WhenCrestronConsole_ReturnsFalse()
        {
            var expected = false;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = false;
            var actual = gc.AddToConsole();
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;

            Assert.IsTrue(expected == actual);
            gc.RemoveFromConsole();
        }

        [TestMethod]
        public void AddToConsole_ReturnsFalse_WhenSameCommandAdded()
        {
            var expected = false;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            gc.AddToConsole();

            var gc2 = new GlobalCommand("aps", "help", Access.Administrator);
            var actual = gc2.AddToConsole();

            Assert.IsTrue(expected == actual);
            gc.RemoveFromConsole();
        }

        [TestMethod]
        public void AddToConsole_ReturnsTrue_WhenServer_AndConsoleReturnsFalse()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = false;
            Crestron.SimplSharp.CrestronEnvironment.DevicePlatform = Crestron.SimplSharp.eDevicePlatform.Server;

            var gc = new GlobalCommand("spa", "Help", Access.Administrator);
            var actual = gc.AddToConsole();

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void RemoveFromConsole_ReturnsTrue_WhenCommandAdded()
        {
            var expected = true;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            gc.AddToConsole();
            var actual = gc.RemoveFromConsole();

            Assert.IsTrue(actual == expected);
        }

        [TestMethod]
        public void RemoveFromConsole_ReturnsFalse_WhenNoCommandAdded()
        {
            var expected = false;
            var gc = new GlobalCommand("aps", "help", Access.Administrator);
            var actual = gc.RemoveFromConsole();

            Assert.IsTrue(actual == expected);
        }

        [TestMethod]
        public void AddCommand_Returns_Success()
        {
            var expected = RegisterResult.Success;
            var c1 = new TestCommand();
            var actual = underTest.AddCommand(c1);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void AddCommand_Returns_GlobalCommandNotFound_WhenNoGlobalRegistered()
        {
            var expected = RegisterResult.GlobalCommandNotFound;
            var c1 = new TestCommand();
            var actual = c1.RegisterCommand("missing");

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void AddCommand_Returns_NoCommandAttributeFound()
        {
            var expected = RegisterResult.NoCommandAttributeFound;
            var c1 = new InvalidCommand();
            var actual = underTest.AddCommand(c1);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void RemoveCommand_Returns_True_When_CommandRemoved()
        {
            var expected = true;
            var c1 = new TestCommand();
            underTest.AddCommand(c1);
            var actual = underTest.RemoveCommand(c1);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void RemoveCommand_Returns_False_When_NoCommandRemoved()
        {
            var expected = false;
            var c1 = new TestCommand();
            var actual = underTest.RemoveCommand(c1);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void IsCommandRegistered_Returns_True_When_CommandRegistered()
        {
            var expected = true;
            var c1 = new TestCommand();
            underTest.AddCommand(c1);

            var actual = underTest.IsCommandRegistered(c1);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void IsCommandRegistered_Returns_False_When_CommandRegistered()
        {
            var expected = false;
            var c1 = new TestCommand();
            var actual = underTest.IsCommandRegistered(c1);

            Assert.IsTrue(expected == actual);
        }

        [TestMethod]
        public void Dispose_RemovesAllCommands_When_Called()
        {
            var c1 = new TestCommand();

            var gc = new GlobalCommand("temp", "help", Access.Administrator);
            gc.AddToConsole();
            gc.AddCommand(c1);

            Assert.IsTrue(gc.IsCommandRegistered(c1));

            gc.Dispose();

            Assert.IsFalse(gc.IsCommandRegistered(c1));
        }

        [TestMethod]
        public void Dispose_RemovesFromConsole_When_Called()
        {
            var c1 = new TestCommand();

            var gc = new GlobalCommand("temp", "help", Access.Administrator);
            gc.AddToConsole();
            gc.Dispose();

            Assert.IsFalse(gc.RemoveFromConsole());
        }

        [TestMethod]
        public void DuplicateFlag_WritesError()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v -f -f -f -f");

            Assert.IsTrue(testWriter.Contains("Duplicate operand or flag names are not allowed!"));
            Assert.IsFalse(testWriter.Contains("NOTHING"));
        }

        [TestMethod]
        public void DuplicateOperand_WritesError()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f temp --f");

            Assert.IsTrue(testWriter.Contains("Duplicate operand or flag names are not allowed!"));
            Assert.IsFalse(testWriter.Contains("NOTHING"));
        }

        [TestMethod]
        public void BasicCommand_PerformsAction_Coverage1()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f \"value\"");

            Assert.IsTrue(testWriter.Contains("NOTHING"));
        }

        [TestMethod]
        public void BasicCommand_PerformsAction_Coverage2()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f value");

            Assert.IsTrue(testWriter.Contains("NOTHING"));
        }

        [TestMethod]
        public void BasicCommand_PerformsAction_Coverage3()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd v --f");

            Assert.IsTrue(testWriter.Contains("NOTHING"));
        }

        [TestMethod]
        public void BasicCommand_PerformsAction_Coverage4()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd some -v");

            Assert.IsTrue(testWriter.Contains("NOTHING"));
        }

        [TestMethod]
        public void BasicCommand_PerformsAction_Coverage5()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd some -v -x");

            Assert.IsTrue(testWriter.Contains("The verb 'some' requires a different combination of operands than what was provided."));
        }

        [TestMethod]
        public void BasicCommand_WithInvalidParamType_WritesMessage()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd some --a bah");

            Assert.IsTrue(testWriter.Contains("Unable to convert the operand 'a' with the value 'bah' to the expected type value 'System.Boolean'."));
        }

        [TestMethod]
        public void ConvertParameter_ConvertsType_Success()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 \"-123\" --int2 65535 --int3 123 --int4 123.43 --int5 423123");

            Assert.IsTrue(testWriter.Contains("SUCCESS"));
        }

        [TestMethod]
        public void ConvertParameter_ConvertsType_Failure1()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 gbg --int2 65535 --int3 123 --int4 123.43 --int5 423123");

            Assert.IsFalse(testWriter.Contains("SUCCESS"));
        }

        [TestMethod]
        public void ConvertParameter_ConvertsType_Failure2()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 gbg --int3 123 --int4 123.43 --int5 423123");

            Assert.IsFalse(testWriter.Contains("SUCCESS"));
        }

        [TestMethod]
        public void ConvertParameter_ConvertsType_Failure3()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 132 --int3 gbg --int4 123.43 --int5 423123");

            Assert.IsFalse(testWriter.Contains("SUCCESS"));
        }

        [TestMethod]
        public void ConvertParameter_ConvertsType_Failure4()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 132 --int3 123 --int4 gbg --int5 423123");

            Assert.IsFalse(testWriter.Contains("SUCCESS"));
        }

        [TestMethod]
        public void ConvertParameter_ConvertsType_Failure5()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd typeTest --bool1 --bool2 yes --bool3 no --bool4 on --bool5 off --bool6 true --int1 123 --int2 132 --int3 123 --int4 123 --int5 gbg");

            Assert.IsFalse(testWriter.Contains("SUCCESS"));
        }

        [TestMethod]
        public void ExecuteCommand_WithNoOptionalFlag_ExecutesCommand()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd opt");

            Assert.IsTrue(testWriter.Contains("OPTIONAL"));
        }

        [TestMethod]
        public void ExecuteCommand_WithNoCommandName_WritesResponse()
        {
            underTest.ExecuteCommand("-v");
            Assert.IsTrue(testWriter.Contains("You must enter the name of a command."));
        }

        [TestMethod]
        public void ExecuteCommand_WithInvalidCommandName_WritesResponse()
        {
            underTest.ExecuteCommand("coms");
            Assert.IsTrue(testWriter.Contains("No command with the name 'coms' exists."));
        }

        [TestMethod]
        public void ExecuteCommand_WithNoVerb_WritesResponse()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd");
            Assert.IsTrue(testWriter.Contains("The command 'cmd' requires a verb."));
        }

        [TestMethod]
        public void ExecuteCommand_WithInvalidVerb_WritesResponse()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd not");
            Assert.IsTrue(testWriter.Contains("No verb with the specified name 'not' exists."));
        }

        [TestMethod]
        public void ExecuteCommand_WithDefaultValue_Coverage()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);
            var handled = false;
            underTest.CommandExceptionEncountered += (o, a) => handled = true;
            underTest.ExecuteCommand("cmd not value");
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void ExecuteCommand_ProcessesException()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("cmd x");

            Assert.IsTrue(testWriter.Contains("TEST MESSAGE") && testWriter.Contains("System.NullReferenceException"));
        }

        [TestMethod]
        public void ExecuteCommand_Handler_ProcessesException()
        {
            var c = new InvalidInternals();
            underTest.AddCommand(c);
            var handled = false;
            EventHandler<TerminalCommandExceptionEventArgs> handler = (o, a) => handled = true;

            underTest.CommandExceptionEncountered += handler;
            underTest.ExecuteCommand("cmd x2");
            underTest.CommandExceptionEncountered -= handler;
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void GlobalHelp_IsPrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("--help");
            Assert.IsTrue(
                testWriter.Contains("TestCommand (Test)") &&
                testWriter.Contains("Test command help."));
        }

        [TestMethod]
        public void GlobalHelp_IsPrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);

            underTest.ExecuteCommand("-h");
            Assert.IsTrue(
                testWriter.Contains("TestCommand (Test)") &&
                testWriter.Contains("Test command help."));
        }

        [TestMethod]
        public void GlobalHelp_Prints_CommandNamesWithSuffix_When_SuffixPresent()
        {
            var c1 = new TestCommand3("string", "s1");
            var c2 = new TestCommand3("string", "s2");
            underTest.AddCommand(c1);
            underTest.AddCommand(c2);

            underTest.ExecuteCommand("-h");
            Assert.IsTrue(
                testWriter.Contains("TestCommand3s1 (TC3s1)") &&
                testWriter.Contains("TestCommand3s2 (TC3s2)"));
        }

        [TestMethod]
        public void VerbHelp_Sample_IsPrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand test --help");

            Assert.IsTrue(
                testWriter.Contains("TestCommand test --fluff") &&
                testWriter.Contains("Sample One"),
                "Sample One Not Found");

            Assert.IsTrue(
                testWriter.Contains("TestCommand t -f") &&
                testWriter.Contains("Sample Two"),
                "Sample Two Not Found");
        }

        [TestMethod]
        public void VerbHelp_Sample_IsPrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand test -h");

            Assert.IsTrue(
                testWriter.Contains("TestCommand test --fluff") &&
                testWriter.Contains("Sample One"),
                "Sample One Not Found");

            Assert.IsTrue(
                testWriter.Contains("TestCommand t -f") &&
                testWriter.Contains("Sample Two"),
                "Sample Two Not Found");
        }

        [TestMethod]
        public void VerbHelp_Operands_ArePrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t2 --help");

            Assert.IsTrue(
                testWriter.Contains("--fluff") &&
                testWriter.Contains("Provides fluff."),
                "Fluff operand help not found.");

            Assert.IsTrue(
                testWriter.Contains("--stuff") &&
                testWriter.Contains("Provides stuff."),
                "Stuff operand help not found.");
        }

        [TestMethod]
        public void VerbHelp_Operands_ArePrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t2 -h");

            Assert.IsTrue(
                testWriter.Contains("--fluff") &&
                testWriter.Contains("Provides fluff."),
                "Fluff operand help not found.");

            Assert.IsTrue(
                testWriter.Contains("--stuff") &&
                testWriter.Contains("Provides stuff."),
                "Stuff operand help not found.");
        }

        [TestMethod]
        public void VerbHelp_Flags_ArePrinted_When_LongHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t3 --help");

            Assert.IsTrue(
                testWriter.Contains("--fluff") &&
                testWriter.Contains("Indicates fluff."),
                "Fluff flag help not found.");

            Assert.IsTrue(
                testWriter.Contains("--stuff, -s (optional)") &&
                testWriter.Contains("Indicates stuff."),
                "Stuff flag help not found.");
        }

        [TestMethod]
        public void VerbHelp_Flags_ArePrinted_When_ShortHelp_Requested()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand t3 -h");

            Assert.IsTrue(
                testWriter.Contains("--fluff") &&
                testWriter.Contains("Indicates fluff."),
                "Fluff flag help not found.");

            Assert.IsTrue(
                testWriter.Contains("--stuff, -s (optional)") &&
                testWriter.Contains("Indicates stuff."),
                "Stuff flag help not found.");
        }

        [TestMethod]
        public void CommandHelp_Verbs_WithNoName_Show_FormattedNone()
        {
            var c = new TestCommand();
            underTest.AddCommand(c);
            underTest.ExecuteCommand("TestCommand --help");

            Assert.IsTrue(testWriter.Contains("<none>"));
        }

        [TestMethod]
        public void ValidateWriter_DoesNothing_When_NewValidator_IsNull()
        {
            underTest.WriteErrorMethod = (s) => { ConsoleBase.Write(s); };
            underTest.WriteErrorMethod("temp");
            Assert.IsTrue(testWriter.Contains("temp") && !testWriter.Contains("temp" + ConsoleBase.NewLine));

            underTest.WriteErrorMethod = null;
            underTest.WriteErrorMethod("Error");
            Assert.IsTrue(testWriter.Contains("Error"));
        }

        [TestMethod]
        public void ValidateFormatter_DoesNothing_When_NewValidator_IsNull()
        {
            underTest.FormatHelpCommandMethod = (s) => s + "1";
            var result = underTest.FormatHelpCommandMethod("temp");
            Assert.IsTrue(result == "temp1");

            underTest.FormatHelpCommandMethod = null;
            result = underTest.FormatHelpCommandMethod("temp");
            Assert.IsTrue(result == "temp");
        }

        [TestMethod]
        public void GetAllGlobalCommands_ReturnsAllGlobalCommands()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var gc2 = new GlobalCommand("sap", "Test", Access.Administrator);
            gc2.AddToConsole();

            var gc3 = new GlobalCommand("spsa", "Test", Access.Operator);
            gc3.AddToConsole();

            var values = GlobalCommand.GetAllGlobalCommands();

            Assert.IsTrue(values.Contains(gc2));
            Assert.IsTrue(values.Contains(gc3));
            Assert.IsTrue(values.Contains(underTest));
        }
    }
}
