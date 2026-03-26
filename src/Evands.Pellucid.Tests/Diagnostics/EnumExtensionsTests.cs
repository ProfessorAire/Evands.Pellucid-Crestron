using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Evands.Pellucid;

namespace Evands.Pellucid.Diagnostics
{
    /// <summary>
    /// Tests extensions for enumerations.
    /// </summary>
    public class EnumExtensionsTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        public EnumExtensionsTests()
        {
        }

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
        }

        [After(Test)]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
        }

        [Test]
        public async Task DebugEnumeration_All_Contains_All_Tests()
        {
            var d1 = DebugLevels.All;
            await Assert.That(d1.Contains(DebugLevels.All)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Debug)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Error)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Exception)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Notice)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Progress)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Success)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Uncategorized)).IsTrue();
            await Assert.That(d1.Contains(DebugLevels.Warning)).IsTrue();
        }

        [Test]
        public async Task DebugEnumeration_All_NotContains_None_Test()
        {
            var d1 = DebugLevels.All;
            await Assert.That(d1.Contains(DebugLevels.None)).IsFalse();
        }

        [Test]
        public async Task DebugEnumeration_Debug_Contains_Debug_Test()
        {
            var d1 = DebugLevels.Debug;
            await Assert.That(d1.Contains(DebugLevels.Debug)).IsTrue();
        }

        [Test]
        public async Task DebugEnumeration_Debug_NotContains_Test()
        {
            var d1 = DebugLevels.Debug;
            await Assert.That(d1.Contains(DebugLevels.All)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.AllButDebug)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Error)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Exception)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.None)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Notice)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Progress)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Success)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Uncategorized)).IsFalse();
            await Assert.That(d1.Contains(DebugLevels.Warning)).IsFalse();
        }

        [Test]
        public async Task LoggerEnumeration_All_Contains_All_Tests()
        {
            var d1 = LogLevels.All;
            await Assert.That(d1.Contains(LogLevels.All)).IsTrue();
            await Assert.That(d1.Contains(LogLevels.AllButDebug)).IsTrue();
            await Assert.That(d1.Contains(LogLevels.Debug)).IsTrue();
            await Assert.That(d1.Contains(LogLevels.Error)).IsTrue();
            await Assert.That(d1.Contains(LogLevels.Exception)).IsTrue();
            await Assert.That(d1.Contains(LogLevels.Notice)).IsTrue();
            await Assert.That(d1.Contains(LogLevels.Warning)).IsTrue();
        }

        [Test]
        public async Task LoggerEnumeration_All_NotContains_None_Test()
        {
            var d1 = LogLevels.All;
            await Assert.That(d1.Contains(LogLevels.None)).IsFalse();
        }

        [Test]
        public async Task LoggerEnumeration_Debug_Contains_Debug_Test()
        {
            var d1 = LogLevels.Debug;
            await Assert.That(d1.Contains(LogLevels.Debug)).IsTrue();
        }

        [Test]
        public async Task LoggerEnumeration_Debug_NotContains_All_Tests()
        {
            var d1 = LogLevels.Debug;
            await Assert.That(d1.Contains(LogLevels.All)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.AllButDebug)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Error)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Exception)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.None)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Notice)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Warning)).IsFalse();
        }

        [Test]
        public async Task LoggerEnumeration_None_Contains_None_Test()
        {
            var d1 = LogLevels.None;
            await Assert.That(d1.Contains(LogLevels.None)).IsTrue();
        }

        [Test]
        public async Task LoggerEnumeration_None_NotContains_Any_Test()
        {
            var d1 = LogLevels.None;
            await Assert.That(d1.Contains(LogLevels.All)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.AllButDebug)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Error)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Exception)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Notice)).IsFalse();
            await Assert.That(d1.Contains(LogLevels.Warning)).IsFalse();
        }
    }
}
