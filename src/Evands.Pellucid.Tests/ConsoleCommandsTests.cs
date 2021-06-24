﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evands.Pellucid.Terminal.Commands;

namespace Evands.Pellucid
{
    [TestClass]
    public class ConsoleCommandsTests
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
        public void Enable_EnablesColorizingConsole()
        {
            Options.Instance.ColorizeConsoleOutput = false;

            var cc = new ConsoleCommands();
            cc.Enable(true);

            Assert.IsTrue(Options.Instance.ColorizeConsoleOutput);
        }

        [TestMethod]
        public void Disable_DisablesColorizingConsole()
        {
            Options.Instance.ColorizeConsoleOutput = true;

            var cc = new ConsoleCommands();
            cc.Disable(true);

            Assert.IsFalse(Options.Instance.ColorizeConsoleOutput);
        }
    }
}