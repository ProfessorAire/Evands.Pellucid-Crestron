using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Evands.Pellucid
{
    public class DumpTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

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
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.ColorizeConsoleOutput = true;
        }

        public DumpTests()
        {
        }
private class EnumerableObject : IEnumerable
        {
            private object[] internalItems;

            public EnumerableObject(params object[] args)
            {
                internalItems = args;
            }

            public IEnumerator GetEnumerator()
            {
                return internalItems.GetEnumerator();
            }
        }

        private enum TestEnum
        {
            Item1,
            Item2,
            Item3,
            Item4
        }

        [Flags]
        private enum TestFlags
        {
            None = 1,
            Item1 = 2,
            Item2 = 4,
            Item3 = 8,
            All = 14
        }

        private class TestWithSameName
        {
            public TestWithSameName()
            {
                Name = string.Empty;
            }

            public override string ToString()
            {
                return "TestWithSameName";
            }

            public string Name { get; set; }
        }

        private class TestObject
        {
            public TestObject(string name)
            {
                Name = name;
                Number = 42;
                Enumerable = new EnumerableObject("Object 1", 2, new object(), 8743L);
                EmptyEnumerable = new EnumerableObject();
                List = new List<string>() { "List1", "List2", "List3" };
                Collection = new List<object>() { "Collection1", 13245, new List<string>() { "CollSub1", "CollSub2" }, new List<string>() };
                OtherValue = 16;
                TestEnumeration = TestEnum.Item3;
                TestFlagEnumeration = TestFlags.Item1 | TestFlags.Item2;
                TestFlagEnumerationTwo = TestFlags.All;
                TestWithSameName = new TestWithSameName();
                TestDictionary = new Dictionary<int, string>() { { 123, "Value One" }, { 234, "Value Two Three Four" } };
                TestDictionary2 = new Dictionary<int,TestWithSameName>()
                {
                    { 1, new TestWithSameName() { Name = "Nothing" } },
                    { 2, new TestWithSameName() { Name = "Much" } }
                };

                SameName = new TestWithSameName() { Name = "ThirdLevel" };
            }

            public string Name { get; set; }

            public object NullValue { get; set; }

            public int Number { get; set; }

            public IEnumerable Enumerable { get; set; }

            public static IEnumerable EmptyEnumerable { get; set; }

            public TestEnum TestEnumeration { get; set; }

            public static TestFlags TestFlagEnumeration { get; set; }

            public static TestFlags TestFlagEnumerationTwo { get; set; }

            public IList List { get; set; }

            public ICollection Collection { get; set; }

            public static ushort OtherValue { get; set; }

            public static TestWithSameName TestWithSameName { get; set; }

            public static TestWithSameName NullTestWithSameName { get; set; }

            public static Dictionary<int, string> TestDictionary { get; set; }

            public static Dictionary<int, TestWithSameName> TestDictionary2 { get; set; }

            public TestWithSameName SameName { get; set; }
        }

        private class TestDepthOne
        {
            public string TestValue { get { return "TestValue"; } }
            public TestObject TestObject {  get { return new TestObject("testName"); } }
        }

        private bool ContainsText(string value)
        {
            return this.writer.Messages.Any(m => m.Contains(value));
        }

        [Test]
        public async Task Dump_WritesCorrectDepth_One()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            var d1 = new TestDepthOne();
            d1.Dump(1);
            await Assert.That(ContainsText("TestValue") && !ContainsText("42")).IsTrue();
        }

        [Test]
        public async Task Dump_WritesCorrectDepth_Two()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            var d1 = new TestDepthOne();
            d1.Dump(2);
            await Assert.That(ContainsText("TestValue") && ContainsText("42") && !ContainsText("ThirdLevel")).IsTrue();
        }

        [Test]
        public async Task Dump_WritesCorrectDepth_Three()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            var d1 = new TestDepthOne();
            d1.Dump(3);
            await Assert.That(ContainsText("TestValue") && ContainsText("42") && ContainsText("ThirdLevel")).IsTrue();
        }

        [Test]
        public async Task Dump_WritesCorrectDepth_Zero()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            var d1 = new TestDepthOne();
            d1.Dump(0);
            await Assert.That(ContainsText("TestValue") && ContainsText("42") && ContainsText("ThirdLevel")).IsTrue();
        }

        [Test]
        public async Task Dump_WritesNull_WithNullObject()
        {
            string testValue = null;
            var expected = "<null>";
            testValue.Dump();

            await Assert.That(ContainsText(expected)).IsTrue();
        }

        [Test]
        public async Task Dump_WritesCorrect_WithNullProperty()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            var testValue = new TestObject("test");
            var expected = "<null>";
            testValue.Dump();

            await Assert.That(ContainsText(expected)).IsTrue();
        }

        [Test]
        public async Task Dump_Writes_Dictionary_Correct()
        {
            Options.Instance.ColorizeConsoleOutput = false;
            var testValue = new Dictionary<int, TestWithSameName>()
                {
                    { 1, new TestWithSameName() { Name = "Nothing" } },
                    { 2, new TestWithSameName() { Name = "Much" } }
                };

            var expected =
@"System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[Evands.Pellucid.DumpTests+TestWithSameName, Evands.Pellucid.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]] (2 Items)
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
| Key0   = 1
| Value0 = Evands.Pellucid.DumpTests+TestWithSameName (1 Property)
|          -------------------------------------------------------
|          | Name = ""Nothing""
|          -------------------------------------------------------
| Key1   = 2
| Value1 = Evands.Pellucid.DumpTests+TestWithSameName (1 Property)
|          -------------------------------------------------------
|          | Name = ""Much""
|          -------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
";
            testValue.Dump(true);

            await Assert.That(ContainsText(expected)).IsTrue();

            writer.Messages.Clear();

            var testValue2 = new Dictionary<int, string>() { { 1, "Value 1" }, { 2, "Value 2" }, { 3, "Value 3" } };

            expected =
@"Dictionary`2 (3 Items)
----------------------
| Key0   = 1
| Value0 = ""Value 1""
| Key1   = 2
| Value1 = ""Value 2""
| Key2   = 3
| Value2 = ""Value 3""
----------------------
";

            testValue2.Dump();

            await Assert.That(ContainsText(expected)).IsTrue();
        }
    }
}
