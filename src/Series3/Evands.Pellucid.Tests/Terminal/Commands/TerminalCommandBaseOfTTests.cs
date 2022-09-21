using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    [TestClass]
    public class TerminalCommandBaseOfTTests
    {
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
        public void CTor_TargetObject_MatchesConstructor()
        {
            var expected = "value";
            var tc3 = new TestCommand3(expected);
            Assert.IsTrue(tc3.TargetObject == expected);
        }

        [TestMethod]
        public void CTor_TargetObject_And_Name_MatchesConstructor()
        {
            var expectedTarget = "value";
            var expectedName = "TestCommand3Suffix";
            var tc3 = new TestCommand3(expectedTarget, "Suffix");
            Assert.IsTrue(tc3.TargetObject == expectedTarget);
            Assert.IsTrue(tc3.Name == expectedName);
        }
    }
}
