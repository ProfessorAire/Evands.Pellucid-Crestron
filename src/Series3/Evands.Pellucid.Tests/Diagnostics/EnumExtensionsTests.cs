using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evands.Pellucid;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Tests extensions for enumerations.
    /// </summary>
    [TestClass]
    public class EnumExtensionsTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        public EnumExtensionsTests()
        {
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

        [TestMethod]
        public void DebugEnumeration_All_Contains_All_Tests()
        {
            var d1 = DebugLevels.All;
            Assert.IsTrue(d1.Contains(DebugLevels.All));
            Assert.IsTrue(d1.Contains(DebugLevels.Debug));
            Assert.IsTrue(d1.Contains(DebugLevels.Error));
            Assert.IsTrue(d1.Contains(DebugLevels.Exception));
            Assert.IsTrue(d1.Contains(DebugLevels.Notice));
            Assert.IsTrue(d1.Contains(DebugLevels.Progress));
            Assert.IsTrue(d1.Contains(DebugLevels.Success));
            Assert.IsTrue(d1.Contains(DebugLevels.Uncategorized));
            Assert.IsTrue(d1.Contains(DebugLevels.Warning));
        }

        [TestMethod]
        public void DebugEnumeration_All_NotContains_None_Test()
        {
            var d1 = DebugLevels.All;
            Assert.IsFalse(d1.Contains(DebugLevels.None));
        }

        [TestMethod]
        public void DebugEnumeration_Debug_Contains_Debug_Test()
        {
            var d1 = DebugLevels.Debug;
            Assert.IsTrue(d1.Contains(DebugLevels.Debug));
        }

        [TestMethod]
        public void DebugEnumeration_Debug_NotContains_Test()
        {
            var d1 = DebugLevels.Debug;
            Assert.IsFalse(d1.Contains(DebugLevels.All));
            Assert.IsFalse(d1.Contains(DebugLevels.AllButDebug));
            Assert.IsFalse(d1.Contains(DebugLevels.Error));
            Assert.IsFalse(d1.Contains(DebugLevels.Exception));
            Assert.IsFalse(d1.Contains(DebugLevels.None));
            Assert.IsFalse(d1.Contains(DebugLevels.Notice));
            Assert.IsFalse(d1.Contains(DebugLevels.Progress));
            Assert.IsFalse(d1.Contains(DebugLevels.Success));
            Assert.IsFalse(d1.Contains(DebugLevels.Uncategorized));
            Assert.IsFalse(d1.Contains(DebugLevels.Warning));
        }

        [TestMethod]
        public void LoggerEnumeration_All_Contains_All_Tests()
        {
            var d1 = LogLevels.All;
            Assert.IsTrue(d1.Contains(LogLevels.All));
            Assert.IsTrue(d1.Contains(LogLevels.AllButDebug));
            Assert.IsTrue(d1.Contains(LogLevels.Debug));
            Assert.IsTrue(d1.Contains(LogLevels.Error));
            Assert.IsTrue(d1.Contains(LogLevels.Exception));
            Assert.IsTrue(d1.Contains(LogLevels.Notice));
            Assert.IsTrue(d1.Contains(LogLevels.Warning));
        }

        [TestMethod]
        public void LoggerEnumeration_All_NotContains_None_Test()
        {
            var d1 = LogLevels.All;
            Assert.IsFalse(d1.Contains(LogLevels.None));
        }

        [TestMethod]
        public void LoggerEnumeration_Debug_Contains_Debug_Test()
        {
            var d1 = LogLevels.Debug;
            Assert.IsTrue(d1.Contains(LogLevels.Debug));
        }

        [TestMethod]
        public void LoggerEnumeration_Debug_NotContains_All_Tests()
        {
            var d1 = LogLevels.Debug;
            Assert.IsFalse(d1.Contains(LogLevels.All));
            Assert.IsFalse(d1.Contains(LogLevels.AllButDebug));
            Assert.IsFalse(d1.Contains(LogLevels.Error));
            Assert.IsFalse(d1.Contains(LogLevels.Exception));
            Assert.IsFalse(d1.Contains(LogLevels.None));
            Assert.IsFalse(d1.Contains(LogLevels.Notice));
            Assert.IsFalse(d1.Contains(LogLevels.Warning));
        }

        [TestMethod]
        public void LoggerEnumeration_None_Contains_None_Test()
        {
            var d1 = LogLevels.None;
            Assert.IsTrue(d1.Contains(LogLevels.None));
        }

        [TestMethod]
        public void LoggerEnumeration_None_NotContains_Any_Test()
        {
            var d1 = LogLevels.None;
            Assert.IsFalse(d1.Contains(LogLevels.All));
            Assert.IsFalse(d1.Contains(LogLevels.AllButDebug));
            Assert.IsFalse(d1.Contains(LogLevels.Error));
            Assert.IsFalse(d1.Contains(LogLevels.Exception));
            Assert.IsFalse(d1.Contains(LogLevels.Notice));
            Assert.IsFalse(d1.Contains(LogLevels.Warning));
        }
    }
}
