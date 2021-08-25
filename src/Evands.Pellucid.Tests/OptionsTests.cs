using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid
{
    [TestClass]
    public class OptionsTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [TestInitialize]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
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

        [TestMethod]
        public void UseDefault_CreatesDefaults_WithAutoSave_Disabled()
        {
            Options.UseDefault();
            Assert.IsTrue(Options.Instance.ColorizeConsoleOutput);
            Assert.IsTrue(Options.Instance.UseTimestamps);
            Assert.IsTrue(Options.Instance.Use24HourTime);
            Assert.AreEqual(Options.Instance.LogLevels, Evands.Pellucid.Diagnostics.LogLevels.None);
            Assert.AreEqual(Options.Instance.DebugLevels, Evands.Pellucid.Diagnostics.DebugLevels.All);
            Assert.IsTrue(Options.Instance.Suppressed.Count == 0);
            Assert.IsTrue(Options.Instance.Allowed.Count == 0);
            Assert.IsFalse(Options.Instance.AutoSave);
        }
    }
}
