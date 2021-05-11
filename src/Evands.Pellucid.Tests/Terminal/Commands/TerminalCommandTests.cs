using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crestron.SimplSharp;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Summary description for TerminalCommandTests
    /// </summary>
    [TestClass]
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

        private TestContext testContextInstance;

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
             global = new GlobalCommand("app", "App Command", Access.Administrator);
             command = new TestCommand();
             global.AddCommand(command);
             global.AddCommand(new TestCommand2());
         }
        
         // Use TestCleanup to run code after each test has run
         [TestCleanup()]
         public void MyTestCleanup()
         {
             global.RemoveCommand(command);
             global = null;
             command = null;
             CrestronConsole.Messages.Length = 0;
             if (writer != null)
             {
                 ConsoleBase.UnregisterConsoleWriter(writer);
             }
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
        public void Command_WithAlias_CalledByAlias_Functions()
        {
            var expected = "Does some thing.";
            global.ExecuteCommand("test verbone");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void Command_WithAlias_AskedForHelp_PrintsAliasCorrectly()
        {
            var expected = "TestCommand (Test)";
            global.ExecuteCommand("test -h");
            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void Verb_WithSubstringAlias_CalledByAlias_Functions()
        {
            var expected = "Does some thing.";
            global.ExecuteCommand("test ver");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void Verb_WithSubstringAlias_AskedForHelp_PrintsAliasCorrectly()
        {
            var expected = "VerbOne (Ver)";
            global.ExecuteCommand("test ver -h");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void Verb_WithStringAlias_CalledByAlias_Functions()
        {
            var expected = "Does some thing2.";
            global.ExecuteCommand("test v2");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void Verb_WithStringAlias_AskedForHelp_PrintsAliasCorrectly()
        {
            var expected = "(v2)";
            global.ExecuteCommand("test v2 -h");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void DefaultVerb_Functions()
        {
            var expected = "Does some default thing.";
            global.ExecuteCommand("test");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void CommandAlias_With_DefaultVerb_Functions()
        {
            var expected = "Default test command executed.";
            global.ExecuteCommand("tc2");

            Assert.IsTrue(CrestronConsole.Messages.ToString().Contains(expected));
        }

        [TestMethod]
        public void CommandAlias_Shows_InTopLevelHelp()
        {
            var expected = "(Test)";
            Options.Instance.ColorizeConsoleOutput = false;
            writer = new TestConsoleWriter();
            ConsoleBase.RegisterConsoleWriter(writer);
            global.ExecuteCommand("-h");
            Assert.IsTrue(writer.Contains(expected));
            Options.Instance.ColorizeConsoleOutput = true;
        }
    }
}
