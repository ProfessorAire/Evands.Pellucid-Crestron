using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    [TestClass]
    public class TerminalCommandBaseTests
    {
        private TestContext testContextInstance;

        private class TestBaseCommand : TerminalCommandBase
        {
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
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
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
        public void CTor_Names_MatchConstructor()
        {
            var suffix = "value";
            var tc2 = new TestCommand2(suffix);
            Assert.IsTrue(tc2.Name == "TestCommand2" + suffix);
            Assert.IsTrue(tc2.Alias == "TC2" + suffix);
        }

        [TestMethod]
        public void RegisterCommand_Returns_NoCommandAttributeFound_When_NonePresent()
        {
            var t = new TestBaseCommand();
            var result = t.RegisterCommand("test");

            Assert.IsTrue(result == RegisterResult.NoCommandAttributeFound);
        }

        [TestMethod]
        public void UnregisterCommand_ReturnsValue()
        {
            Crestron.SimplSharp.CrestronConsole.AddNewConsoleCommandResult = true;
            var t = new TestCommand();
            var gc = new GlobalCommand("ValueTest", "help", Access.Administrator);
            var added = gc.AddToConsole();
            Assert.IsTrue(added);
            var reg = t.RegisterCommand("ValueTest");
            Assert.IsTrue(reg == RegisterResult.Success);
            var result = t.UnregisterCommand("ValueTest");
            Assert.IsTrue(result);
            gc.RemoveFromConsole();
            gc.Dispose();
        }
    }
}
