using System;
using System.Collections.Generic;
namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    public class DumpObjectTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(this.writer);
            Options.Instance.ColorizeConsoleOutput = false;
        }

        [After(Test)]
        public void TestCleanup()
        {
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            this.writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(this.writer);
            ConsoleBase.OptionalHeader = string.Empty;
        }

        public DumpObjectTests()
        {
        }
        private class TestClass
        {
            public string FirstProperty { get; set; }

            public string SecondProperty { get; set; }

            public int ThirdProperty { get; set; }

            public bool FourthProperty { get; set; }
        }

        private class TestClass2
        {
            public double First { get; set; }

            public TestClass Second { get; set; }
        }

        private class TestClass3
        {
            public bool One { get; set; }

            public string Two { get; set; }

            public int Three { get; set; }

            public TestClass2 Fourth { get; set; }
        }

        [Test]
        public async Task Ctor_Object_SetsValue()
        {
            var expected = new Object();
            var underTest = new DumpObject(expected);

            await Assert.That(underTest.Value).IsEqualTo(expected);
        }

        [Test]
        public async Task Ctor_Object_Name_SetsValue_And_SetsName()
        {
            var expectedValue = new Object();
            var expectedName = "Test Name";
            var underTest = new DumpObject(expectedValue, expectedName);

            await Assert.That(underTest.Value).IsEqualTo(expectedValue);
            await Assert.That(underTest.Name).IsEqualTo(expectedName);
        }

        [Test]
        public async Task Ctor_NullValue_Name_SetsValue_And_SetsName()
        {
            object expectedValue = null;
            var expectedName = "Test Name";
            var underTest = new DumpObject(null, expectedName);

            await Assert.That(underTest.Value).IsEqualTo(expectedValue);
            await Assert.That(underTest.Name).IsEqualTo(expectedName);
        }

        [Test]
        public async Task Ctor_Object_Name_Type_SetsValue_And_SetsName_And_SetsType()
        {
            var expectedValue = new Object();
            var expectedName = "Test Name";
            var expectedType = typeof(ArgumentNullException);
            var underTest = new DumpObject(expectedValue, expectedName, expectedType);

            await Assert.That(underTest.Value).IsEqualTo(expectedValue);
            await Assert.That(underTest.Name).IsEqualTo(expectedName);
            await Assert.That(underTest.ValueType).IsEqualTo(expectedType);
        }

        [Test]
        public async Task ToString_NullValueType_FullNames_Writes_Correct()
        {
            var underTest = new DumpObject(new Object(), null, null);
            var expected = @"
<unknown type> (0 Properties)
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString(true);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_NullValueType_ShortNames_Writes_Correct()
        {
            var underTest = new DumpObject(new Object(), null, null);
            var expected = @"
<unknown type> (0 Properties)
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString(false);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_MaxDepth_NegativeCurrentDepth_ShortNames_Writes_Correct()
        {
            var underTest = this.GetObject();
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
----------------------------------------------------------------------------------------
| FirstProperty  = ""First""
| SecondProperty = ""Second""
| ThirdProperty  = 3
| FourthProperty = True
----------------------------------------------------------------------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString(1, -10, true);

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithNoPadding_WithBasicProperties_Writes_Correct()
        {
            var underTest = this.GetObject();
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
----------------------------------------------------------------------------------------
| FirstProperty  = ""First""
| SecondProperty = ""Second""
| ThirdProperty  = 3
| FourthProperty = True
----------------------------------------------------------------------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString(true);

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithNoPadding_WithNestedProperties_Writes_Correct()
        {
            var underTest = new DumpObject(new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) });
            var expected = @"
TestClass2 (2 Properties)
-------------------------
| First  = 2.5
| Second = TestClass (4 Properties)
|          ------------------------
|          | FirstProperty  = ""First""
|          | SecondProperty = ""Second""
|          | ThirdProperty  = 3
|          | FourthProperty = False
|          ------------------------
-------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString();

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithNoPadding_WithDeepNestedProperties_Writes_Correct()
        {
            var underTest = new DumpObject(
                new TestClass3()
                {
                    One = true,
                    Two = "Two",
                    Three = 3,
                    Fourth = new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) }
                });
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3 (4 Properties)
-----------------------------------------------------------------------------------------
| One    = True
| Two    = ""Two""
| Three  = 3
| Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2 (2 Properties)
|          -----------------------------------------------------------------------------------------
|          | First  = 2.5
|          | Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
|          |          ----------------------------------------------------------------------------------------
|          |          | FirstProperty  = ""First""
|          |          | SecondProperty = ""Second""
|          |          | ThirdProperty  = 3
|          |          | FourthProperty = False
|          |          ----------------------------------------------------------------------------------------
|          -----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString(true);

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithNoPadding_WithDeepNestedProperties_WritesTwoLevels_Correct()
        {
            var underTest = new DumpObject(
                new TestClass3()
                {
                    One = true,
                    Two = "Two",
                    Three = 3,
                    Fourth = new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) }
                });

            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3 (4 Properties)
-----------------------------------------------------------------------------------------
| One    = True
| Two    = ""Two""
| Three  = 3
| Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2 (2 Properties)
|          -----------------------------------------------------------------------------------------
|          | First  = 2.5
|          | Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
|          -----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString(2, true);

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_With_ListProperty_Prints_Correct()
        {
            var underTest = new DumpObject(
                new TestClassWithList()
                {
                    IntList = new List<int>() { 0, 1, 2, 3 },
                    StringList = new List<string>() { "OneItem" }
                });

            var expected = @"
TestClassWithList (2 Properties)
--------------------------------
| IntList    = List`1 (4 Items)
|              ----------------
|              | 0: 0
|              | 1: 1
|              | 2: 2
|              | 3: 3
|              ----------------
| StringList = List`1 (1 Item)
|              ---------------
|              | 0: ""OneItem""
|              ---------------
--------------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString();

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_WithMinimalSpacing_With_ListProperty_Prints_Correct()
        {
            Options.Instance.UseMinimalSpacingWhenDumping = true;

            var underTest = new DumpObject(
                new TestClassWithList()
                {
                    IntList = new List<int>() { 0, 1, 2, 3 },
                    StringList = new List<string>() { "OneItem" }
                });

            var expected = @"
TestClassWithList (2 Properties)
--------------------------------
| IntList    = List`1 (4 Items)
|  ----------------
|  | 0: 0
|  | 1: 1
|  | 2: 2
|  | 3: 3
|  ----------------
| StringList = List`1 (1 Item)
|  ---------------
|  | 0: ""OneItem""
|  ---------------
--------------------------------
";

            var actual = "\r\n" + underTest.ToString();

            Options.Instance.UseMinimalSpacingWhenDumping = false;

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_With_GetPropertyValueFailures_AddsFailureObjects()
        {
            var underTest = new DumpObject(new TestFailureClass());

            var expected = @"
TestFailureClass (1 Property)
-----------------------------
| DumpFailures = List`1 (2 Items)
|                ----------------
|                | 0: DumpObjectFailure (4 Properties)
|                |    --------------------------------
|                |    | PropertyName     = ""InstancePropertyOne""
|                |    | ErrorMessage     = ""Exception encountered while dumping property value.""
|                |    | ExceptionMessage = ""Exception has been thrown by the target of an invocation.""
|                |    | ExceptionType    = ""System.Reflection.TargetInvocationException""
|                |    --------------------------------
|                | 1: DumpObjectFailure (4 Properties)
|                |    --------------------------------
|                |    | PropertyName     = ""StaticPropertyOne""
|                |    | ErrorMessage     = ""Exception encountered while dumping property value.""
|                |    | ExceptionMessage = ""Exception has been thrown by the target of an invocation.""
|                |    | ExceptionType    = ""System.Reflection.TargetInvocationException""
|                |    --------------------------------
|                ----------------
-----------------------------
".Replace("\r\n", "\n").Replace("\n", "\r\n");

            var actual = "\r\n" + underTest.ToString();

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_With_ChildPropertySameAsSelf_Does_Not_DumpChild()
        {
            var underTest = new DumpObject(Options.Instance);
            await Assert.That(underTest.ToString().Contains("Instance")).IsFalse();
        }

        [Test]
        public async Task ToString_WithMinimalSpacing_WithDeepNestedProperties_Writes_Correct()
        {
            Options.Instance.UseMinimalSpacingWhenDumping = true;

            var underTest = new DumpObject(
                new TestClass3()
                {
                    One = true,
                    Two = "Two",
                    Three = 3,
                    Fourth = new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) }
                });
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3 (4 Properties)
-----------------------------------------------------------------------------------------
| One    = True
| Two    = ""Two""
| Three  = 3
| Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2 (2 Properties)
|  -----------------------------------------------------------------------------------------
|  | First  = 2.5
|  | Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
|  |  ----------------------------------------------------------------------------------------
|  |  | FirstProperty  = ""First""
|  |  | SecondProperty = ""Second""
|  |  | ThirdProperty  = 3
|  |  | FourthProperty = False
|  |  ----------------------------------------------------------------------------------------
|  -----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
";

            var actual = "\r\n" + underTest.ToString(true);

            Options.Instance.UseMinimalSpacingWhenDumping = false;

            await Assert.That(actual).IsEqualTo(expected);
        }

        private DumpObject GetObject()
        {
            return this.GetObject("First", "Second", 3, true);
        }

        private DumpObject GetObject(string one, string two, int three, bool four)
        {
            return new DumpObject(this.GetTestClass(one, two, three, four));
        }

        private TestClass GetTestClass(string one, string two, int three, bool four)
        {
            return new TestClass()
            {
                FirstProperty = one,
                SecondProperty = two,
                ThirdProperty = three,
                FourthProperty = four
            };
        }

        private class TestClassWithList
        {
            public List<int> IntList { get; set; }

            public List<string> StringList { get; set; }
        }

        private class TestFailureClass
        {
            public string InstancePropertyOne { get { throw new NotImplementedException("InstancePropertyOne has not been implemented."); } }

            public static string StaticPropertyOne { get { throw new NotImplementedException("StaticPropertyOne has not been implemented."); } }
        }
    }
}
