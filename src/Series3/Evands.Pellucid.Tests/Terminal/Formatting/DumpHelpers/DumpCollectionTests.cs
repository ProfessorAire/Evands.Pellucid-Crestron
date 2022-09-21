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
        public void ToString_WithNullValue_Prints_UnknownType()
        {
            IDictionary<object, object> dict = null;
            var underTest = new DumpCollection(dict);
            var expected = @"
<unknown null collection> (0 Items)
-----------------------------------
-----------------------------------
";
            var actual = "\r\n" + underTest.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNullValue_ShortNames_Prints_UnknownType()
        {
            IDictionary<object, object> dict = null;
            var underTest = new DumpCollection(dict);
            var expected = @"
<unknown null collection> (0 Items)
-----------------------------------
-----------------------------------
";
            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithList_Writes_Correct()
        {
            var underTest = new DumpCollection(new List<object>() { "Item1", 2, 245.43, EventArgs.Empty});
            var expected = @"
List`1 (4 Items)
----------------
| 0: ""Item1""
| 1: 2
| 2: 245.43
| 3: EventArgs (0 Properties)
----------------
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_CurrentLevelNegative_WithList_Writes_Correct()
        {
            var underTest = new DumpCollection(new List<object>() { "Item1", 2, 245.43, EventArgs.Empty });
            var expected = @"
List`1 (4 Items)
----------------
| 0: ""Item1""
| 1: 2
| 2: 245.43
| 3: EventArgs (0 Properties)
----------------
";

            var actual = "\r\n" + underTest.ToString(0, -1, false);
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
            var expected = @"
List`1 (4 Items)
----------------
| 0: ""Item1""
| 1: 2
| 2: 245.43
| 3: TestObject1 (2 Properties)
|    --------------------------
|    | TestProperty1 = ""Test Value 1""
|    | TestProp2     = 123.321
|    --------------------------
----------------
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithArray_Writes_Correct()
        {
            var underTest = new DumpCollection(new object[] { "Item1", 2, 245.43, EventArgs.Empty });
            var expected = @"
Object[] (4 Items)
------------------
| 0: ""Item1""
| 1: 2
| 2: 245.43
| 3: EventArgs (0 Properties)
------------------
";

            var actual = "\r\n" + underTest.ToString(false);
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

            var expected = @"
Dictionary`2 (4 Items)
----------------------
| Key0   = ""Item1""
| Value0 = ""Entry1""
| Key1   = 2
| Value1 = 2
| Key2   = ""Double""
| Value2 = 245.43
| Key3   = ""EventArgs""
| Value3 = EventArgs (0 Properties)
----------------------
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithDictionary_WithObjects_Writes_Correct()
        {
            var underTest = new DumpCollection(new Dictionary<int, object>()
            {
                { 1, new TestObject1() { TestProperty1 = "Item1", TestProp2 = 2.22 } }
            });

            var expected = @"
Dictionary`2 (1 Item)
---------------------
| Key0   = 1
| Value0 = TestObject1 (2 Properties)
|          --------------------------
|          | TestProperty1 = ""Item1""
|          | TestProp2     = 2.22
|          --------------------------
---------------------
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithEmptyDictionary_Writes_Correct()
        {
            var underTest = new DumpCollection(new Dictionary<object, object>() {});

            var expected = @"
Dictionary`2 (0 Items)
----------------------
----------------------
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithEmptyEnumerable_Writes_Correct()
        {
            var underTest = new DumpCollection(new Object[0]);

            var expected = @"
Object[] (0 Items)
------------------
------------------
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithShortTypeNames_WithList_WithName_Writes_Correct()
        {
            var underTest = new DumpCollection(new int[] { 1, 2, 3, 4 }, "ItemOne");
            var expected = @"
ItemOne = Int32[] (4 Items)
          -----------------
          | 0: 1
          | 1: 2
          | 2: 3
          | 3: 4
          -----------------
";

            var actual = "\r\n" + underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_OfObjects_WithName_Writes_Correct()
        {
            var underTest = new DumpCollection(
                new List<TestObject1>()
                {
                    new TestObject1() { TestProperty1 = "Test1", TestProp2 = 23.32 },
                    new TestObject1() { TestProperty1 = "Test2", TestProp2 = 32.23 }
                }, "TestValue");

            var expected = @"
TestValue = List`1 (2 Items)
            ----------------
            | 0: TestObject1 (2 Properties)
            |    --------------------------
            |    | 0: TestProperty1 = ""Test1""
            |    | 1: TestProp2     = 23.32
            |    --------------------------
            | 1: TestObject1 (2 Properties)
            |    --------------------------
            |    | 0: TestProperty1 = ""Test2""
            |    | 1: TestProp2     = 32.23
            |    --------------------------
            ----------------
";

            var actual = expected.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Dump_WithExtended_PrintsCorrect()
        {
            var col = new DumpCollection(new string[] { "One", "Two", "Three"}, "Collection");
            var chrome = new RoundedChrome();
            Formatters.Chrome = chrome;
            var actual = col.ToString();
            ConsoleBase.WriteLineNoHeader(col.ToString());
            Formatters.Chrome = new BasicChrome();

            Assert.IsTrue(actual.Contains(chrome.BodyTopLeft) && actual.Contains(chrome.BodyLeft) && actual.Contains(chrome.BodyBottomLeft));
        }

        private class TestObject1
        {
            public string TestProperty1 { get; set; }

            public double TestProp2 { get; set; }
        }
    }
}
