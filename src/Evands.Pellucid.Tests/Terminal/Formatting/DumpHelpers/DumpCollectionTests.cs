using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    [TestClass]
    public class DumpCollectionTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [TestInitialize]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
            Options.Instance.ColorizeConsoleOutput = false;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.ColorizeConsoleOutput = true;
        }

        public DumpCollectionTests()
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

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithList_Writes_Correct()
        {
            var underTest = new DumpCollection(new List<object>() { "Item1", 2, 245.43, EventArgs.Empty});
            var expected = @"List`1
------
""Item1""
2
245.43
EventArgs";

            var actual = underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithList_RealObjects_Writes_Correct()
        {
            var underTest = new DumpCollection(new List<object>()
            {
                "Item1",
                2,
                245.43,
                new TestObject1() { TestProperty1 = "Test Value 1", TestProp2 = 123.321 }
            });
            var expected = @"List`1
------
""Item1""
2
245.43
  TestObject1
  -----------
  TestProperty1 = ""Test Value 1""
  TestProp2     = 123.321";

            var actual = underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithArray_Writes_Correct()
        {
            var underTest = new DumpCollection(new object[] { "Item1", 2, 245.43, EventArgs.Empty });
            var expected = @"Object[]
--------
""Item1""
2
245.43
EventArgs";

            var actual = underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithDictionary_Writes_Correct()
        {
            var underTest = new DumpCollection(new Dictionary<object, object>()
            {
                { "Item1", "Entry1" },
                { 2, 2 },
                { "Double", 245.43 },
                { "EventArgs", EventArgs.Empty }
            });

            var expected = @"Dictionary`2
------------
Key0   = ""Item1""
Value0 = ""Entry1""
Key1   = 2
Value1 = 2
Key2   = ""Double""
Value2 = 245.43
Key3   = ""EventArgs""
Value3 = EventArgs";

            var actual = underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        private class TestObject1
        {
            public string TestProperty1 { get; set; }

            public double TestProp2 { get; set; }
        }
    }
}
